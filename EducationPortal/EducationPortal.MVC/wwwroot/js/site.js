function OnTryCreateMaterial(id) {
    if ($('.validation>ul>li').is(':empty')) {
        window.location.href = "../Index/" + id;
    } 
}

let mainSettings = {
    defaultAnimationDelay: 1000
};

let courseDeleteButtonState = false

$('#course-delete-button').on('click', function () {
    if (courseDeleteButtonState) {
        $('#course-delete-confirm').css('display', 'none')
        courseDeleteButtonState = false;
    }
    else {
        $('#course-delete-confirm').css('display', 'block')
        courseDeleteButtonState = true;
    }
})

$('#course-delete-cancel').on('click', function () {
    $('#course-delete-confirm').css('display', 'none')
});

$('#create-material-button').on('click', function () {
    $('#create-material-area').css('display', 'block')
    $('#display-material-area').css('display', 'none')
});

$('#material-type-select').on('change', function () {
    let message = {
        materialType: $('#material-type-select').val()
    };

    $.post('../Editor/', message, function (data) {
        $('#create-material-form-content').html(data);
        $('#create-material-submit').css('display', 'block')
        $('#create-material-form').attr("data-ajax-url", "../Create" + message.materialType)
        $('#create-material-form').attr("data-ajax-method", "Post")
    });
});