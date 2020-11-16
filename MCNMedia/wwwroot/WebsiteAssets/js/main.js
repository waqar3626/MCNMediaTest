$('.select2').select2();

$('.cover').on('click', function () {
    $(this).children().css({
        'z-index': 1,
        'opacity': 1
    });
    $(this).children().trigger('play');

});
$('.cover-banner').on('click', function () {
    $(this).children().css({
        'z-index': 0,
        'opacity': 1
    });
    $(this).children().trigger('play');

});
$('video').on('click', function () {
});

jQuery(document).ready(function ($) {
    "use strict";
    //  TESTIMONIALS CAROUSEL HOOK
    $('#customers-testimonials').owlCarousel({
        loop: true,
        center: true,
        items: 3,
        margin: 0,
        autoplay: true,
        dots: false,
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
var slider = tns({
    arrowKeys: true,
    autoplay: false,
    container: ".js-slider-product-tray",
    controls: true,
    controlsContainer: ".js-controls",
    gutter: 0,
    margin: 10,
    items: 1,
    loop: false,
    mouseDrag: true,
    nav: false,
    responsive: {
        360: {
            items: 1
        },
        600: {
            items: 1
        },
        768: {
            items: 1
        },
        800: {
            items: 2
        },
        915: {
            items: 2
        },
        1195: {
            items: 3
        },
    },
    slideBy: "page",
    touch: true
});