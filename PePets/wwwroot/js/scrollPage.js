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
