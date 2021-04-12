$('.global-material-item').on('click', function () {
    let message = {
        courseId: $('#course-id').val(),
        materialId: $(this).find('.material-item-id').val()
    };

    $.post('../ChooseFromGlobal/', message, function (data) {
        $('#display-material-area').html(data);
        $('#display-material-area').css('display', 'block')
        $('#create-material-area').css('display', 'none')
        $('#create-material-submit').css('display', 'none')
    });
});

$('.course-material-item').on('click', function () {
    let message = {
        courseId: $('#course-id').val(),
        materialId: $(this).find('.material-item-id').val()
    };

    $.post('../ChooseFromCourse/', message, function (data) {
        $('#display-material-area').html(data);
        $('#display-material-area').css('display', 'block')
        $('#create-material-area').css('display', 'none')
        $('#create-material-submit').css('display', 'none')
    });
});