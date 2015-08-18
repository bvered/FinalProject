var uri = '/api/Courses/GetCriterias';
var uri2 = '/api/Courses/AddComment';
var uri3 = '/api/Courses/GetTeachers';
var uri4 = '/api/Courses/GetCourse';
var uri5 = '/api/Courses/AddVote';
var uri6 = '/api/AddFile/AddSyllabus';

var course;
var allCriterias;
var numberOfCommentsLoaded;
var filteredComments;
var allCourseTeachers;

var CourseInfoDiv = document.getElementById("CourseInfoDiv");
var NewSyllabusDiv = document.getElementById("NewSyllabusDiv");
var NewCourseCommentDiv = document.getElementById("NewCourseCommentDiv");
var CommentsDiv = document.getElementById("CommentsDiv");

var queryString;
var currentUniversity;

$(document).ready(function () {
    queryString = getQuertyString();
    $('#University').attr('value', queryString["University"]);
    currentUniversity = queryString["University"];
});

function homePage() {
    window.location = "../HomePage/HomePage.html?University=" + currentUniversity;
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function loadView() {
    if (!loadCommentsCriteras() || !allTeachers()) {
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
    var didLikeBefore = $.jStorage.get(id);
    if (didLikeBefore == true) {
        alert("מותר לדרג פעם אחת");
        return succeed;
    }
    var request = $.ajax({
        type: "POST",
        data: JSON.stringify(vote),
        url: uri5,
        contentType: "application/json",
        success: function (data) {
            voteValueLabel.innerHTML = data;
            $.jStorage.set(id, true);
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
    printCourseScores();
    setupFilters();
    setCourseCommentsWithFilters();
}

function setupFilters() {
    var teachersToFilterBy = [];
    var yearToFilterBy = [];
    for (courseInSemster in course.CourseInSemesters) {
        teachersToFilterBy.push(course.CourseInSemesters[courseInSemster].Teacher);
        yearToFilterBy.push(course.CourseInSemesters[courseInSemster].Year);
    };
    teachersToFilterBy = teachersToFilterBy.filter(function (itm, i, a) {
        return i == a.indexOf(itm);
    });
    yearToFilterBy = yearToFilterBy.filter(function (itm, i, a) {
        return i == a.indexOf(itm);
    });
    for (year in yearToFilterBy) {
        $('#filteredByYear').append($('<option>', {
            value: yearToFilterBy[year],
            text: yearToFilterBy[year],
        }));
    }
    for (var teacher in teachersToFilterBy) {
        $('#filterByTeacher').append($('<option>', {
            value: teachersToFilterBy[teacher].Id,
            text: teachersToFilterBy[teacher].Name,
        }));
    }
    for (teacher in allCourseTeachers) {
        $('#allCourseTeachers').append($('<option>', {
            value: teacher,
            text: allCourseTeachers[teacher],
        }));
    };
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

function printCourseScores() {
    for (criteria in allCriterias) {
        var clonedBasedRatingTR = document.getElementById("newCriteriaRatingBase1").cloneNode(true);
        clonedBasedRatingTR.id = "newCriteriaRatingBase" + criteria;
        clonedBasedRatingTR.children[0].innerHTML = allCriterias[criteria];
        clonedBasedRatingTR.children[1].innerHTML = course.AverageCriteriaRatings.AverageRatingsList[criteria];
        clonedBasedRatingTR.style.display = 'block';
        $('#courseInfoTable tr:last').before(clonedBasedRatingTR);
    }
}

function showCourseComments() {
    $('#allComments').empty();
    if (filteredComments == null || filteredComments.length == 0) {
        var noCommentsErrorLabel = document.createElement("Label");
        noCommentsErrorLabel.innerHTML = "אין תגובות להציג";
        noCommentsErrorLabel.className = "NoComments";
        $('#allComments').append(noCommentsErrorLabel);
    }
    for (comment in filteredComments) {
        printComment(filteredComments[comment], comment);
    }
}

function printComment(comment, itr) {
    var allComments = document.getElementById("allComments");
    var commentView = document.getElementById("commentTable").cloneNode(true);
    commentView.style.display = 'block';
    commentView.id = "commentView" + itr;
    commentView.rows[0].cells[1].children[0].innerHTML = comment.CommentText;
    for (rating in comment.CriteriaRatings) {
        var loadedComment = comment.CriteriaRatings[rating];
        var clonedCommentCriteriaTR = commentView.rows[parseInt(rating) + 1];
        setSelectedRadionButtonValue(clonedCommentCriteriaTR.children[1].children[0], loadedComment.Rating);
    }
    var likesCell = commentView.rows[7].children[1];
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
    var commentDate = commentView.rows[8].children[1];
    commentDate.innerHTML = comment.DateTime.replace("T", " ");
    allComments.appendChild(commentView);
}

function showNewCommentAction() {
    document.getElementById("newCommentTable").style.display = "block";
    document.getElementById("showNewCommentButtonTR").style.display = "none";
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
    var one = $('input[name=star]:checked', '#ratingsForm1').val();
    var two = $('input[name=star2]:checked', '#ratingsForm2').val();
    var three = $('input[name=star3]:checked', '#ratingsForm3').val();
    var four = $('input[name=star4]:checked', '#ratingsForm4').val();
    var five = $('input[name=star5]:checked', '#ratingsForm5').val();
    var six = $('input[name=star6]:checked', '#ratingsForm6').val();
    var ratings = [one, two, three, four, five, six];
    for (rate in ratings) {
        if (ratings[rate] == undefined) {
            alert("עליך לדרג את כל הקריטריונים");
            return;
        }
    }
    var comment = {
        Id: course.Id,
        teacherId: $('#allCourseTeachers').val(),
        Ratings: ratings,
        Comment: $('#CourseNewCommetBox').val(),
        semester: $('courseSemesters').val(),
        Year: $('#Year').val(),
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

function ChangePage() {
    window.location = "../UploadFile/UploadFile.html?University=" + currentUniversity + "&courseId=" + course.Id;
}

function ChangePage2() {
    window.location = "../GetAllSyllabus/GetAllSyllabus.html?University=" + currentUniversity + "&courseId=" + course.Id;
}

function setCourseCommentsWithFilters() {
    var year = $('#filteredByYear').val();
    year = (year != "") ? year : "-1";
    var commentRequest = {
        CourseId: course.Id,
        TeacherId: $('#filterByTeacher').val(),
        Year: year,
        Semester: $('#filteredBySemester').val(),
        SortNew: $("#filteredByNew").is(':checked'),
    };
    var request = $.ajax({
        type: "POST",
        data: JSON.stringify(commentRequest),
        url: '/api/Courses/GetCommentsForCourse',
        contentType: "application/json",
        success: function (data) {
            filteredComments = data;
            showCourseComments();
        },
        fail: function (jqXhr, textStatus) {
            filteredComments = null;
            showCourseComments();
        },
        async: false
    });

}

function allTeachers() {
    var succeed = false;
    var request = $.ajax({
        type: "GET",
        url: '/api/Teachers/GetAllTeacherNamesAndIds',
        contentType: "application/json",
        success: function (data) {
            allCourseTeachers = data;
            succeed = true;
        },
        fail: function (data) {
            succeed = false;
        },
        async: false
    });
    return succeed;
}