var uri = '/api/Teachers/GetCriterias';
var uri2 = '/api/Teachers/AddComment';
var uri4 = '/api/Teachers/GetTeacher';
var uri5 = '/api/Comment/Vote';

var teacher;
var allCriterias;
var allTeachers;
var teacherInfoList;
var numberOfCommentsLoaded;

var TeacherInfoDiv = document.getElementById("TeacherInfoDiv");
var newCommentDiv = document.getElementById("NewCommentDiv");
var teacherCommentsDiv = document.getElementById("CommentsDiv");

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function loadView() {
    if (loadCommentsCriteras() == false) {
        showLoadingTeacherFailed();
        return;
    }
    didSucceedLoadingTeacher = loadTeacher();
    if (didSucceedLoadingTeacher == true) {
        printInformationOfTeacher();
    } else {
        showLoadingTeacherFailed();
    }
}
function loadCommentsCriteras() {
    succeed = false;

    var request = $.ajax({
        type: "GET",
        url: uri,
        contentType: "application/json",
        success: function (data) {
                allCriterias = data;
                succeed = true;
        },
        fail: function (data) {
            succeed = false;
        },
        async: false
    });
    return succeed;
}

function loadTeacher() {
    id = getParameterByName('id');
    succeed = false;

    var request = $.ajax({
        type: "GET",
        url: uri4 + "/" + id,
        contentType: "application/json",
        success: function (data) {
            teacher = data;
            succeed = true;
        },
        fail: function (data) {
            succeed = false;
        },
        async: false
    });
    return succeed;
}

function addVote(voteValueLabel ,id, like) {
    succeed = false;
    var vote = {
        commentId: id,
        liked: like,
    };
    var request = $.ajax({
        type: "POST",
        data: JSON.stringify(comment),
        url: uri5,
        contentType: "application/json",
        success: function (data) {
            voted = 1;
            if (like == false) {
                voted = -1;
            }
            voteValueLabel.innerHTML = parseInt(voteValueLabel.innerHTML) + voted;

        },
        fail: function (jqXhr, textStatus) {
            alert("נכשל: " + textStatus);
        },
        async: false
    });
    return succeed;
}

function printInformationOfTeacher() {
    showTeacherInfoToUser();
    showCommentOptions();
    showTeacherCommentsByTeacher();
}

function showTeacherInfoToUser() {
    var teacherNameLabel = document.createElement("Label");
    teacherNameLabel.id = "propertyLabelTeacherName";
    teacherNameLabel.innerHTML = "שם: " + teacher.Name;
    TeacherInfoDiv.appendChild(teacherNameLabel);
    TeacherInfoDiv.appendChild(document.createElement('br'));

    var teacherScoreLabel = document.createElement("Label");
    teacherScoreLabel.id = "propertyLabelTeacherScore";
    teacherScoreLabel.innerHTML = "ציון ממוצע: " + teacher.Score;
    TeacherInfoDiv.appendChild(teacherScoreLabel);
    TeacherInfoDiv.appendChild(document.createElement('br'));

    var teacherRoomLabel = document.createElement("Label");
    teacherRoomLabel.id = "propertyLabelTeacherRoom";
    teacherRoomLabel.innerHTML = "חדר: " + teacher.Room;
    TeacherInfoDiv.appendChild(teacherRoomLabel);
    TeacherInfoDiv.appendChild(document.createElement('br'));

    var teacherCellphoneLabel = document.createElement("Label");
    teacherCellphoneLabel.id = "propertyLabelTeacherCellphone";
    teacherCellphoneLabel.innerHTML = "טלפון: " + teacher.Cellphone;
    TeacherInfoDiv.appendChild(teacherCellphoneLabel);
    TeacherInfoDiv.appendChild(document.createElement('br'));

    var teacherEmailLabel = document.createElement("Label");
    teacherEmailLabel.id = "propertyLabelTeacherEmail";
    teacherEmailLabel.innerHTML = "מייל: " + teacher.Email;
    TeacherInfoDiv.appendChild(teacherEmailLabel);
    TeacherInfoDiv.appendChild(document.createElement('br'));
}

function showTeacherCommentsByTeacher() {
    var commentsLabel = document.createElement("Label");
    commentsLabel.id = "commentsLabel";
    commentsLabel.innerHTML = "Comments";
    teacherCommentsDiv.appendChild(commentsLabel);
    teacherCommentsDiv.appendChild(document.createElement('br'));

    var allComments = teacher.TeacherComments;
    numberOfCommentsLoaded = allComments.length;
    for (comment = 0; comment < numberOfCommentsLoaded; comment++) {
        printComment(allComments[comment], comment);
    }
}

