
var uri = '/api/Courses/GetCourses';
var uri3 = '/api/Teachers/GetTeachers';

var allCourses;
$.getJSON(uri).done(function(data) {
    allCourses = data;

    console.log(allCourses);
    $("#CourseName").autocomplete({
        source: allCourses
    });
});


var allTeachers;
$.getJSON(uri3).done(function(data) {
    allTeachers = data;
    console.log(allTeachers);
});

function checkAndAdd() {
    if (allTeachers.indexOf($("#TeacherName").val()) >= 0) { //if the teacher already exists
        $("#TeacherExists")[0].hidden = false;
    } else { //if we can add the new teacher
        hideAllLabels();
        addTeacher($("#TeacherName").val());
    }
};


function inputSuccesss(divId, inputId, inputStatusId, labelId) {
    $(divId).removeClass("has-error").addClass("has-success");
    $(inputId).removeClass("glyphicon-remove").addClass("glyphicon-ok");
    $(inputStatusId).input = "(success)";
    $(labelId)[0].hidden = true;
};

function inputError(divId, inputId, inputStatusId, labelId) {
    $(divId).removeClass("has-success").addClass("has-error");
    $(inputId).removeClass("glyphicon-ok").addClass("glyphicon-remove");
    $(inputStatusId).input = "(error)";
    $(labelId)[0].hidden = false;
};

function addTeacher(teacherName, teacherRoom, teacherPhone, teacherEmail) {
    var uri4 = '/api/Teachers/AddTeacher';

    $(function() {
        var teacher = {
            Name: teacherName,
            Room: teacherRoom,
            Cellphone: teacherPhone,
            Email: teacherEmail,
        };

        var request = $.ajax({
            type: "POST",
            data: JSON.stringify(teacher),
            url: uri4,
            contentType: "application/json"
        });

        request.done(function() {
            $("#addTeacherSuccessfuly")[0].hidden = false;
            $("#HomePage")[0].hidden = false;
        });

        request.fail(function(jqXhr, textStatus) {
            alert("Request failed: " + textStatus);
        });
    });
}

function hideAllLabels() {
    $("#TeacherExists")[0].hidden = true;
}