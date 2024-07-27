
$(function() {
	//////////////////////////////////////////////////////////////////////////////////////
        //toggle user box
	$('#user_account_btn').click(function(e) {
		$('#user_account_box').stop().slideToggle(300);
		$('#user_account_btn').toggleClass('user_account_btn_hover');
		
		return false;
	});//end user_account_btn click
	
	
	//close the user_account_box if user clicks anywhere
	$(document).click(function(e) {
		
		var container = $("#user_account_box");
		
		if (!container.is(e.target) &&
			container.has(e.target).length === 0) {
				container.slideUp();	
				$('#user_account_btn').removeClass('user_account_btn_hover');
		}
	});//end document click
	
});//end document ready