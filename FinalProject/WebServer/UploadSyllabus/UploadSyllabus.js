
$(document).ready(function () {

    $('#btnUploadFile').on('click', function () {

        var files = $("#fileUpload").get(0).files;

        var data = new FormData();

        // Add the uploaded image content to the form data collection
        if (files.length == 1) {
            data.append("UploadedFile", files[0]);
        }

        var quertyString = getQuertyString();

        var courseId = quertyString["courseId"];
        var semster = quertyString["semester"];
        var year = quertyString["year"];

        data.append("courseId", courseId);
        data.append("semester", semster);
        data.append("year", year);

        // Make Ajax request with the contentType = false, and procesDate = false
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