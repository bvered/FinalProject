
var uri = '/api/Teachers/GetCriterias';
var uri2 = '/api/Teachers/AddComment';
var uri3 = '/api/Teachers/GetTeachers';

var allCriterias;
$.getJSON(uri).done(function(data) {
    allCriterias = data;

    console.log(allCriterias);
    $("#CriteriaName").autocomplete({
        source: allCourses
    });
});

var allTeachers;
$.getJSON(uri3).done(function(data) {
    allTeachers = data;
    console.log(allTeachers);
});

function checkAndAdd() {
    //if we can add the new comment
    inputSuccesss("#universityWrap", "#UniversityInput", "input2Status", "#universityError");
    hideAllLabels();
    addComment($("#TeacherId").val(), $("#CommentText").val(),
        [$("#Rating1").val(), $("#Rating2").val(), $("#Rating3").val(), $("#Rating4").val(), $("#Rating5").val()]);
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

function addComment(TeacherId, CommentText, Ratings) {
    var uri4 = '/api/Teachers/AddComment';

    $(function() {
        var comment = {
            Id: UserId,
            Comment: Comment,
            Ratings: Ratings
        };

        var request = $.ajax({
            type: "POST",
            data: JSON.stringify(comment),
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