function printComment(comment, itr) {
    var commentBox = document.createElement("textarea");
    commentBox.placeholder = comment.CommentText;
    commentBox.id = "TeacherCommet" + itr;
    teacherCommentsDiv.appendChild(commentBox);

    var numberOfVotes = document.createElement("Label");
    numberOfVotes.id = "CommentNumber" + itr + "Votes";
    numberOfVotes.innerHTML = " אהבו: " + comment.TotalNumberOfLikes;
    var voteUpButton = document.createElement("Button");
    voteUpButton.id = "CommentNumber" + itr + "VoteUp";
    voteUpButton.value = comment.id;
    voteUpButton.onclick = function () { addVote(numberOfVotes, comment.id, true); };
    voteUpButton.innerHTML = "Like";
    var voteDownButton = document.createElement("Button");
    voteDownButton.id = "CommentNumber" + itr + "VoteDown";
    voteDownButton.value = comment.id;
    voteDownButton.onclick = function () { addVote(numberOfVotes, comment.id, false); };
    voteDownButton.innerHTML = "Dislike";
    teacherCommentsDiv.appendChild(voteDownButton);
    teacherCommentsDiv.appendChild(numberOfVotes);
    teacherCommentsDiv.appendChild(voteUpButton);
    teacherCommentsDiv.appendChild(document.createElement('br'));

    for (rating in comment.CriteriaRatings) {
        var loadedComment = comment.CriteriaRatings[rating];

            var labelForRatingString = document.createElement("Label");
            labelForRatingString.id = "CommentNumber" + itr + "RatingString" + rating;
            labelForRatingString.innerHTML = loadedComment.Criteria.DisplayName + " ";

            var labelForRatingNumber = document.createElement("Label");
            labelForRatingNumber.id = "CommentNumber" + itr + "RatingNumber" + rating;
            labelForRatingNumber.innerHTML = loadedComment.Rating;

            teacherCommentsDiv.appendChild(labelForRatingString);
            teacherCommentsDiv.appendChild(labelForRatingNumber);
            teacherCommentsDiv.appendChild(document.createElement('br'));
    }
}

function showCommentOptions() {
    removingAllContentOfDiv(newCommentDiv);
    var addNewCommentButton = document.createElement("Button");
    addNewCommentButton.innerHTML = "הוסף תגובה חדשה";
    addNewCommentButton.onclick = revealAddCommentViewToUser;
    newCommentDiv.appendChild(addNewCommentButton);
    newCommentDiv.appendChild(document.createElement("BR"));
}

function revealAddCommentViewToUser() {
    removingAllContentOfDiv(newCommentDiv);
    var commentBox = document.createElement("textarea");
    commentBox.placeholder = "דעתך כאן..";
    commentBox.id = "TeacherNewCommetBox";
    newCommentDiv.appendChild(commentBox);
    newCommentDiv.appendChild(document.createElement('br'));
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

        newCommentDiv.appendChild(labelInput);
        newCommentDiv.appendChild(inputText);

        newCommentDiv.appendChild(document.createElement("BR"));
    }
    var sendButton = document.createElement("BUTTON");
    sendButton.innerHTML = "שלח";
    sendButton.onclick = addComment;
    newCommentDiv.appendChild(sendButton);
    newCommentDiv.appendChild(document.createElement("BR"));
}

function showLoadingTeacherFailed() {
    var failedSearchLabel = document.createElement("Label");
    failedSearchLabel.id = "failedSearchLabel";
    failedSearchLabel.innerHTML = "מרצה לא נמצא, הנך מועבר לעמוד הראשי.";
    TeacherInfoDiv.appendChild(failedSearchLabel);
}

function addComment() {
    var ratings = [];
    for (criteria in allCriterias) {
        ratings.push(document.getElementById("criteriaRating" + criteria).value);
    }
    var comment = {
        Id: teacher.Id,
        Ratings: ratings,
        Comment: document.getElementById("TeacherNewCommetBox").value,
    };
    var request = $.ajax({
        type: "POST",
        data: JSON.stringify(comment),
        url: uri2,
        contentType: "application/json",
        success: function (data) {
            alert("תגובתך הוספה בהצלחה");
            location.reload();
        },
        fail: function (jqXhr, textStatus) {
            alert("נכשל: " + textStatus);
        },
        async: false
    });
}

function removingAllContentOfDiv(div) {
    while (div.hasChildNodes()) {
        div.removeChild(div.lastChild);
    }
}