//global variables
var slideshow_handle;
var curImageIndex;
var curBulletIndex;
var hovered_bullet;

$(function() {
	
	//apply .current to the first image
	$('.slideshow_pics .item:first').addClass('current');
	
	//highlight related bullet	
	highlighBullet();
	
	//start slideshow
	slideshow_handle = setInterval(slideshow, 4000);
	
	/******************** bullets hover ********************/
	$('.slideshow_bullets li').hover(function() {
		clearInterval(slideshow_handle);		
		
		/*alert(curBulletIndex);*/
		
		hovered_bullet = $(this).index();
		
		
		$('.slideshow_pics .item:eq(' + hovered_bullet + ')').stop().hide().addClass('current_hovered').fadeIn();
		
		//remove highlight from current bullet	
		$('.slideshow_bullets li').removeClass('cur_bullet');
		
		//alert(hovered_bullet);
		$('.slideshow_bullets li:eq(' + hovered_bullet + ')').addClass('current_hovered');
		
	},
	function() {
		$('.slideshow_pics .item').stop().removeClass('current');
		$('.slideshow_bullets li').removeClass('cur_bullet current_hovered');
		$('.slideshow_bullets li:eq(' + hovered_bullet + ')').addClass('cur_bullet');

		//apply .current to the first image
		$('.slideshow_pics .item:eq(' + hovered_bullet + ')').stop().removeClass('current_hovered').addClass('current').fadeIn();

		slideshow_handle = setInterval(slideshow, 4000);
		
	});
	
});//end document ready


//function to create slideshow
function slideshow() {
	
	//get current and next images
	var currentimg = $('.item.current');
	var nextimg = currentimg.next('.item');
	
	//check if end
	if (nextimg.length == 0) {
		nextimg = $('.slideshow_pics .item:first');	
	}//end check if end
	
	currentimg.removeClass('current').addClass('previous');
	nextimg.addClass('current').hide().fadeIn(1000, function() {
		currentimg.removeClass('previous');	
	});//end callback
	
	//highlight related bullet	
	highlighBullet();
}//end slideshow()


//function to highlight related bullet
function highlighBullet() {
	//remove class from all bullets
	$('.slideshow_bullets li').removeClass('cur_bullet');
	
	/*var curImageIndex = $('.slideshow_pics .item.current').index();*/
	curImageIndex = $('.slideshow_pics .item.current').index();
	
	$('.slideshow_bullets li').each(function() {
		
		//get current bullet index
		/*curBulletIndex = $(this).index();*/
		
		var bulletIndex = $(this).index();
		
		if (curImageIndex == bulletIndex) {			
			$(this).addClass('cur_bullet');
			curBulletIndex = bulletIndex;			
		}		
		
	});//end slideshow_bullets each
	
}//end highlightBullet()












