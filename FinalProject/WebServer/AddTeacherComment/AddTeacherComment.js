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
    var didLikeBefore = $.jStorage.get(id);
    if (didLikeBefore == true) {
        alert("מותר לדרג פעם אחת");
        return succeed;
    }
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
            $.jStorage.set(id, true);
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
    showTeacherCommentsByTeacher(false);
}

function showTeacherInfoToUser() {
    var teacherNameLabel = document.getElementById("teacherNameTD");
    teacherNameLabel.innerHTML = teacher.Name;

    var teacherScoreLabel = document.getElementById("teacherAvgTD");
    teacherScoreLabel.innerHTML = teacher.Score;

    var teacherRoomLabel = document.getElementById("teacherRoomTD");
    teacherRoomLabel.innerHTML = teacher.Room;

    var teacherCellphoneLabel = document.getElementById("teacherPhoneTD");
    teacherCellphoneLabel.innerHTML = teacher.Cellphone;

    var teacherEmailLabel = document.getElementById("teacherEmailTD");
    teacherEmailLabel.innerHTML = teacher.Email;

    //for (avgRating in allCriterias) {
    //    var AvgRating = document.getElementById("AvgRating#").cloneNode(true);
    //    AvgRating.style.display = 'block';
    //    AvgRating.id = "AvgRating#"+avgRating;
    //    AvgRating.cells[0].innerHTML = allCriterias[avgRating];
    //    AvgRating.cells[1].innerHTML = teacher.RatingScores[avgRating];
    //    document.getElementById("teacherInfo").appendChild(AvgRating);
    //}
}

function showNewCommentAction() {
    document.getElementById("newCommentTable").style.display = "block";
    document.getElementById("showNewCommentButtonTR").style.display = "none";
    revealAddCommentViewToUser();
}

function revealAddCommentViewToUser() {
    var newCommentTable = document.getElementById("newCommentTable");
    var newCommentCriteriaTR = document.getElementById("criteriaTR");
    for (ratingText in allCriterias) {
        var clonedCommentCriteriaTR = newCommentCriteriaTR.cloneNode(true);
        clonedCommentCriteriaTR.id = "criteriaTR" + ratingText;
        clonedCommentCriteriaTR.style.display = "block";
        var criteriaTextTD = clonedCommentCriteriaTR.children[0];
        criteriaTextTD.id = "criteriaText" + ratingText;
        criteriaTextTD.innerHTML = allCriterias[ratingText];
        var starRatingTD = clonedCommentCriteriaTR.children[1];
        starRatingTD.id = "criteriaRating" + ratingText;
        starRatingTD.children[0].id = "starRating" + ratingText;
        newCommentTable.appendChild(clonedCommentCriteriaTR);
    }
    var sendTR = document.getElementById("sendTR");
    sendTR.parentNode.removeChild(sendTR);
    newCommentTable.appendChild(sendTR);
}

function showTeacherComments(sortByNew) {
    var newCommentDiv = document.getElementById("allComments").clear();
    while (newCommentDiv.firstChild) {
        newCommentDiv.removeChild(newCommentDiv.firstChild);
    }
    for (courseSemester in course.CourseInSemesters) {
        var semesterComments = course.CourseInSemesters[courseSemester].CourseComments;
        if (!sortByNew) {
            semesterComments = semesterComments.sort(function (a, b) {
                return b.TotalNumberOfLikes - a.TotalNumberOfLikes
            });
        }
        for (comment in semesterComments) {
            printComment(course.CourseInSemesters[courseSemester].CourseComments[comment], comment);
        }
    }
}

function showTeacherCommentsByTeacher(sortByNew) {
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
    var allComments = document.getElementById("allComments");
    var commentView = document.getElementById("commentTable").cloneNode(true);
    commentView.style.display = 'block';
    commentView.id = "commentView" + itr;
    commentView.rows[0].cells[1].children[0].innerHTML = comment.CommentText;
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


    //var numberOfVotes = document.createElement("Label");
    //numberOfVotes.id = "CommentNumber" + itr + "Votes";
    //numberOfVotes.innerHTML = " אהבו: " + comment.TotalNumberOfLikes;
    //var voteUpButton = document.createElement("Button");
    //voteUpButton.className = "voteButton"
    //voteUpButton.id = "CommentNumber" + itr + "VoteUp";
    //voteUpButton.value = comment.Id;
    //var voteUpFunctionString = function () { addVote(numberOfVotes, comment.Id, true); };
    //voteUpButton.onclick = voteUpFunctionString;
    //voteUpButton.innerHTML = "👍";
    //var voteDownButton = document.createElement("Button");
    //voteDownButton.className = "voteButton";
    //voteDownButton.id = "CommentNumber" + itr + "VoteDown";
    //voteDownButton.value = comment.Id;
    //var voteDownFunctionString = function () { addVote(numberOfVotes, comment.Id, false); };
    //voteDownButton.onclick = voteDownFunctionString;
    //voteDownButton.innerHTML = "👎";
    //likesCell.appendChild(voteDownButton);
    //likesCell.appendChild(numberOfVotes);
    //likesCell.appendChild(voteUpButton);

    for (rating in comment.CriteriaRatings) {
        var loadedComment = comment.CriteriaRatings[rating];
        var clonedCommentCriteriaTR = document.getElementById("criteriaTR").cloneNode(true);
        clonedCommentCriteriaTR.style.display = 'block';
        clonedCommentCriteriaTR.children[0].innerHTML = loadedComment.Criteria.DisplayName;
        setSelectedRadionButtonValue(clonedCommentCriteriaTR.children[1].children[0], loadedComment.Rating);
        commentView.appendChild(clonedCommentCriteriaTR);
    }
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
    var ratings = [];
    for (criteria in allCriterias) {
        ratings.push(getSelectedRadioButtonValue(document.getElementById("criteriaRating" + criteria).children[0]));
    }
    var comment = {
        Id: teacher.Id,
        Ratings: ratings,
        Comment: document.getElementById("teacherNewCommetBox").value,
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

function getSelectedRadioButtonValue(radioButtonForm) {
    for (star in radioButtonForm.children[0].children) {
        if (radioButtonForm.children[0].children[star].checked == true) {
            return radioButtonForm.children[0].children[star].value;
        }
    }
    return 0;
}

function setSelectedRadionButtonValue(radioButtonForm, value) {
    for (radioButton in radioButtonForm.children[0].children) {
        if (radioButtonForm.children[0].children[radioButton].id == ("star-" + value)) {
            radioButtonForm.children[0].children[radioButton].checked = true;
            return;
        }
    }
}