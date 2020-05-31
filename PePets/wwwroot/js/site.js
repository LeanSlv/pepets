// URL настройки
var UrlSettings = {
    GetBreedsUrl: '@Url.Action("GetBreeds", "Advert", null, Request.Url.Scheme, null)'
}

// Slick слайдер для просмотра фотографий объявления
jQuery(function ($) {
    $(document).ready(function () {
        console.log("READY!");
   
        $('.advert-review-slider-for').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            fade: true,
            infinite: false,
            useTransform: true,
            asNavFor: '.advert-review-slider-nav'
        });
        $('.advert-review-slider-nav').slick({
            slidesToShow: 5,
            slidesToScroll: 5,
            asNavFor: '.advert-review-slider-for',
            dots: false,
            infinite: false,
            focusOnSelect: true
        });

        $('a[data-slide]').click(function (e) {
            e.preventDefault();
            var slideno = $(this).data('slide');
            $('.advert-review-slider-nav').slick('slickGoTo', slideno - 1);
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

// Динамическое заполнение списка пород в зависимости от выбранного вида
$('#PetDescription_Type').change(function () {
    // получаем выбранный вид
    var name = $(this).val();

    $.ajax({
        type: 'GET',
        url: '/Advert/LoadBreedsViewComponent?typeName=' + name,
        success: function (data) {
            // заменяем содержимое присланным частичным представлением
            $('#PetDescription_Breed').replaceWith(data);
        }
    });
});
