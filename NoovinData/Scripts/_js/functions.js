//////////////////////////////////////////////////////////////////////////////////////
//show modal
function show_modal(){
	$('.modal').fadeIn(200);
	$('.modal .bg').css("opacity",'.3');
	$('body').css({'overflow':'hidden'});
}//show_modal

//////////////////////////////////////////////////////////////////////////////////////
//hide modal
function hide_modal(id){
	$('.modal').fadeOut(500);
	$('.modal .bg').css('opacity','0');
	$('body').css({'overflow':'auto'});
	
	
	setTimeout(function(){
		$('.modal_dynamic .window').remove();
		$('.modal_static '+id).css({'display':'none','opacity':'0'})
	},500);
}//hide_modal

//////////////////////////////////////////////////////////////////////////////////////
//modal msg
function modal_msg(type,msg,btns){
	show_modal();
	$('.modal_scroll .modal_dynamic').html("<div class='window'><i class='modal_close'>X</i><div class='modal_header "+type+" '><i class='modal_icon'></i></div><div class='modal_msg'><p>"+msg+"</p></div><div class='modal_action'><a href='#' class='modal_close_btn modal_btn'>بستن</a>"+btns+"</div></div>");
	$('.modal_scroll .modal_dynamic .window').css({'display':'inline-block'}).stop(true,true).fadeTo(500,1);
	
	$('.modal_close, .modal_close_btn').click(function(){
		hide_modal();
	});
}//modal_msg


//////////////////////////////////////////////////////////////////////////////////////
//modal box
function modal_box(id){
	show_modal();
	$('.modal_static '+id).css({'display':'inline-block'}).stop(true,true).fadeTo(500,1);
	
	$('.modal_close, .modal_close_btn').click(function(){
		hide_modal(id);
	});
}//modal_msg


//////////////////////////////////////////////////////////////////////////////////////
//show notif
function show_notif($title,$text,$type,$time){ 
	switch($type){
		case 's':
			$type='success';
		break;
		
		case 'w':
			$type='warning';
		break;
		
		case 'e':
			$type='error';
		break;
		
		case 'i':
			$type='info';
		break;
	}//switch
	
	$time = ($time==0)?false:$time;
	
	$.toast({
		heading: $title,
		text: $text,
		showHideTransition: 'fade',
		icon: $type,
		hideAfter: $time,
		stack:1,
	});
}//show_notif

function clean_reload(){
	$('.clean_reload select option').removeAttr("selected");
	$('.clean_reload select option').first().prop("selected",true);
	$('.clean_reload input[type=text],textarea').val("");		
}

//Preview Image Function
function preview($file,$target){
	
	//show image befor upload
	$($file).on("change", function()
	{
		var files = !!this.files ? this.files : [];
		if (!files.length || !window.FileReader) return; // no file selected, or no FileReader support
	
		if (/^image/.test( files[0].type)){ // only image file
			var reader = new FileReader(); // instance of the FileReader
			reader.readAsDataURL(files[0]); // read the local file
	
			reader.onloadend = function(){ // set image data as background of div
				$($target).html("<img src='"+this.result+"'>").show();
			}
		}
	});
}

////////////////////////////////////////////////////////////////////////////
// ellipsis
function ellipsis(){
	$(".ellipsis").dotdotdot({
		ellipsis	: '... ',
		wrap		: 'word',
		fallbackToLetter: true,
		after		: null,
		watch		: window,	
		height		: null,
		tolerance	: 0,
		callback	: function( isTruncated, orgContent ) {},
		lastCharacter	: {
			remove		: [ ' ', ',', ';', '.', '!', '?' ],
			noEllipsis	: []
		}
	});// ellipsis
}



////////////////////////////////////////////////////////////////////////////////////////
//Date Picker
function date_picker($element){
	Calendar.setup({
		inputField: $element,
		button: 'date_btn',
		ifFormat: '%Y/%m/%d',
		dateType: 'jalali'
	});
	$("input#"+$element).keypress(function(e){
		e.preventDefault();
	});	
}//date_picker






























