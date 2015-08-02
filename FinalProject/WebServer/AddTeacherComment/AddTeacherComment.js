var uri = '/api/Teachers/GetCriterias';
var uri2 = '/api/Teachers/AddComment';
var uri4 = '/api/Teachers/GetTeacher';
var uri5 = '/api/Teachers/AddVote';
var uri6 = '/api/AddFile/AddSyllabus';

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
        data: JSON.stringify(vote),
        url: uri5,
        contentType: "application/json",
        success: function (data) {
            voteValueLabel.innerHTML = "אהבו: " + data;

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
    showTeacherCommentsByTeacher(false);
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

function showTeacherCommentsByTeacher(sortByNew) {
    removingAllContentOfDiv(teacherCommentsDiv);
    var commentsLabel = document.createElement("Label");
    commentsLabel.id = "commentsLabel";
    commentsLabel.innerHTML = "Comments";
    teacherCommentsDiv.appendChild(commentsLabel);
    teacherCommentsDiv.appendChild(document.createElement('br'));

    var allComments = teacher.TeacherComments;
    if(!sortByNew) {
        allComments = allComments.sort(function (a, b) {
            return b.TotalNumberOfLikes - a.TotalNumberOfLikes
        });
    }
    numberOfCommentsLoaded = allComments.length;
    for (comment = 0; comment < numberOfCommentsLoaded; comment++) {
        printComment(allComments[comment], comment);
    }
}

function printComment(comment, itr) {
    var commentViewDiv = document.createElement("div");
    commentViewDiv.id = "commentViewDiv" + itr;
    teacherCommentsDiv.appendChild(commentViewDiv);
    var commentBox = document.createElement("Label");
    commentBox.innerHTML = comment.CommentText;
    commentBox.id = "TeacherCommet" + itr;
    commentViewDiv.appendChild(commentBox);

    var numberOfVotes = document.createElement("Label");
    numberOfVotes.id = "CommentNumber" + itr + "Votes";
    numberOfVotes.innerHTML = " אהבו: " + comment.TotalNumberOfLikes;
    var voteUpButton = document.createElement("Button");
    voteUpButton.id = "CommentNumber" + itr + "VoteUp";
    voteUpButton.value = comment.Id;
    voteUpButton.onclick = function () { addVote(numberOfVotes, comment.Id, true); };
    voteUpButton.innerHTML = "Like";
    var voteDownButton = document.createElement("Button");
    voteDownButton.id = "CommentNumber" + itr + "VoteDown";
    voteDownButton.value = comment.Id;
    voteDownButton.onclick = function () { addVote(numberOfVotes, comment.Id, false); };
    voteDownButton.innerHTML = "Dislike";
    commentViewDiv.appendChild(voteDownButton);
    commentViewDiv.appendChild(numberOfVotes);
    commentViewDiv.appendChild(voteUpButton);
    commentViewDiv.appendChild(document.createElement('br'));

    for (rating in comment.CriteriaRatings) {
        var loadedComment = comment.CriteriaRatings[rating];

            var labelForRatingString = document.createElement("Label");
            labelForRatingString.id = "CommentNumber" + itr + "RatingString" + rating;
            labelForRatingString.innerHTML = loadedComment.Criteria.DisplayName + " ";

            var labelForRatingNumber = document.createElement("Label");
            labelForRatingNumber.id = "CommentNumber" + itr + "RatingNumber" + rating;
            labelForRatingNumber.innerHTML = loadedComment.Rating;

            commentViewDiv.appendChild(labelForRatingString);
            commentViewDiv.appendChild(labelForRatingNumber);
            commentViewDiv.appendChild(document.createElement('br'));
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

$(':button').click(function () {
    var formData = new FormData($('form')[0]);
    $.ajax({
        url: uri6,
        type: 'POST',
        xhr: function () {  // Custom XMLHttpRequest
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) { // Check if upload property exists
                myXhr.upload.addEventListener('progress', progressHandlingFunction, false); // For handling the progress of the upload
            }
            return myXhr;
        },
        //Ajax events
        beforeSend: beforeSendHandler,
        success: completeFileUploadHandler,
        error: failedFileUploadHandler,
        // Form data
        data: formData,
        //Options to tell jQuery not to process data or worry about content-type.
        cache: false,
        contentType: false,
        processData: false
    });
});

function progressHandlingFunction(e) {
    if (e.lengthComputable) {
        $('progress').attr({ value: e.loaded, max: e.total });
    }
}

function completeFileUploadHandler() {

}

function failedFileUploadHandler() {
    alert("תגובתך הוספה בהצלחה");
}

function removingAllContentOfDiv(div) {
    while (div.hasChildNodes()) {
        div.removeChild(div.lastChild);
    }
}