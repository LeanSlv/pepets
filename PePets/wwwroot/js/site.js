// URL ���������
var UrlSettings = {
    GetBreedsUrl: '@Url.Action("GetBreeds", "Advert", null, Request.Url.Scheme, null)'
}

//������� ��������� �������� � ���� � ��������� id
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

// ��������� ����� ����������� � ��������� ����
function ShowRegisterForm() {
    $('#LoginForm').hide();
    $('#RegisterForm').show();
}

$('#ModalAuth').on('hidden.bs.modal', function (e) {
    $('#RegisterForm').hide();
    $('#LoginForm').show();
})

// �������� ������ ������������ ������� ���������� � ������� ������������
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

$(document).ready(function () {
    console.log("READY!");

    // Slick ������� ��� ��������� ���������� ����������        
    $('.advert-review-slider-for').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false,
        fade: true,
        cssEase: 'linear',
        asNavFor: '.advert-review-slider-nav'
    });

    $('.advert-review-slider-nav').slick({
        slidesToShow: 5,
        slidesToScroll: 1,
        asNavFor: '.advert-review-slider-for',
        centerMode: true,
        centerPadding: '1px',
        focusOnSelect: true
    });
});

// ������������ ���������� ������ ����� � ����������� �� ���������� ����
$('#types').change(function () {
    // �������� ��������� ���
    var type = $(this).val();
    console.log("change " + type);

    $.ajax({
        type: 'GET',
        url: '/Advert/GetBreeds?type=' + type,
        success: function (data) {
            console.log("success");
            console.log(data);
            // �������� ���������� ���������� ��������� ��������������
            //$('#breeds').replaceWith(data);
        }
    });
});