
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
        var isSyllabus = $('input[name=isSyllabus]:checked').val();

        data.append("courseId", courseId);
        data.append("semester", semester);
        data.append("year", year);
        data.append("isSyllabus", isSyllabus);

        var ajaxRequest = $.ajax({
            type: "POST",
            url: "/api/AddFile/AddFile",
            contentType: false,
            processData: false,
            data: data
        });

        ajaxRequest.done(function (xhr, textStatus) {
            $("#AddFileSuccesfully")[0].hidden = false;
        });
    });
});

function homePage() {
    window.location = "../HomePage/HomePage.html";
}