jQuery(function ($) {
    $(document).ready(function () {
        console.log("READY!");

        // Slick слайдер для просмотра фотографий объявления        
        $('.advert-review-slider-for').slick({
            slidesToShow: 1,
            arrows: false,
            fade: true,
            cssEase: 'linear',
            asNavFor: '.advert-review-slider-nav'
        });

        //TODO: Исправить баг с прокручиванием нижнего слайдера при смене фотографии путем свайпа основной фотографии 
        $('.advert-review-slider-nav').slick({
            slidesToShow: 10,
            slidesToScroll: 1,
            asNavFor: '.advert-review-slider-for',
            centerMode: true,
            centerPadding: '5px',
            focusOnSelect: true
        });
    });
});

//Плавная прокрутка страницы к тэгу с указанным id
function SlowScroll(id) {
    var offset = $("nav.navbar").height() + 10;
    $("html, body").animate({
        scrollTop: $(id).offset().top - offset
    }, {
        duration: 250,
        easing: "swing"
    });
    return false;
}

// Появление формы регистрации в модальном окне
function ShowRegisterForm() {
    $('#LoginForm').hide();
    $('#RegisterForm').show();
}

$('#ModalAuth').on('hidden.bs.modal', function (e) {
    $('#RegisterForm').hide();
    $('#LoginForm').show();
})

// Анимация кнопок переключение списков объявлений в профиле пользователя
function LoadMyAdverts() {
    $('#myAdvertsLink').addClass("adverts-link-active").removeClass('border-bottom-animation');
    $('#likesAdvertsLink').removeClass('adverts-link-active').addClass('border-bottom-animation');

    $('#myAdverts').show();
    $('#likesAdverts').hide();
}

function LoadLikesAdverts() {
    $('#likesAdvertsLink').addClass("adverts-link-active").removeClass("border-bottom-animation");
    $('#myAdvertsLink').removeClass('adverts-link-active').addClass('border-bottom-animation');

    $('#likesAdverts').show();
    $('#myAdverts').hide();
}