var teacher;
var allCriterias;
var numberOfCommentsLoaded;
var queryString;
var currentUniversity;

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

function printInformationOfTeacher() {
    showTeacherInfoToUser();
    printTeacherScores();
    showTeacherCommentsByTeacher(false);
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

function showTeacherCommentsByTeacher(sortByNew) {
    var allComments = teacher.TeacherComments;
    if(!sortByNew) {
        allComments = allComments.sort(function (a, b) {
            return b.TotalNumberOfLikes - a.TotalNumberOfLikes;
        });
    }
    numberOfCommentsLoaded = allComments.length;
    for (var comment = 0; comment < numberOfCommentsLoaded; comment++) {
        printComment(allComments[comment], comment);
    }
}

function printComment(comment, itr) {
    var allComments = document.getElementById("allComments");
    var commentView = document.getElementById("commentTable").cloneNode(true);
    commentView.style.display = 'block';
    commentView.id = "commentView" + itr;
    commentView.rows[0].cells[1].children[0].innerHTML = comment.CommentText;
    for (var rating in comment.CriteriaRatings) {
        var loadedComment = comment.CriteriaRatings[rating];
        var clonedCommentCriteriaTR = document.getElementById("criteriaTR").cloneNode(true);
        clonedCommentCriteriaTR.style.display = 'block';
        clonedCommentCriteriaTR.children[0].innerHTML = loadedComment.Criteria.DisplayName;
        setSelectedRadionButtonValue(clonedCommentCriteriaTR.children[1].children[0], loadedComment.Rating);
        commentView.appendChild(clonedCommentCriteriaTR);
    }
    var commentDate = commentView.rows[2].children[1];
    commentDate.innerHTML = comment.DateTime.replace("T", " ");
    var likesCell = commentView.rows[1].children[1];
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
    allComments.appendChild(commentView);
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
    var one = $('input[name=star]:checked', '#ratingsForm1').val();
    var two = $('input[name=star2]:checked', '#ratingsForm2').val();
    var three = $('input[name=star3]:checked', '#ratingsForm3').val();
    var four = $('input[name=star4]:checked', '#ratingsForm4').val();
    var five = $('input[name=star5]:checked', '#ratingsForm5').val();
    var ratings = [one, two, three, four, five];
    for(var rate in ratings)
    {
        if (ratings[rate] == undefined) {
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
            alert("תגובתך הוספה בהצלחה");
            location.reload();
        },
        fail: function (jqXhr, textStatus) {
            alert("נכשל: " + textStatus);
        },
        async: false
    });
}

function getSelectedRadioButtonValue(radioButtonForm) {
    for (var star in radioButtonForm.children[0].children) {
        if (radioButtonForm.children[0].children[star].checked == true) {
            return radioButtonForm.children[0].children[star].value;
        }
    }
    return 0;
}

function setSelectedRadionButtonValue(radioButtonForm, value) {
    for (var radioButton in radioButtonForm.children[0].children) {
        if (radioButtonForm.children[0].children[radioButton].id == ("star-" + value)) {
            radioButtonForm.children[0].children[radioButton].checked = true;
            return;
        }
    }
}