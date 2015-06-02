
var uri = '/api/Courses/GetCourses';
var uri2 = '/api/Universities/GetUniversities';
var uri3 = '/api/Teachers/GetTeachers';

var allCourses;
$.getJSON(uri).done(function (data) {
    allCourses = data;

    console.log(allCourses);
    $("#CourseName").autocomplete({
        source: allCourses
    });
});

var allUniversites;
$.getJSON(uri2).done(function (data) {
    allUniversites = data;

    console.log(allUniversites);
    $("#UniversityName").autocomplete({
        source: allUniversites
    });
});

var allTeachers;
$.getJSON(uri3).done(function (data) {
    allTeachers = data;
    console.log(allTeachers);

    $("#teacherName").autocomplete({
        source: allTeachers
    });
});

function checkAndAdd() {
    if ((allUniversites.indexOf($("#UniversityName").val()) < 0)) { //if the university doesn't exists
        inputError("#universityWrap", "#UniversityInput", "input2Status", "#universityError");
        hideAllLabels();
    } else if ((allTeachers.indexOf($("#teacherName").val()) < 0)) { //if the teacher doesn't exists
         inputError("#teacherWrap", "#teacherInput", "input2Status", "#teacherError");
         hideAllLabels();
    } else if (allCourses.indexOf($("#CourseName").val()) >= 0) { //if the course already exists
        inputSuccesss("#universityWrap", "#UniversityInput", "input2Status", "#universityError");
        $("#CourseExists")[0].hidden = false;
        $("#CourseLink")[0].hidden = false;
        $("#CourseLink")[0].href = "#" + $("#CourseName").val();
    } else { //if we can add the new course
        inputSuccesss("#universityWrap", "#UniversityInput", "input2Status", "#universityError");
        inputSuccesss("#teacherWrap", "#teacherInput", "input2Status", "#teacherError");
        hideAllLabels();
        addCourse($("#CourseName").val(), $("#UniversityName").val(), $("#teacherName").val());
    }
};

function checkUniveristy() {
    if (allUniversites.indexOf($("#UniversityName").val()) >= 0)
        inputSuccesss("#universityWrap", "#UniversityInput", "input2Status", "#universityError");
    else
        inputError("#universityWrap", "#UniversityInput", "input2Status", "#universityError");
};

function checkTeacher() {
    if (allTeachers.indexOf($("#teacherName").val()) >= 0)
        inputSuccesss("#teacherWrap", "#teacherInput", "input2Status", "#teacherError");
    else
        inputError("#teacherWrap", "#teacherInput", "input2Status", "#teacherError");
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

function addCourse(CourseName, universityName, teacherName) {
    var uri4 = '/api/Courses/AddCourse';

    $(function () {
        var course = {
            Name: CourseName,
            UniversityName: universityName,
            TeacherName: teacherName
    };

        var request = $.ajax({
            type: "POST",
            data: JSON.stringify(course),
            url: uri4,
            contentType: "application/json"
        });

        request.done(function () {
            $("#addCourseSuccessfuly")[0].hidden = false;
            $("#HomePage")[0].hidden = false;
        });

        request.fail(function (jqXhr, textStatus) {
            alert("Request failed: " + textStatus);
        });
    });
}

function hideAllLabels() {
    $("#CourseExists")[0].hidden = true;
    $("#CourseLink")[0].hidden = true;
}