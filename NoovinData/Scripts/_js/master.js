$(document).ready(function() {
	//Global Variables
	$froot=$('#froot').val();
	$curpage=$('#curpage').val();
	
	
	//smooth scroll
	//jQuery.scrollSpeed(100, 800);
	
	//disable # anchors
	$('body').on('click','a[href=#]',function(e){
		e.preventDefault();
	});
    
    $('[data-backurl]').click(function(e){
		e.preventDefault();
        parent.history.back();
        return false;
    });
	
	$('input[type=text]').keypress(function(){
		$this = $(this);
		/*if($this.val().length == 1)
		{*/
			var x =  new RegExp("[\x00-\x80]+"); // is ascii
			
			var isAscii = x.test($this.val().substr($this.val().length-1));
			
			if(isAscii)
			{
				$this.css("direction", "ltr");
			}
			else
			{
				$this.css("direction", "rtl");
			}
		//}
			
	});
		
	
	
	
	$('.tooltip').parent().hover(function(){
		$(this).find('.tooltip').stop(true,true).fadeIn(200);
	},function(){
		$(this).find('.tooltip').stop(true,true).fadeOut(200);
	});
	


	//Change State & City
	$("select[name=state]").change(function(){
		var $this = $(this);
		var state_id = $(this).val();
		$.post("_includes/pages/ajax_process.php",{
			state_id:state_id,
			go_change_city:""
		},function(response){
			$this.closest("form").find("select[name=city]").html(response);
		});
	});//Change State & City
	
	//number_format inputs
	$("input.number_format").keyup(function(){
		var num = this.value.replace(/[^\d]/g, '');
		if(num.length>3)
			num = num.replace(/\B(?=(?:\d{3})+(?!\d))/g, ',');
		this.value = num;
	});	
	
	
	/////////////////////////
	//services_page  question
	
	$('div.question').click(function(){
	if($(this).find('i').hasClass('i-plus')){
		$(this).find('i').removeClass('i-plus');
		$(this).find('i').addClass('i-minus');
		$(this).siblings('div.answer').fadeIn(500);	
		/*$(this).css('color','#20A69A');*/
	}
	else{
		$(this).siblings('div.answer').fadeOut(100);
		$(this).find('i').removeClass('i-minus');
		$(this).find('i').addClass('i-plus');
		/*$(this).css('color','#898989');*/
			
	}
	
	
	});
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////		
	//user_panel brows pic_product and show image in div	
	$('.file_style1 input[type=file]').change(function(){
		var file_name =$(this).val();
		$(this).siblings('input[type=text]').val(file_name);
	});
	$('.file_style1 input[type=file]').val('');
	//preview('.file_style1 input[type=file]',$('.file_style1').siblings("#show_pic"));//show image in div
	
	
	
	////////////////////////////////////////////////////////////////////////////////
	// TOP AJAX SEATCH
	top_search_height = ($(window).height() < 400)?400:$(window).height()-200;
	$("#search_result_box").css("max-height",top_search_height);
	$("#search_result_box li").hover(function(){
		$("#search_result_box li").css('opacity','.75');
		$(this).css('opacity','1');
	},function(){
		$("#search_result_box li").css('opacity','1');
	});
	
	//search
	$("#frm_topsearch").submit(function(e){
		e.preventDefault();
	});
	$("#frm_topsearch input[name=keyword],#frm_topsearch select").on('keyup change',function(){
		var keyword = $("#frm_topsearch input[name=keyword]").val();
		var cat1_id = $("#frm_topsearch select[name=cat1_id]").val();
		if(keyword.trim() != ''){
			//$("#search_result_box .list").html("<li class='loader'></li>");
			$("#search_result_box").css('display','block'); 
			$.post("_includes/ajax-process.php",{
				keyword:keyword,
				cat1_id:cat1_id,
				go_ajax_search:''
			},function(response){
			   $("#search_result_box").html(response);
			});
		}//(keyword.trim() != '')
		else{
		   $("#search_result_box").css('display','none'); 
		}
	});
	$("#frm_topsearch input[name=keyword]").click(function(){
		var val = $(this).val();
		
		if(val != ''){
			$("#search_result_box").css('display','block'); 
		}
	});
	$("header .top_search_form").click(function(e){
		e.stopPropagation(); 
	});
	$("#frm_topsearch input[name=keyword]").click(function(e){
        e.stopPropagation(); 
        if($("#search_result_box div").length > 0){
            $("#search_result_box").css('display','block'); 
        }
    });
	$('html').click(function(){
		$("#search_result_box").css('display','none'); 
	});
	
	
	
	
	
	
	
	////////////////////////////////////////////////////////////////////////////
    // ellipsis
    ellipsis();
	
	
	

});//document ready
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	