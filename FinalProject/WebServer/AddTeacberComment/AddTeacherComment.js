
var uri = '/api/Teachers/GetCriterias';
var uri2 = '/api/Teachers/AddComment';
var uri3 = '/api/Teachers/GetTeachers';

var teacherFound;

var allCriterias;
$.getJSON(uri).done(function(data) {
    allCriterias = data;
    console.log(allCriterias);
    populateCriterias();
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

function addComment() {
    var uri4 = '/api/Teachers/AddComment';

    $(function() {
        var ratings = [];
        for(criteria in allCriterias) {
            ratings.push(document.getElementById("criteriaRating"+criteria).value);
        }

        var comment = {
            Id: UserId,
            teacher: document.getElementById("TeacherId").value,
            Comment: document.getElementById("TeacherNewCommetBox").value,
            Ratings: ratings
        };

        var request = $.ajax({
            type: "POST",
            data: JSON.stringify(comment),
            url: uri4,
            contentType: "application/json"
        });
        request.done(function() {
            document.getElementById("newCommentForm").fadeOut("slow");
        });
        request.fail(function(jqXhr, textStatus) {
            alert("Request failed: " + textStatus);
        });
    });
}

function populateCriterias() {
    var divToAppend = document.getElementById("newCommentForm");
    var commentBox = document.createElement("textarea");
    commentBox.placeholder = "Write something..";
    commentBox.id = "TeacherNewCommetBox";
    divToAppend.appendChild(commentBox);
    divToAppend.appendChild(document.createElement("BR"));
    for (ratingText in allCriterias) {
        var criteriaText = allCriterias[ratingText];
        var labelInput = document.createElement("Label");
        labelInput.id = "criteriaText" + ratingText;
        labelInput.innerHTML = criteriaText;

        var inputText = document.createElement("INPUT");
        inputText.id = "criteriaRating" + ratingText;
        inputText.name = "criteriaRating" + ratingText;
        inputText.type = "number";
        inputText.min = "0";
        inputText.max = "5";

        divToAppend.appendChild(labelInput);
        divToAppend.appendChild(inputText);

        divToAppend.appendChild(document.createElement("BR"));
    }
    var sendButton = document.createElement("BUTTON");
    sendButton.id = "SendNewCommentButton";
    sendButton.onclick = addComment();
    divToAppend.appendChild(sendButton);
}

function showTeacherCommentsById(teacher) {
    var divToAppend = document.getElementById("allCommentsOfTeacher");

    var allComments = teacher.TeacherComments;
    for (comment in allComments) {
        printComment(divToAppend, allComments[comment], comment);
    }
}

function printComment(divToAppend, comment, itr) {
    var commentBox = document.createElement("textarea");
    commentBox.placeholder = comment.CommentText;
    commentBox.id = "TeacherCommet" + itr;
    divToAppend.appendChild(commentBox);
    for (rating in CriteriaRatings) {
        var ratingString = CriteriaRatings[rating].Criteria.DisplayName;
        var ratingNumber = CriteriaRatings[rating].Criteria.Rating;

        var labelForRatingString = document.createElement("Label");
        labelForRatingString.id = "CommentNumber" + itr + "RatingString" + rating;
        labelForRatingString.innerHTML = ratingString;

        var labelForRatingNumber = document.createElement("Label");
        labelForRatingNumber.id = "CommentNumber" + itr + "RatingNumber" + rating;
        labelForRatingNumber.innerHTML = ratingString;
    }
}