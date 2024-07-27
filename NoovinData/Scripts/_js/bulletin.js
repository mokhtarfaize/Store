$(function() {   
   $(document).mousemove(function(e) {
       //get mouse location
       var cursorX = e.pageX;
       var cursorY = e.pageY;
       
       //move background image based on cursor position
       $('#bulletin').css('background-position', (-cursorX)  / 50 + 'px ' + (-cursorY / 50) + 'px');
   });//end mouse move
});//end document ready