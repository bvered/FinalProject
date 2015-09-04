
var queryString;
var currentUniversity;

$(document).ready(function () {
    queryString = getQuertyString();
    $('#University').attr('value', queryString["University"]);
    currentUniversity = queryString["University"];

    ifNoUniversity(currentUniversity);
    getBackground(currentUniversity);
});

function addTeacher() {
    var uri = '/api/Teachers/AddTeacher';

    if (!$("#TeacherName").val() ||
        !$("#TeacherRoom").val() ||
        !$("#TeacherPhone").val() ||
        !$("#TeacherEmail").val()) {
        $("#EmptyRequierments")[0].hidden = false;
    } else {
        $(function () {
            var teacher = {
                University: currentUniversity,
                Name: $("#TeacherName").val(),
                Room: $('#TeacherRoom').val(),
                Cellphone: $('#TeacherPhone').val(),
                Email: $('#TeacherEmail').val()
            };

            $.ajax({
                type: "POST",
                data: JSON.stringify(teacher),
                url: uri,
                contentType: "application/json",


                statusCode: {
                    222: function (data) {
                        $("#TeacherExists")[0].hidden = false;
                        $("#Teacherink")[0].hidden = false;
                        $("#Teacherink")[0].href = "/AddTeacherComment/AddTeacherComment.html?University=" + currentUniversity + "&id=" + data;

                        $("#addTeacherSuccessfuly")[0].hidden = true;
                        $("#resultTeacher")[0].hidden = true;
                        $("#EmptyRequierments")[0].hidden = true;
                    },

                    200: function (data) {
                        $("#TeacherExists")[0].hidden = true;
                        $("#Teacherink")[0].hidden = true;
                        $("#EmptyRequierments")[0].hidden = true;

                        $("#addTeacherSuccessfuly")[0].hidden = false;
                        $("#resultTeacher")[0].hidden = false;
                        $("#resultTeacher")[0].href = "/AddTeacherComment/AddTeacherComment.html?University=" + currentUniversity + "&id=" + data;
                    }
                }
            });
        });
    }
}

function hideAllLabels() {
    $("#TeacherExists")[0].hidden = true;
}
