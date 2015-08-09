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


function homePage() {
    window.location = "../HomePage/HomePage.html";
}

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
    showCourseComments(true, null);
}

function setupFilters() {
    var selectTeacher = $('filterByTeacher');
    var teachersToFilterBy = [];
    for (courseInSemster in course.CourseInSemesters) {
        teachersToFilterBy.push(course.CourseInSemesters[courseInSemster].Teacher)
    }
    teachersToFilterBy = teachersToFilterBy.filter(function (itm, i, a) {
        return i == a.indexOf(itm);
    });
    for (teacher in teachersToFilterBy) {
        var teacherOption = document.createElement('option');
        teacherOption.text = teachersToFilterBy[teacher].Name;
        teacherOption.value = teachersToFilterBy[teacher].Id;
        selectTeacher.add(teacherOption);
    }
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

function showCourseComments(sortByNew, teacher) {
    $('allComments').empty();
    var commentsByTeacher;
    if (teacher == null) {
        commentsByTeacher = course.CourseInSemesters;
    } else {
        commentsByTeacher = course.CourseInSemesters[courseSemester].filter(function (courseInSemester) {
            if (courseInSemester.Teacher.Id == teacher) {
                return true;
            } else {
                return false;
            }
        });
    }
    
    for (courseSemester in commentsByTeacher) {
        var semesterComments = course.CourseInSemesters[courseSemester].CourseComments;
        if (sortByNew) {
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
    revealAddCommentViewToUser();
}

function revealAddCommentViewToUser() {
    var courseSemesters = document.getElementById("courseSemesters");
    for (semester in course.CourseInSemesters) {
        var semesterOption = document.createElement("option");
        semesterOption.text = course.CourseInSemesters[semester].Year + " " + semesterNameByEnum(course.CourseInSemesters[semester].Semester);
        semesterOption.value = course.CourseInSemesters[semester].Id
        courseSemesters.add(semesterOption);
    }
    //var newCommentTable = document.getElementById("newCommentTable");
    //var newCommentCriteriaTR = document.getElementById("criteriaTR");
    //var courseSemestersTR = document.getElementById("courseSemestersTR");
    //for (ratingText in allCriterias) {
    //    var clonedCommentCriteriaTR = newCommentCriteriaTR.cloneNode(true);
    //    clonedCommentCriteriaTR.id = "criteriaTR" + ratingText;
    //    clonedCommentCriteriaTR.style.display = "block";
    //    var criteriaTextTD = clonedCommentCriteriaTR.children[0];
    //    criteriaTextTD.id = "criteriaText" + ratingText;
    //    criteriaTextTD.innerHTML = allCriterias[ratingText];
    //    var starRatingTD = clonedCommentCriteriaTR.children[1];
    //    starRatingTD.id = "criteriaRating" + ratingText;
    //    starRatingTD.children[0].id = "starRating" + ratingText;
    //    newCommentTable.appendChild(clonedCommentCriteriaTR);
    //}
    //var courseSemesters = document.getElementById("courseSemesters");
    //var sendTR = document.getElementById("sendTR");
    //sendTR.parentNode.removeChild(sendTR);
    //newCommentTable.appendChild(sendTR);
}

function showLoadingCourseFailed() {
    alert("קורס לא נמצא, הנך מועבר לעמוד הראשי.");
    parent.location = "../HomePage/HomePage.html";
}

/*function addComment() {
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
}*/

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

function ChangePage() {
    window.location = "../UploadSyllabus/UploadSyllabus.html?courseId=" + course.Id;
}

function ChangePage2() {
    window.location = "../GetAllSyllabus/GetAllSyllabus.html?courseId=" + course.Id;
}

$('filterByTeacher').on('change', function () {
    showCourseComments(true, $(this).val());


    //var filterBy = {
    //    courseId: course.Id,
    //    teacherId: $(this).val(),
    //};
    //var request = $.ajax({
    //    type: "POST",
    //    data: JSON.stringify(filterBy),
    //    url: '/api/AddFile/GetCommentsByTeacherName',
    //    contentType: "application/json",
    //    success: function (data) {
    //        course.
    //    },
    //    fail: function (jqXhr, textStatus) {
    //        alert("פעולה נכשלה");
    //    },
    //    async: false
    //});
});