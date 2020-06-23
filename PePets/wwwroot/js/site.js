jQuery(function ($) {
    $(document).ready(function () {
        // Slick ������� ��� ��������� ���������� ����������
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

        // ������� ���� ������ ����
        $('#input_date').daterangepicker({
            singleDatePicker: true,
            showDropdowns: true,
            minYear: 1901,
            locale: {
                format: 'DD.MM.YYYY'
            }
        });
    });
});

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
    $('#loginModalForm').hide();
    $('#registerModalForm').show();
}

$('#ModalAuth').on('hidden.bs.modal', function (e) {
    $('#registerModalForm').hide();
    $('#loginModalForm').show();
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

// ������������ ���������� ������ ����� � ����������� �� ���������� ����
$('#PetDescription_Type').change(function () {
    // �������� ��������� ���
    var name = $(this).val();

    $.ajax({
        type: 'GET',
        url: '/Advert/LoadBreedsViewComponent?typeName=' + name,
        success: function (data) {
            // �������� ���������� ���������� ��������� ��������������
            $('#PetDescription_Breed').replaceWith(data);
        }
    });
});

// ������������ �����
$('#search_form').submit(function (event) {
    event.preventDefault();
    var input_value = $('.search_input').val();

    $.ajax({
        type: 'GET',
        url: '/Search/Search?input=' + input_value,
        success: function (data) {
            // �������� ���������� ���������� ��������� ��������������
            $('#posts').replaceWith(data);
        }
    });
});

// ���������� ���������� � ���������
$('.btn-like').click(function (event) {
    event.preventDefault();
    var that = $(this);
    var href = that[0].href;

    $.ajax({
        type: 'GET',
        url: href,
        success: function (data) {
            if (data) {
                // ������ �� ����������� ������
                that.children(".fa-heart").removeClass("far");
                that.children(".fa-heart").addClass("fas");
            }
            else {
                // ������ �� ������������� ������
                that.children(".fa-heart").removeClass("fas");
                that.children(".fa-heart").addClass("far");
            }
        }
    });
})

// ������ �������� �� �������� �������������� �������
$('#upload-avatar').change(function () {
    let avatar = this.files[0];
    let image_url = window.URL.createObjectURL(avatar);
    let img = '<img src=' + image_url + ' height="200" width="200" id="change_avatar" />'
    $('#change_avatar').replaceWith(img);
})

// switch ��� ������ �������� �� �������� �������� � �������������� ����������
$('#phoneSwitch').change(function (event) {
    if ($(this).is(':checked')) {
        $('#inputPhone').attr('disabled', true);
        $('#countries_phone').attr('disabled', true);
    }
    else {
        $('#inputPhone').removeAttr('disabled', false);
        $('#countries_phone').removeAttr('disabled', false);
    }
});

// ���� �� ����
$('#loginForm').submit(function (event) {
    event.preventDefault();
    var loginForm = $(this);
    $.ajax({
        type: 'POST',
        url: '/Account/Login/',
        data: loginForm.serialize(),
        success: function (data) {
            if (data.startsWith('<!DOCTYPE html>')) {
                window.location.href = '/';
            }
            else {
                $('#loginModalForm').replaceWith(data);
            }
        }
    });
});

// ����������� �� �����
$('#registerForm').submit(function (event) {
    event.preventDefault();
    var registerForm = $(this);

    $.ajax({
        type: 'POST',
        url: '/Account/Register/',
        data: registerForm.serialize(),
        success: function (data) {
            if (data.startsWith('<!DOCTYPE html>')) {
                window.location.href = '/';
            }
            else {
                $('#registerModalForm').replaceWith(data);
                $('#registerModalForm').show();
            }
        }
    });
});
