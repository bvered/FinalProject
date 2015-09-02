var teacher;
var allCriterias;
var numberOfCommentsLoaded;
var queryString;
var currentUniversity;
var filteredComments;

$(document).ready(function () {
    setupUniversity();
    setupPage();
});

function setupUniversity() {
    queryString = getQuertyString();
    $('#University').attr('value', queryString["University"]);
    currentUniversity = queryString["University"];
    getBackground(currentUniversity);
}

function setupPage() {
    if (loadCommentsCriteras() == false) {
        showLoadingTeacherFailed();
        return;
    }
    var didSucceedLoadingTeacher = loadTeacher();
    if (didSucceedLoadingTeacher == true) {
        setupNewComment();
        printInformationOfTeacher();
    } else {
        showLoadingTeacherFailed();
    }
}

function loadCommentsCriteras() {
    var succeed = false;

    $.ajax({
        type: "GET",
        url: '/api/Teachers/GetCriterias',
        contentType: "application/json",
        success: function (data) {
                allCriterias = data;
                succeed = true;
        },
        fail: function () {
            succeed = false;
        },
        async: false
    });
    return succeed;
}

function loadTeacher() {
    var id = getQuertyString()["id"];
    var succeed = false;

    $.ajax({
        type: "GET",
        url: '/api/Teachers/GetTeacher' + "/" + id,
        contentType: "application/json",
        success: function (data) {
            teacher = data;
            succeed = true;
        },
        fail: function () {
            succeed = false;
        },
        async: false
    });
    return succeed;
}

function addVote(voteValueLabel ,id, like) {
    var didLikeBefore = $.jStorage.get(id);
    if (didLikeBefore == true) {
        alert("מותר לדרג פעם אחת");
        return false;
    }
    var vote = {
        commentId: id,
        liked: like
    };
    $.ajax({
        type: "POST",
        data: JSON.stringify(vote),
        url: '/api/Teachers/AddVote',
        contentType: "application/json",
        success: function (data) {
            voteValueLabel.innerHTML = "אהבו: " + data;
            $.jStorage.set(id, true);
        },
        fail: function (jqXhr, textStatus) {
            alert("נכשל: " + textStatus);
        },
        async: false
    });
    return false;
}

function setupNewComment() {
    $('#newAvgRating0').rating('refresh', { showClear: false, showCaption: false });
    $('#newAvgRating1').rating('refresh', { showClear: false, showCaption: false });
    $('#newAvgRating2').rating('refresh', { showClear: false, showCaption: false });
    $('#newAvgRating3').rating('refresh', { showClear: false, showCaption: false });
}

function printInformationOfTeacher() {
    showTeacherInfoToUser();
    printTeacherScores();
    setTeacherCommentsWithFilters();
}

function showTeacherInfoToUser() {
    var teacherNameLabel = document.getElementById("teacherNameTD");
    teacherNameLabel.innerHTML = teacher.Name;

    var teacherScoreLabel = document.getElementById("teacherAvgTD");
    var averageRatings = parseInt(teacher.Score);
    $('#avgRating').rating('update', averageRatings);
    $('#avgRating').rating('refresh', { readonly: true, showClear: false, showCaption: false });

    var teacherRoomLabel = document.getElementById("teacherRoomTD");
    teacherRoomLabel.innerHTML = teacher.Room;

    var teacherCellphoneLabel = document.getElementById("teacherPhoneTD");
    teacherCellphoneLabel.innerHTML = teacher.Cellphone;

    var teacherEmailLink = document.getElementById("teacherEmailLink");
    var teacherEmail = "mailto:" + teacher.Email;
    teacherEmailLink.href = teacherEmail;
    teacherEmailLink.innerHTML =  teacher.Email;

}

function printTeacherScores() {
    for (var criteria in allCriterias) {
        var ratingName = document.getElementById("criteriaTextTD" + criteria);
        ratingName.innerHTML = allCriterias[criteria];
        $('#avgRating' + criteria).rating('update', teacher.AverageCriteriaRatings.AverageRatingsList[criteria]);
        $('#avgRating' + criteria).rating('refresh', { readonly: true, showClear: false, showCaption: false });
    }
}

function showNewCommentAction() {
    document.getElementById("newCommentTable").style.display = "block";
    document.getElementById("showNewCommentButtonTR").style.display = "none";
}

function showComments() {
    $('#allComments').empty();
    if (filteredComments == null || filteredComments.length == 0) {
        var noCommentsErrorLabel = document.createElement("h1");
        noCommentsErrorLabel.innerHTML = "אין תגובות להציג";
        noCommentsErrorLabel.className = "NoComments";
        $('#allComments').append(noCommentsErrorLabel);
        return;
    }
    for (comment in filteredComments) {
        printComment(filteredComments[comment], comment);
    }
}

