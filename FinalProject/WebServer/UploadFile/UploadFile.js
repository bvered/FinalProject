var queryString;
var currentUniversity;

$(document).ready(function () {
    queryString = getQuertyString();
    $('#University').attr('value', queryString["University"]);
    currentUniversity = queryString["University"];

    $('#btnUploadFile').on('click', function () {

        var courseId = queryString["courseId"];
        var semester = $('input[name=semester]:checked').val();
        var year = $('#Year').val();
        var isSyllabus = $('input[name=isSyllabus]:checked').val();

        if (!courseId || !semester || !year || !isSyllabus) {
            $("#EmptyRequierments")[0].hidden = false;
            $("#AddFileSuccesfully")[0].hidden = true;
            $("#returnToCourse")[0].hidden = true;

        } else {
            var files = $("#fileUpload").get(0).files;
            var data = new FormData();
            if (files.length == 1) {
                data.append("UploadedFile", files[0]);
            }
            data.append("courseId", courseId);
            data.append("semester", semester);
            data.append("year", year);
            data.append("isSyllabus", isSyllabus);

            var ajaxRequest = $.ajax({
                type: "POST",
                url: "/api/AddFile/AddFile",
                contentType: false,
                processData: false,
                data: data,
                statusCode: {
                    200: function (data, textStatus, jqXHR) {
                        $("#EmptyRequierments")[0].hidden = true;
                        $("#AddFileSuccesfully")[0].hidden = false;
                        $("#returnToCourse")[0].hidden = false;
                        $("#returnToCourse")[0].href = "/AddCourseComment/AddCourseComment.html?University=" + currentUniversity + "&id=" + courseId;
                    }
                }
            });
        }
    }
    );
});


function homePage() {
    window.location = "../HomePage/HomePage.html?University=" + currentUniversity;
}