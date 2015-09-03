var queryString;
var currentUniversity;

$(document).ready(function () {
    queryString = getQuertyString();
    $('#University').attr('value', queryString["University"]);
    currentUniversity = queryString["University"];
    getBackground(currentUniversity);

    var courseId = queryString["courseId"];
    $("#returnToCourse")[0].href = "/AddCourseComment/AddCourseComment.html?University=" + currentUniversity + "&id=" + courseId;

    $('#btnUploadFile').on('click', function () {

        var semester = $('input[name=semester]:checked').val();
        var year = $('#Year').val();
        var isSyllabus = $('input[name=isSyllabus]:checked').val();

        if (!courseId || !semester || !year || !isSyllabus || isNaN(year)) {
            $("#EmptyRequierments")[0].hidden = false;
            $("#AddFileSuccesfully")[0].hidden = true;

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

           $.ajax({
                type: "POST",
                url: "/api/AddFile/AddFile",
                contentType: false,
                processData: false,
                data: data,
                statusCode: {
                    200: function () {
                        $("#EmptyRequierments")[0].hidden = true;
                        $("#AddFileSuccesfully")[0].hidden = false;
                    }
                }
            });
        }
    }
    );
});