function printComment(comment, itr) {
    var allComments = document.getElementById("allComments");
    var commentView = document.getElementById("commentTable").cloneNode(true);
    allComments.appendChild(commentView);
    commentView.style.display = 'block';
    commentView.id = "commentView" + itr;
    commentView.rows[0].cells[1].children[0].innerHTML = comment.CommentText;
    for (var rating in comment.CriteriaRatings) {
        var loadedComment = comment.CriteriaRatings[rating];
        var commentRating = commentView.rows[parseInt(rating) + 1].cells[1].firstElementChild.firstElementChild;
        var ratingIdName = "commentView" + itr + "Rating" + rating;
        commentRating.setAttribute('id', ratingIdName);
        $('#' + ratingIdName).rating({ 'size': 'xs' });
        $('#' + ratingIdName).rating('update', loadedComment.Rating);
        $('#' + ratingIdName).rating('refresh', { readonly: true, showClear: false, showCaption: false });
    }
    var commentDate = commentView.rows[6].children[1];
    commentDate.innerHTML = comment.DateTime.replace("T", " ");
    var likesCell = commentView.rows[5].children[1];
    var numberOfLikes = document.createElement("Label");
    numberOfLikes.id = "CommentNumber" + itr + "Likes";
    numberOfLikes.innerHTML = comment.TotalNumberOfLikes;
    numberOfLikes.className = "LikesLabel";
    var numberOfDislikes = document.createElement("Label");
    numberOfDislikes.id = "CommentNumber" + itr + "Dislikes";
    numberOfDislikes.innerHTML = comment.TotalNumberOfDislikes;
    numberOfDislikes.className = "DislikeLabel";
    var voteUpButton = document.createElement("Button");
    voteUpButton.id = "CommentNumber" + itr + "VoteUp";
    voteUpButton.value = comment.Id;
    var voteUpFunctionString = function () { addVote(numberOfLikes, comment.Id, true); };
    voteUpButton.onclick = voteUpFunctionString;
    voteUpButton.innerHTML = "👍";
    voteUpButton.className = "voteButton";
    var voteDownButton = document.createElement("Button");
    voteDownButton.id = "CommentNumber" + itr + "VoteDown";
    voteDownButton.value = comment.Id;
    voteDownButton.className = "voteButton";
    var voteDownFunctionString = function () { addVote(numberOfDislikes, comment.Id, false); };
    voteDownButton.onclick = voteDownFunctionString;
    voteDownButton.innerHTML = "👎";
    likesCell.appendChild(numberOfLikes);
    likesCell.appendChild(voteUpButton);
    likesCell.appendChild(document.createElement("BR"));
    likesCell.appendChild(numberOfDislikes);
    likesCell.appendChild(voteDownButton);
}

function showLoadingTeacherFailed() {
    alert("מרצה לא נמצא, הנך מועבר לעמוד הראשי.");
    TeacherInfoDiv.appendChild(failedSearchLabel);
}

function addComment() {
    if (document.getElementById("teacherNewCommetBox").value == "") {
        alert("הכנס תגובה");
        return;
    }
    var one = $('#newAvgRating0').val();
    var two = $('#newAvgRating1').val();
    var three = $('#newAvgRating2').val();
    var four = $('#newAvgRating3').val();
    var ratings = [one, two, three, four];
    for(var rate in ratings)
    {
        if (ratings[rate] == undefined || ratings[rate] == 0) {
            alert("עליך לדרג את כל הקריטריונים");
            return;
        }
    }
    var comment = {
        Id: teacher.Id,
        Ratings: ratings,
        Comment: document.getElementById("teacherNewCommetBox").value
    };
    $.ajax({
        type: "POST",
        data: JSON.stringify(comment),
        url: '/api/Teachers/AddComment',
        contentType: "application/json",
        success: function () {
            location.reload();
        },
        fail: function (jqXhr, textStatus) {
            alert("נכשל: " + textStatus);
        },
        async: false
    });
}

function setTeacherCommentsWithFilters() {
    var sortBy = {
        TeacherId: teacher.Id,
        sortByDate: $("#filteredByNew").is(':checked'),
        sortByLikes: $('#filterByLikes').is(':checked'),
    };
    var request = $.ajax({
        type: "POST",
        data: JSON.stringify(sortBy),
        url: '/api/Teachers/GetSortedTeacherComments',
        contentType: "application/json",
        success: function (data) {
            filteredComments = data;
            showComments();
        },
        fail: function (jqXhr, textStatus) {
            filteredComments = null;
            showComments();
        },
        async: false
    });

}
