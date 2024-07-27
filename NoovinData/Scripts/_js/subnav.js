$(document).ready(function() {

    $('.has_subnav').hover(function() {
        $(this).find('.subnav').stop().slideDown('fast');
        
    }, function() {
        $(this).find('.subnav').stop().slideUp('fast');
    });
    
});//end document ready