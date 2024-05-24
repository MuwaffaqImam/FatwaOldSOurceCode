(function ($) {
    'use strict';

    // Sticky menu
    var $window = $(window);
    $window.on('scroll', function () {
        var scroll = $window.scrollTop();
        if (scroll < 300) {
            $('.sticky').removeClass('is-sticky');
        } else {
            $('.sticky').addClass('is-sticky');
        }
    });

    // Off Canvas Open close
    $('.mobile-menu-btn').on('click', function () {
        $('body').addClass('fix');
        $('.off-canvas-wrapper').addClass('open');
    });

    $('.btn-close-off-canvas,.off-canvas-overlay').on('click', function () {
        $('body').removeClass('fix');
        $('.off-canvas-wrapper').removeClass('open');
    });

    // offcanvas mobile menu
    var $offCanvasNav = $('.mobile-menu');

    var $offCanvasNavSubMenu = $offCanvasNav.find('.dropdown');

    /* Add Toggle Button With Off Canvas Sub Menu */
    $offCanvasNavSubMenu
        .parent()
        .prepend('<span class="menu-expand"><i></i></span>');

    /* Close Off Canvas Sub Menu */
    $offCanvasNavSubMenu.slideUp();

    /* Category Sub Menu Toggle */
    $offCanvasNav.on('click', 'li a, li .menu-expand', function (e) {
        var $this = $(this);
        if (
            $this
                .parent()
                .attr('class')
                .match(
                    /\b(menu-item-has-children|has-children|has-sub-menu)\b/,
                ) &&
            ($this.attr('href') === '#' || $this.hasClass('menu-expand'))
        ) {
            e.preventDefault();
            if ($this.siblings('ul:visible').length) {
                $this.parent('li').removeClass('active');
                $this.siblings('ul').slideUp();
            } else {
                $this.parent('li').addClass('active');
                $this
                    .closest('li')
                    .siblings('li')
                    .removeClass('active')
                    .find('li')
                    .removeClass('active');
                $this.closest('li').siblings('li').find('ul:visible').slideUp();
                $this.siblings('ul').slideDown();
            }
        }
    });

    // hero slider active js
    $('.hero-slider-active').slick({
        speed: 1000,
        slidesToShow: 3,
        autoplay: false,
        arrows: false,
        dots: true,
    });

    // brand logo carousel active js
    $('.blog-post-carousel').slick({
        speed: 1000,
        slidesToShow: 4,
        autoplay: false,
        arrows: true,
        adaptiveHeight: true,
        prevArrow:
            '<button type="button" class="fa fa-arrow-alt-circle-right w-100"></button>',
        nextArrow:
            '<button type="button" class="fa fa-arrow-alt-circle-left w-100"></button>',
        appendArrows: '.slick-maqalat',
        rtl: true,
        responsive: [
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 2,
                },
            },
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 1,
                },
            },
        ],
    });

    // maktaba  carousel active js
    $('.maktaba-post-carousel').slick({
        speed: 1000,
        slidesToShow: 3,
        autoplay: false,
        arrows: true,
        adaptiveHeight: true,
        prevArrow:
            '<button type="button" class="fa-solid fa-arrow-alt-circle-right w-100"></button>',
        nextArrow:
            '<button type="button" class="fa-solid fa-arrow-alt-circle-left w-100"></button>',
        appendArrows: '.slick-maktaba',
        rtl: true,
        responsive: [
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 2,
                },
            },
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 1,
                },
            },
        ],
    });

    // estsharat  carousel active js
    $('.estsharat-post-carousel').slick({
        speed: 1000,
        slidesToShow: 2,
        autoplay: false,
        arrows: true,
        adaptiveHeight: true,
        prevArrow:
            '<button type="button" class="fa fa-arrow-alt-circle-right w-100"></button>',
        nextArrow:
            '<button type="button" class="fa fa-arrow-alt-circle-left w-100"></button>',
        appendArrows: '.slick-estsharat',
        rtl: true,
        responsive: [
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 2,
                },
            },
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 1,
                },
            },
        ],
    });

    // vedio  carousel active js
    $('.vedio-post-carousel').slick({
        speed: 1000,
        slidesToShow: 3,
        autoplay: false,
        arrows: true,
        adaptiveHeight: true,
        prevArrow:
            '<button type="button" class="fa fa-arrow-alt-circle-right w-100"></button>',
        nextArrow:
            '<button type="button" class="fa fa-arrow-alt-circle-left w-100"></button>',
        appendArrows: '.slick-vedio',
        rtl: true,
        responsive: [
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 2,
                },
            },
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 1,
                },
            },
        ],
    });

    // Scroll to top active js
    $(window).on('scroll', function () {
        if ($(this).scrollTop() > 600) {
            $('.scroll-top').removeClass('not-visible');
        } else {
            $('.scroll-top').addClass('not-visible');
        }
    });
    $('.scroll-top').on('click', function (event) {
        $('html,body').animate(
            {
                scrollTop: 0,
            },
            1000,
        );
    });

    var input = document.querySelector('#phone');
    window.intlTelInput(input, {
        // allowDropdown: false,
        // autoHideDialCode: false,
        // autoPlaceholder: "off",
        // dropdownContainer: document.body,
        // excludeCountries: ["us"],
        // formatOnDisplay: false,
        // geoIpLookup: function(callback) {
        //   $.get("http://ipinfo.io", function() {}, "jsonp").always(function(resp) {
        //     var countryCode = (resp && resp.country) ? resp.country : "";
        //     callback(countryCode);
        //   });
        // },
        // hiddenInput: "full_number",
        // initialCountry: "auto",
        // localizedCountries: { 'de': 'Deutschland' },
        // nationalMode: false,
        // onlyCountries: ['us', 'gb', 'ch', 'ca', 'do'],
        // placeholderNumberType: "MOBILE",
        // preferredCountries: ['cn', 'jp'],
        // separateDialCode: true,
        utilsScript: 'assets/js/utils.js',
    });

    $('#datepicker').datepicker({
        language: 'ar',
        orientation: 'auto left',
        autoclose: true,
        todayHighlight: true,
    });

    $('.input-group-addon').click(function () {
        $(document).ready(function () {
            $('#my-datepicker').datepicker().focus();
        });
    });
    $('.input-group-addon').datepicker();
    // wow js active
    new WOW().init();
})(jQuery);
