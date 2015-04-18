
var uri = '/api/Courses/GetCourses';
var uri2 = '/api/Universities/GetUniversities';
var uri3 = '/api/Teachers/GetTeachers';

var allCourses;
$.getJSON(uri).done(function(data) {
    allCourses = data;

    console.log(allCourses);
    $("#CourseName").autocomplete({
        source: allCourses
    });
});

var allUniversites;
$.getJSON(uri2).done(function(data) {
    allUniversites = data;

    console.log(allUniversites);
    $("#UniversityName").autocomplete({
        source: allUniversites
    });
});

var allTeachers;
$.getJSON(uri3).done(function(data) {
    allTeachers = data;
    console.log(allTeachers);
});

function checkAndAdd() {
    if ((allUniversites.indexOf($("#UniversityName").val()) < 0)) { //if the university doesn't exists
        inputError("#universityWrap", "#UniversityInput", "input2Status", "#universityError");
        hideAllLabels();
    } else if (allTeachers.indexOf($("#TeacherName").val()) >= 0) { //if the teacher already exists
        inputSuccesss("#universityWrap", "#UniversityInput", "input2Status", "#universityError");
        $("#TeacherExists")[0].hidden = false;
        $("#TeachersLink")[0].hidden = false;
        $("#TeachersLink")[0].href = "#"+ $("#TeacherName").val();
    } else { //if we can add the new teacher
        inputSuccesss("#universityWrap", "#UniversityInput", "input2Status", "#universityError");
        hideAllLabels();
        addTeacher($("#TeacherName").val(), $("#UniversityName").val());
    }
};

function checkUniveristy() {
    if (allUniversites.indexOf($("#UniversityName").val()) >= 0)
        inputSuccesss("#universityWrap", "#UniversityInput", "input2Status", "#universityError");
    else
        inputError("#universityWrap", "#UniversityInput", "input2Status", "#universityError");
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

function addTeacher(teacherName, universityName) {
    var uri4 = '/api/Teachers/AddTeacher';

    $(function() {
        var teacher = {
            Name: teacherName,
            UniversityName: universityName
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
    $("#TeachersLink")[0].hidden = true;
}