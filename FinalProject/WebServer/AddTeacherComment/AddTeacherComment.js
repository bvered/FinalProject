var uri = '/api/Teachers/GetCriterias';
var uri2 = '/api/Teachers/AddComment';
var uri3 = '/api/Teachers/GetTeachers';
var uri4 = '/api/Teachers/GetTeacher';
var uri5 = 'api/Teachers/GetCommentById';

var teacher;
var allCriterias;
var allTeachers;
var teacherInfoList;
var numberOfCommentsLoaded;

var TeacherInfoDiv = document.getElementById("TeacherInfoDiv");
var newCommentDiv = document.getElementById("NewCommentDiv");
var uniDiv = document.getElementById("UniversitiesDiv");
var teacherCourseDiv = document.getElementById("CoursesDiv");
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

function loadTeacher() {
    id = getParameterByName('id');
    succeed = false;

    var request = $.ajax({
        type: "GET",
        url: uri4 + "/" + id,
        contentType: "application/json",
        success: function (data) {
            if (data.length == 1) {
                teacher = data[0];
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

function printInformationOfTeacher() {
    showTeacherInfoToUser();
    showCommentOptions();
    showTeacherUniversities();
    showTeacherCourses()
    showTeacherCommentsByTeacher();
}

function showTeacherInfoToUser() {
    var teacherNameLabel = document.createElement("Label");
    teacherNameLabel.id = "propertyLabelTeacherName";
    teacherNameLabel.innerHTML = "Teacher name: " + teacher.Name;
    TeacherInfoDiv.appendChild(teacherNameLabel);
    TeacherInfoDiv.appendChild(document.createElement('br'));

    var teacherScoreLabel = document.createElement("Label");
    teacherScoreLabel.id = "propertyLabelTeacherScore";
    teacherScoreLabel.innerHTML = "Teacher average score: " + teacher.Score;
    TeacherInfoDiv.appendChild(teacherScoreLabel);
    TeacherInfoDiv.appendChild(document.createElement('br'));
}

function showTeacherUniversities() {
    var universityLabel = document.createElement("Label");
    universityLabel.id = "universityLabel";
    universityLabel.innerHTML = "Universities: ";
    uniDiv.appendChild(universityLabel);
    uniDiv.appendChild(document.createElement('br'));
    if (teacher.Universities.length > 0) {
        for (university in teacher.Universities) {
            var universityLink = document.createElement('a');
            universityLink.id = "universityLink" + university;
            universityLink.href = '/University/University.html&University=' + teacher.Universities[university].Name;
            universityLink.innerHTML = teacher.Universities[university].Name;
            uniDiv.appendChild(universityLink);
            uniDiv.appendChild(document.createElement('br'));
        }
    }
    else {
        var noUniversityLabel = document.createElement("Label");
        noUniversityLabel.id = "noUniversityLabel";
        noUniversityLabel.innerHTML = "No universities found for: " + Teacher.Name + ".";
        TeacherInfoDiv.appendChild(noUniversityLabel);
    }
}

function showTeacherCourses() {
    var courseLabel = document.createElement("Label");
    courseLabel.id = "courseLabel";
    courseLabel.innerHTML = "Courses";
    teacherCourseDiv.appendChild(courseLabel);
    teacherCourseDiv.appendChild(document.createElement('br'));
    if (teacher.Courses.length > 0) {
        for (course in teacher.Courses) {
            var courseButton = document.createElement('a');
            courseButton.id = "courseLink" + course;
            courseButton.href = '/Course/Course.html&Course=' + teacher.Courses[course].Name;
            courseButton.innerHTML = teacher.Courses[course].Name;
            teacherCourseDiv.appendChild(courseButton);
            teacherCourseDiv.appendChild(document.createElement('br'));
        }
    }
    else {
        var noCourseLabel = document.createElement("Label");
        noCourseLabel.id = "noCourseLabel";
        noCourseLabel.innerHTML = "No courses found for: " + teacher.Name + ".";
        teacherCourseDiv.appendChild(noCourseLabel);
        teacherCourseDiv.appendChild(document.createElement('br'));
    }
}

function showTeacherCommentsByTeacher() {
    var commentsLabel = document.createElement("Label");
    commentsLabel.id = "commentsLabel";
    commentsLabel.innerHTML = "Comments";
    teacherCourseDiv.appendChild(commentsLabel);
    teacherCommentsDiv.appendChild(document.createElement('br'));

    var allComments = teacher.TeacherComments;
    numberOfCommentsLoaded = allComments.length;
    for (comment in numberOfCommentsLoaded) {
        printComment(allComments[comment], comment);
    }
}

function printComment(comment, itr) {
    var commentBox = document.createElement("textarea");
    commentBox.placeholder = comment.CommentText;
    commentBox.id = "TeacherCommet" + itr;
    teacherCommentsDiv.appendChild(commentBox);
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

            teacherCommentsDiv.appendChild(labelForRatingString);
            teacherCommentsDiv.appendChild(labelForRatingNumber);
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
            if (data.length == 1) {
                loadedComment = data;
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
}

function showCommentOptions() {
    var commentBox = document.createElement("textarea");
    commentBox.placeholder = "Write something..";
    commentBox.id = "TeacherNewCommetBox";
    teacherCommentsDiv.appendChild(commentBox);
    teacherCommentsDiv.appendChild(document.createElement('br'));
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

        teacherCommentsDiv.appendChild(labelInput);
        teacherCommentsDiv.appendChild(inputText);

        teacherCommentsDiv.appendChild(document.createElement("BR"));
    }
    var sendButton = document.createElement("BUTTON");
    sendButton.innerHTML = "Add";
    sendButton.onclick = addComment;
    teacherCommentsDiv.appendChild(sendButton);
}

function showLoadingTeacherFailed() {
    var failedSearchLabel = document.createElement("Label");
    failedSearchLabel.id = "failedSearchLabel";
    failedSearchLabel.innerHTML = "Failed to find user, re-directing to homepage";
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