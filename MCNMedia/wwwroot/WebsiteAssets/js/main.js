$('.select2').select2();

$('.cover').on('click', function () {
    $(this).children().css({
      'z-index' : 1,
      'opacity': 1
    });
  $(this).children().trigger('play');
     
});

$('video').on('click', function () {
});

jQuery(document).ready(function($) {
    "use strict";
    //  TESTIMONIALS CAROUSEL HOOK
    $('#customers-testimonials').owlCarousel({
        loop: true,
        center: true,
        items: 3,
        margin: 0,
        autoplay: true,
        dots:false,
        autoplayTimeout: 8500,
        smartSpeed: 450,
        responsive: {
          0: {
            items: 1
          },
          768: {
            items: 2
          },
          1170: {
            items: 3
          }
        }
    });
});

