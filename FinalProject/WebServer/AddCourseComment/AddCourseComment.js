var uri = '/api/Courses/GetCriterias';
var uri2 = '/api/Courses/AddComment';
var uri3 = '/api/Courses/GetTeachers';
var uri4 = '/api/Courses/GetCourse';
var uri5 = 'api/Courses/GetAllSemesters';

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
    didSucceedLoadingCourse = loadCourse();
    if (didSucceedLoadingCourse == true) {
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
            course = data;
            succeed = true;
        },
        fail: function (data) {
            succeed = false;
        },
        async: false
    });
    return succeed;
}

function addVote(voteValueLabel, id, like) {
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


function printCourseInfo() {
    printCourseProperties();
    showCommentOptions();
    showCourseComments();
}

function printCourseProperties() {
    var courseNameLabel = document.createElement("Label");
    courseNameLabel.id = "propertyLabelCourseName";
    courseNameLabel.innerHTML = "שם קורס: " + course.Name;
    CourseInfoDiv.appendChild(courseNameLabel);
    CourseInfoDiv.appendChild(document.createElement('br'));

    var courseScoreLabel = document.createElement("Label");
    courseScoreLabel.id = "propertyLabelCourseScore";
    courseScoreLabel.innerHTML = "ממוצע דירוגים: " + course.Score;
    CourseInfoDiv.appendChild(courseScoreLabel);
    CourseInfoDiv.appendChild(document.createElement('br'));

    var courseFacultyLabel = document.createElement("Label");
    courseFacultyLabel.id = "propertyLabelCourseScore";
    courseFacultyLabel.innerHTML = "פקולטה: " + course.Faculty;
    CourseInfoDiv.appendChild(courseFacultyLabel);
    CourseInfoDiv.appendChild(document.createElement('br'));

    var courseMandtoryLabel = document.createElement("Label");
    courseMandtoryLabel.id = "propertyLabelCourseScore";
    courseMandtoryLabel.innerHTML = "חובה: " + course.IsMandatory;
    CourseInfoDiv.appendChild(courseMandtoryLabel);
    CourseInfoDiv.appendChild(document.createElement('br'));

    var courseAcademicDegreeLabel = document.createElement("Label");
    courseAcademicDegreeLabel.id = "propertyLabelCourseScore";
    courseAcademicDegreeLabel.innerHTML = "תואר: " + course.AcademicDegree;
    CourseInfoDiv.appendChild(courseAcademicDegreeLabel);
    CourseInfoDiv.appendChild(document.createElement('br'));

    var courseIntendedYearLabel = document.createElement("Label");
    courseIntendedYearLabel.id = "propertyLabelCourseScore";
    courseIntendedYearLabel.innerHTML = "שנה: " + course.IntendedYear;
    CourseInfoDiv.appendChild(courseIntendedYearLabel);
    CourseInfoDiv.appendChild(document.createElement('br'));
}

function showCourseComments() {
    var commentsLabel = document.createElement("Label");
    commentsLabel.id = "commentsLabel";
    commentsLabel.innerHTML = "תגובות";
    CommentsDiv.appendChild(commentsLabel);
    CommentsDiv.appendChild(document.createElement('br'));

    for (courseSemester in course.CourseInSemesters) {
        for (comment in course.CourseInSemesters[courseSemester].CourseComments) {
            printComment(course.CourseInSemesters[courseSemester].CourseComments[comment], comment);
        }
    }
}

function printComment(comment, itr) {
    var commentBox = document.createElement("textarea");
    commentBox.placeholder = comment.CommentText;
    commentBox.id = "CourseCommet" + itr;
    CommentsDiv.appendChild(commentBox);
    CommentsDiv.appendChild(document.createElement('br'));

    var numberOfVotes = document.createElement("Label");
    numberOfVotes.id = "CommentNumber" + itr + "Votes";
    numberOfVotes.innerHTML = " אהבו: " + comment.TotalNumberOfLikes;
    var voteUpButton = document.createElement("Button");
    voteUpButton.id = "CommentNumber" + itr + "VoteUp";
    voteUpButton.value = comment.id;
    var voteUpFunctionString = function () { addVote(numberOfVotes, comment.id, true); };
    voteUpButton.onclick = voteUpFunctionString;
    voteUpButton.innerHTML = "Like";
    var voteDownButton = document.createElement("Button");
    voteDownButton.id = "CommentNumber" + itr + "VoteDown";
    voteDownButton.value = comment.id;
    var voteDownFunctionString = function () { addVote(numberOfVotes, comment.id, false); };
    voteDownButton.onclick = voteDownFunctionString;
    voteDownButton.innerHTML = "Dislike";
    CommentsDiv.appendChild(voteDownButton);
    CommentsDiv.appendChild(numberOfVotes);
    CommentsDiv.appendChild(voteUpButton);
    CommentsDiv.appendChild(document.createElement('br'));

    for (rating in comment.CriteriaRatings) {
        var loadedComment = comment.CriteriaRatings[rating];
            var ratingString = loadedComment.Criteria.DisplayName;
            var ratingNumber = loadedComment.Rating;

            var labelForRatingString = document.createElement("Label");
            labelForRatingString.id = "CommentNumber" + itr + "RatingString" + rating;
            labelForRatingString.innerHTML = ratingString;

            var labelForRatingNumber = document.createElement("Label");
            labelForRatingNumber.id = "CommentNumber" + itr + "RatingNumber" + rating;
            labelForRatingNumber.innerHTML = ratingNumber;

            CommentsDiv.appendChild(labelForRatingString);
            CommentsDiv.appendChild(document.createElement('br'));
            CommentsDiv.appendChild(labelForRatingNumber);
            CommentsDiv.appendChild(document.createElement('br'));
    }
}

function showCommentOptions() {
    removingAllContentOfDiv(NewCourseCommentDiv);
    var addNewCommentButton = document.createElement("Button");
    addNewCommentButton.innerHTML = "הוסף תגובה חדשה";
    addNewCommentButton.onclick = revealAddCommentViewToUser;
    NewCourseCommentDiv.appendChild(addNewCommentButton);
    NewCourseCommentDiv.appendChild(document.createElement("BR"));}

function revealAddCommentViewToUser() {
    removingAllContentOfDiv(NewCourseCommentDiv);
    var commentBox = document.createElement("textarea");
    commentBox.placeholder = "תגובה חדשה..";
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
    var courseSemesters = document.createElement("SELECT");
    courseSemesters.id = "courseSemesters";
    for (semester in course.CourseInSemesters) {
        var semesterOption = document.createElement("option");
        semesterOption.text = course.CourseInSemesters[semester].Year + " " + course.CourseInSemesters[semester].Semester;
        semesterOption.value = course.CourseInSemesters[semester].Id
        courseSemesters.add(semesterOption);
    }
    NewCourseCommentDiv.appendChild(courseSemesters);
    var sendButton = document.createElement("BUTTON");
    sendButton.innerHTML = "Add";
    sendButton.onclick = addComment;
    NewCourseCommentDiv.appendChild(sendButton);
}

function showLoadingCourseFailed() {
    var failedSearchLabel = document.createElement("Label");
    failedSearchLabel.id = "failedSearchLabel";
    failedSearchLabel.innerHTML = "קורס לא נמצא, הנך מועבר לעמוד הראשי.";
    CourseInfoDiv.appendChild(failedSearchLabel);
}

function addComment() {
    var ratings = [];
    for (criteria in allCriterias) {
        ratings.push(document.getElementById("criteriaRating" + criteria).value);
    }
    var semester;
    if (document.getElementById("courseSemesters") == null) {
        semester = "";
    } else {
        semester = document.getElementById("courseSemesters").value;
    }
    var comment = {
        Id: course.Id,
        Ratings: ratings,
        Comment: document.getElementById("CourseNewCommetBox").value,
        SemseterId: semester,
    };
    var request = $.ajax({
        type: "POST",
        data: JSON.stringify(comment),
        url: uri2,
        contentType: "application/json",
        success: function (data) {
            alert("תגובך הוספה בהצלחה!");
            location.reload();
        },
        fail: function (jqXhr, textStatus) {
            alert("Request failed: " + textStatus);
        },
        async: false
    });
}

function removingAllContentOfDiv(div) {
    while (div.hasChildNodes()) {
        div.removeChild(div.lastChild);
    }
}