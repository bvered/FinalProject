
$(document).ready(function () {

    $('#btnUploadFile').on('click', function () {

        var files = $("#fileUpload").get(0).files;

        var data = new FormData();

        if (files.length == 1) {
            data.append("UploadedFile", files[0]);
        }

        var quertyString = getQuertyString();

        var courseId = quertyString["courseId"];
        var semester = $('input[name=semester]:checked').val();
        var year = $('#Year').val();

        data.append("courseId", courseId);
        data.append("semester", semester);
        data.append("year", year);

        var ajaxRequest = $.ajax({
            type: "POST",
            url: "/api/AddFile/AddSyllabus",
            contentType: false,
            processData: false,
            data: data
        });

        ajaxRequest.done(function (xhr, textStatus) {
            $("#AddSyllabysSuccesfully")[0].hidden = false;
        });
    });
});

function homePage() {
    window.location = "../HomePage/HomePage.html";
}