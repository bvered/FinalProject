var uri = '/api/Courses/GetCriterias';
var uri2 = '/api/Courses/AddComment';
var uri3 = '/api/Courses/GetTeachers';
var uri4 = '/api/Courses/GetCourse';
var uri5 = 'api/Courses/GetCommentById';
var uri6 = 'api/Courses/GetAllSemesters';

var course;
var allCriterias;
var numberOfCommentsLoaded;

var CourseInfoDiv = document.getElementById("CourseInfoDiv");
var NewCourseCommentDiv = document.getElementById("NewCourseCommentDiv");
var CommentsDiv = document.getElementById("CommentsDiv");

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function loadView() {
    if (loadCommentsCriteras() == false) {
        showLoadingCourseFailed();
        return;
    }
    didSucceedLoadingTeacher = loadCourse();
    if (didSucceedLoadingTeacher == true) {
        printCourseInfo();
    } else {
        showLoadingCourseFailed();
    }
}
function loadCommentsCriteras() {
    succeed = false;

    var request = $.ajax({
        type: "GET",
        url: uri,
        contentType: "application/json",
        success: function (data) {
            if (data.length > 0) {
                allCriterias = data;
                succeed = true;
            } else {
                succeed == false;
            }
        },
        fail: function (data) {
            succeed = false;
        },
        async: false
    });
    return succeed;
}

function loadCourse() {
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

function printCourseInfo() {
    printCourseProperties();
    showCommentOptions();
    showCourseComments();
}

function printCourseProperties() {
    var teacherNameLabel = document.createElement("Label");
    courseNameLabel.id = "propertyLabelCourseName";
    courseNameLabel.innerHTML = "Course name: " + teacher.Name;
    CourseInfoDiv.appendChild(courseNameLabel);
    CourseInfoDiv.appendChild(document.createElement('br'));

    var courseScoreLabel = document.createElement("Label");
    courseScoreLabel.id = "propertyLabelCourseScore";
    courseScoreLabel.innerHTML = "Course average score: " + course.Score;
    CourseInfoDiv.appendChild(courseScoreLabel);
    CourseInfoDiv.appendChild(document.createElement('br'));
}

function showCourseUniversity() {
    var universityLabel = document.createElement("Label");
    universityLabel.id = "universityLabel";
    universityLabel.innerHTML = "University: ";
    CourseInfoDiv.appendChild(universityLabel);
    var universityLink = document.createElement("a");
    universityLink.href = '/University/University.html/' + course.University.Id;
    universityLink.innerHTML = course.University.Name;
    CourseInfoDiv.appendChild(universityLink);
    CourseInfoDiv.appendChild(document.createElement('br'));
}

function showCourseComments() {
    var commentsLabel = document.createElement("Label");
    commentsLabel.id = "commentsLabel";
    commentsLabel.innerHTML = "Comments";
    CommentsDiv.appendChild(commentsLabel);
    CommentsDiv.appendChild(document.createElement('br'));

    var allComments = course.CourseComments;
    numberOfCommentsLoaded = allComments.length;
    for (comment in numberOfCommentsLoaded) {
        printComment(allComments[comment], comment);
    }
}

function printComment(comment, itr) {
    var commentBox = document.createElement("textarea");
    commentBox.placeholder = comment.CommentText;
    commentBox.id = "TeacherCommet" + itr;
    CommentsDiv.appendChild(commentBox);
    for (rating in comment.CriteriaRatings) {
        var succeed = false;
        var loadedComment;
        loadComment(comment.CriteriaRatings[rating].Id, loadedComment, succeed);
        if (succeed) {
            var ratingString = loadedComment.Criteria.DisplayName;
            var ratingNumber = loadedComment.Rating;

            var labelForRatingString = document.createElement("Label");
            labelForRatingString.id = "CommentNumber" + itr + "RatingString" + rating;
            labelForRatingString.innerHTML = ratingString;

            var labelForRatingNumber = document.createElement("Label");
            labelForRatingNumber.id = "CommentNumber" + itr + "RatingNumber" + rating;
            labelForRatingNumber.innerHTML = ratingString;

            CommentsDiv.appendChild(labelForRatingString);
            CommentsDiv.appendChild(labelForRatingNumber);
        }
    }
}

function loadComment(commentId, loadedComment, succeed) {
    succeed = false;

    var request = $.ajax({
        type: "GET",
        url: uri5 + "/" + commentId,
        contentType: "application/json",
        success: function (data) {
            loadedComment = data;
            succeed = true;
        },
        fail: function (data) {
            succeed = false;
        },
        async: false
    });
}

function showCommentOptions() {
    var commentBox = document.createElement("textarea");
    commentBox.placeholder = "Add new comment..";
    commentBox.id = "CourseNewCommetBox";
    NewCourseCommentDiv.appendChild(commentBox);
    NewCourseCommentDiv.appendChild(document.createElement('br'));
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

        NewCourseCommentDiv.appendChild(labelInput);
        NewCourseCommentDiv.appendChild(inputText);

        NewCourseCommentDiv.appendChild(document.createElement("BR"));
    }
    var sendButton = document.createElement("BUTTON");
    sendButton.innerHTML = "Add";
    sendButton.onclick = addComment;
    NewCourseCommentDiv.appendChild(sendButton);
}

function showLoadingCourseFailed() {
    var failedSearchLabel = document.createElement("Label");
    failedSearchLabel.id = "failedSearchLabel";
    failedSearchLabel.innerHTML = "Failed to find user, re-directing to homepage";
    CourseInfoDiv.appendChild(failedSearchLabel);
}

function addComment() {
    var ratings = [];
    for (criteria in allCriterias) {
        ratings.push(document.getElementById("criteriaRating" + criteria).value);
    }
    var comment = {
        Id: course.Id,
        Ratings: ratings,
        Comment: document.getElementById("TeacherNewCommetBox").value,
        SemseterId: document.getElementById("semsterChosen").value,
    };
    var request = $.ajax({
        type: "POST",
        data: JSON.stringify(comment),
        url: uri2,
        contentType: "application/json",
        success: function (data) {
            alert("New comment added!");
            numberOfCommentsLoaded = numberOfCommentsLoaded + 1;
            printComment(data, numberOfCommentsLoaded)
        },
        fail: function (jqXhr, textStatus) {
            alert("Request failed: " + textStatus);
        },
        async: false
    });
    removingAllContentOfDiv(newCommentDiv);
}

function insertAddCommentButton() {
    removingAllContentOfDiv(teacherCommentsDiv);
    var addButton = document.createElement("BUTTON");
    sendButton.innerHTML = "Comment";
    sendButton.onclick = addComment;
    teacherCommentsDiv.appendChild(sendButton);
}

function removingAllContentOfDiv(div) {
    while (div.hasChildNodes()) {
        node.removeChild(div.lastChild);
    }
}