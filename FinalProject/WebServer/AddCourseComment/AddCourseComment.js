var uri = '/api/Courses/GetCriterias';
var uri2 = '/api/Courses/AddComment';
var uri3 = '/api/Courses/GetTeachers';
var uri4 = '/api/Courses/GetCourse';
var uri5 = '/api/Courses/AddVote';
var uri6 = '/api/AddFile/AddSyllabus';

var course;
var allCriterias;
var numberOfCommentsLoaded;

var CourseInfoDiv = document.getElementById("CourseInfoDiv");
var NewSyllabusDiv = document.getElementById("NewSyllabusDiv");
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

function printCourseInfo() {
    printCourseProperties();
    showCourseComments();
}

function printCourseProperties() {
    var courseNameTD = document.getElementById("courseNameTD");
    courseNameTD.innerHTML = course.Name;

    var courseAvgTD = document.getElementById("courseAvgTD");
    courseAvgTD.innerHTML = course.Score;

    var courseFacultyTD = document.getElementById("courseFacultyTD");
    courseFacultyTD.innerHTML = facultyNameByEnum(course.Faculty);

    var courseObligtoryTD = document.getElementById("courseObligtoryTD");
    courseObligtoryTD.innerHTML = course.IsMandatory ? "כן" : "לא";

    var courseOfDegreeTD = document.getElementById("courseOfDegreeTD");
    courseOfDegreeTD.innerHTML = academicDegreeNameByEnum(course.AcademicDegree);

    var courseYearTD = document.getElementById("courseYearTD");
    courseYearTD.innerHTML = intendedYearNameByEnum(course.IntendedYear);
}

function showCourseComments(sortByNew) {
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

function printComment(comment, itr) {
    var allComments = document.getElementById("allComments");
    var commentView = document.getElementById("commentTable").cloneNode(true);
    commentView.style.display = 'block';
    commentView.id = "commentView" + itr;
    commentView.rows[0].cells[1].children[0].innerHTML = comment.CommentText;
    var likesCell = commentView.rows[0].cells[1];
    likesCell.style.textAlign = "center";
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
    likesCell.appendChild(numberOfDislikes);
    likesCell.appendChild(voteDownButton);
    likesCell.appendChild(document.createElement("BR"));
    likesCell.appendChild(numberOfLikes);
    likesCell.appendChild(voteUpButton);

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

function showNewCommentAction() {
    document.getElementById("newCommentTable").style.display = "block";
    document.getElementById("showNewCommentButtonTR").style.display = "none";
    revealAddCommentViewToUser();
}

function revealAddCommentViewToUser() {
    var newCommentTable = document.getElementById("newCommentTable");
    var newCommentCriteriaTR = document.getElementById("criteriaTR");
    var courseSemestersTR = document.getElementById("courseSemestersTR");
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
    var courseSemesters = document.getElementById("courseSemesters");
    for (semester in course.CourseInSemesters) {
        var semesterOption = document.createElement("option");
        semesterOption.text = course.CourseInSemesters[semester].Year + " " + semesterNameByEnum(course.CourseInSemesters[semester].Semester);
        semesterOption.value = course.CourseInSemesters[semester].Id
        courseSemesters.add(semesterOption);
    }
    var sendTR = document.getElementById("sendTR");
    sendTR.parentNode.removeChild(sendTR);
    newCommentTable.appendChild(sendTR);
}

function showLoadingCourseFailed() {
    alert("קורס לא נמצא, הנך מועבר לעמוד הראשי.");
    parent.location = "../HomePage/HomePage.html";
}

function addComment() {
    if (document.getElementById("CourseNewCommetBox").value == "") {
        alert("הכנס תגובה");
        return;
    }
    var ratings = [];
    for (criteria in allCriterias) {
        ratings.push(getSelectedRadioButtonValue(document.getElementById("criteriaRating" + criteria).children[0]));
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

function facultyNameByEnum(faculty) {
    switch (faculty) {
        case 0:
            return "מדעי המחשב";
            break;
        case 1:
            return "מדעי ההתנהגות";
            break;
        case 2:
            return "מערכות מידע";
            break;
        case 3:
            return "אחיות";
            break;
        case 4:
            return "כלכלה וניהול";
            break;
        case 5:
            return "פוליטיקה וממשל";
            break;
        case 6:
            return "פיתוח ארגוני";
            break;
        case 7:
            return "מנהל עסקים";
            break;
        case 8:
            return "פסיכולוגיה";
            break;
        default:
            return "";
            break;
    }
}

function academicDegreeNameByEnum(degree) {
    switch (degree) {
        case 0:
            return "תואר ראשון";
        case 1:
            return "תואר שני";
        default:
            return "אין";
    }
}

function intendedYearNameByEnum(year) {
    switch (year) {
        case 0:
            return "הכל";
        case 1:
            return "שנה ראשונה";
        case 2:
            return "שנה שניה";
        case 3:
            return "שנה שלישית";
        case 4:
            return "שנה רביעית";
        default:
            "אין";
    }
}

function semesterNameByEnum(semester) {
    switch (semester) {
        case 0:
            return "א";
        case 1:
            return "ב";
        case 2:
            return "קיץ";
        default:
            "";
    }
}