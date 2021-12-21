$(function () {
    $('#cb1').click(function () {
        $('.modal').addClass('modal_active');
    });

    $('.modal__close-button').click(function () {
        $('.modal').removeClass('modal_active');
    });
});

$('.modal').mouseup(function (e) {
    let modalContent = $(".modal__content");
    if (!modalContent.is(e.target) && modalContent.has(e.target).length === 0) {
        $(this).removeClass('modal_active');
    }
});