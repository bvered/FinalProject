
var queryString;
var currentUniversity;
var allTeachers;
var allCourses;

$(document).ready(function () {
    queryString = getQuertyString();
    $('#University').attr('value', queryString["University"]);
    currentUniversity = queryString["University"];
    getBackground(currentUniversity);

    getCourses();
    getTeachers();
});

function getCourses() {
    var uri = '/api/Courses/GetCoursesNames/' + currentUniversity;

    $.getJSON(uri).done(function (data) {
        allCourses = data;

        console.log(allCourses);
        $("#CourseName").autocomplete({
            source: function (request, response) {
                var results = $.ui.autocomplete.filter(allCourses, request.term);
                response(results.slice(0, 10));
            }
        });
    });
}

function getTeachers() {
    var uri2 = '/api/Teachers/GetTeachersNames/' + currentUniversity;

    $.getJSON(uri2).done(function (data) {
        allTeachers = data;
        console.log(allTeachers);

        $("#teacherName").autocomplete({
            source: function (request, response) {
                var results = $.ui.autocomplete.filter(allTeachers, request.term);
                response(results.slice(0, 10));
            }
        });
    });
}

function checkAndAdd() {
    if ((allTeachers.indexOf($("#teacherName").val()) < 0)) { 
        $("#teacherError")[0].hidden = false;
        hideAllLabels();
    } else if (!($("#CourseName").val()) ||
        !($("#teacherName").val()) || 
        !($('input[name=faculty]:checked').val()) ||
        !($('input[name=mandatory]:checked').val()) || 
        !($('input[name=degree]:checked').val()) ||
        !($('input[name=year]:checked').val())) {

        $("#EmptyRequierments")[0].hidden = false;

        $("#CourseExists")[0].hidden = true;
        $("#CourseLink")[0].hidden = true;
        $("#teacherError")[0].hidden = true;
    } else { 
        $("#teacherError")[0].hidden = true;
        hideAllLabels();
        addCourse();
    }
};

function addCourse() {
    var uri4 = '/api/Courses/AddCourse';

    $(function () {
        var course = {
            Univesity: currentUniversity,
            Name: $("#CourseName").val(),
            TeacherName: $("#teacherName").val(),
            Faculty: $('input[name=faculty]:checked').val(),
            Ismandatory:  $('input[name=mandatory]:checked').val(),
            AcademicDegree:  $('input[name=degree]:checked').val(),
            IntendedYear: $('input[name=year]:checked').val(),
        };

        var request = $.ajax({
            type: "POST",
            data: JSON.stringify(course),
            url: uri4,
            contentType: "application/json",

            statusCode: {
                222: function (data, textStatus, jqXHR) {
                    $("#CourseExists")[0].hidden = false;
                    $("#CourseLink")[0].hidden = false;
                    $("#CourseLink")[0].href = "/AddCourseComment/AddCourseComment.html?University=" + currentUniversity + "&id=" + data;
                },
                200: function(data, textStatus, jqXHR) {
                    $("#addCourseSuccessfuly")[0].hidden = false;
                    $("#resultCourse")[0].hidden = false;
                    $("#resultCourse")[0].href = "/AddCourseComment/AddCourseComment.html?University=" + currentUniversity + "&id=" + data;
                }
            }
        });

        request.fail(function (jqXhr, textStatus) {
            alert("Request failed: " + textStatus);
        });
    });
}

function hideAllLabels() {
    $("#CourseExists")[0].hidden = true;
    $("#CourseLink")[0].hidden = true;
    $("#EmptyRequierments")[0].hidden = true;
}
