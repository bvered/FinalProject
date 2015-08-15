var queryString;
var currentUniversity;

$(document).ready(function() {
    $('body').scroll(function() {
        $('#content').animate({ top: $(this).scrollTop() });
    });

    queryString = getQuertyString();
    $('#University').attr('value', queryString["University"]);
    currentUniversity = queryString["University"];
    getBackground(currentUniversity);

    ShowResults();
});

var coursesArrayResult = [];
var teachersArrayResult = [];
var maxPages;

function ShowResults() {
    createSearchText(queryString);

    initFilterValues(queryString);
    $('#page').attr('value', 1);

    if (queryString["search"] === "All") {
        $('#FilterForCourses')[0].hidden = false;
        getAllData();
    } else if (queryString["search"] === "Courses") {
        $('#FilterForCourses')[0].hidden = false;
        getCourseData();
    } else if (queryString["search"] === "Teachers") {
        $('#FilterForCourses')[0].hidden = true;
        getTeacherData();
    }
}

function initFilterValues(query_string) {
    var typeSelect = $('input:radio[name=type]');
    if (typeSelect.is(':checked') === false) {
        var valueString = '[value=' + query_string["search"] + ']';
        typeSelect.filter(valueString).prop('checked', true);
    }

    var DegreeSelect = $('input:radio[name=degree]');
    if (DegreeSelect.is(':checked') === false) {
        valueString = '[value=' + query_string["degree"] + ']';
        DegreeSelect.filter(valueString).prop('checked', true);
    }

    var FacultySelect = $('input:radio[name=faculty]');
    if (FacultySelect.is(':checked') === false) {
        valueString = '[value=' + query_string["faculty"] + ']';
        FacultySelect.filter(valueString).prop('checked', true);
    }

    var MandatorySelect = $('input:radio[name=mandatory]');
    if (MandatorySelect.is(':checked') === false) {
        valueString = '[value=' + query_string["mandatory"] + ']';
        MandatorySelect.filter(valueString).prop('checked', true);
    }

    var YearSelect = $('input:radio[name=year]');
    if (YearSelect.is(':checked') === false) {
        valueString = '[value=' + query_string["year"] + ']';
        YearSelect.filter(valueString).prop('checked', true);
    }
}

function createSearchText(query_string) {
    var searchText = query_string["SearchText"];
    var res = searchText.split("+");
    var searchTextToReturn = new String();
    for (i in res) {
        searchTextToReturn = searchTextToReturn + res[i];

        if (res.length - 1 != i) {
            searchTextToReturn = searchTextToReturn + ' ';
        }
    }

    query_string["SearchText"] = searchTextToReturn;
    $("#searchText").attr('value', query_string["SearchText"]);
}

function getTeacherData() {
    var uri = '/api/SmartSearch/GetAllSearchedTeachers';

    var searchCourse = {
        University: currentUniversity,
        Name: $('#searchText').attr('value'),
        counter: $('#page').attr('value') - 1,
    };
    if (queryString["isTop"] === "true") {
        searchCourse.isTop = true;
    }
    var request = $.ajax({
        type: "POST",
        url: uri,
        data: JSON.stringify(searchCourse),
        contentType: "application/json", 
        success: function(data) {
            if (data.Results.length == 0) {
                $("#NoMatches")[0].hidden = false;
                $("#footer").hide();
            } else {
                maxPages = Math.ceil(data.TotalCount / 5);
                createPaging(maxPages);
                $("#footer").show();
                showTeachersData(data.Results);
            }
            succeed = true;
        },
        fail: function(data) {
        },
    });
}

function getCourseData() {
    var uri = '/api/SmartSearch/GetCourseByFilter';

    var searchCourse = {
        University: currentUniversity,
        SearchText: $('#searchText').attr('value'),
        Faculty: $('input[name=faculty]:checked').val(),
        IsMandatory: $('input[name=mandatory]:checked').val(),
        AcademicDegree: $('input[name=degree]:checked').val(),
        IntendedYear: $('input[name=year]:checked').val(),
        SearchPreferences: $.jStorage.get("SearchPreferences"),
        counter: $('#page').attr('value') - 1
    };
    if (queryString["isTop"] === "true") {
        searchCourse.isTop = true;
    }

    var request = $.ajax({
        type: "POST",
        url: uri,
        data: JSON.stringify(searchCourse),
        contentType: "application/json",

        success: function (data) {
            $.jStorage.set("SearchPreferences", data.SearchPreferences);
            if (data.AllResults.length == 0) {
                $("#NoMatches")[0].hidden = false;
                $("#footer").hide();
            } else {
                $("#footer").show();
                maxPages = Math.ceil(data.TotalCount / 5);
                createPaging(maxPages);
                showCoursesData(data.AllResults);
            }
            succeed = true;
        },
        fail: function(data) {
        }
    });
}

function getAllData() {
    var uri = '/api/SmartSearch/GetAnyResults';

    var searchQuery = {
        University: currentUniversity,
        SearchText: $('#searchText').attr('value'),
        Faculty: $('input[name=faculty]:checked').val(),
        IsMandatory: $('input[name=mandatory]:checked').val(),
        AcademicDegree: $('input[name=degree]:checked').val(),
        IntendedYear: $('input[name=year]:checked').val(),
        SearchPreferences: $.jStorage.get("SearchPreferences"),
        counter: $('#page').attr('value') - 1
    };
    var request = $.ajax({
        type: "POST",
        url: uri,
        data: JSON.stringify(searchQuery),
        contentType: "application/json",
        success: function (searchResult) {
            $.jStorage.set("SearchPreferences", searchResult.SearchPreferences);

            if (searchResult.TotalCount == 0) { 
                $("#NoMatches")[0].hidden = false;
                $("#footer").hide();
            } else {
                $("#footer").show();
                maxPages = Math.ceil(searchResult.TotalCount / 5);
                createPaging(maxPages);
                showTeachersData(searchResult.TeacherResults);
                showCoursesData(searchResult.CourseResults);
            }
        },
        fail: function(data) {
        }
    });
};

function ChangeResults() {
    clearResults();

    var filterType = $('input[name=type]:checked').val();
    if (filterType == "All") {
        $('#FilterForCourses')[0].hidden = false;
        getAllData();
    } else if (filterType == "Teachers") {
        $('#FilterForCourses')[0].hidden = true;
        getTeacherData();
    } else if (filterType == "Courses") {
        $('#FilterForCourses')[0].hidden = false;
        getCourseData();
    }
}

function clearResults() {
    var myNode = document.getElementById("content");
    while (myNode.firstChild) {
        myNode.removeChild(myNode.firstChild);
    }

    var pages = document.getElementById("pages");
    while (pages.firstChild) {
        pages.removeChild(pages.firstChild);
    }
}

function showTeachersData(arrayResult) {
    var newLine = '<br>';
    $("#resultAdd").attr("href", "/AddTeacher/AddTeacher.html?University="+ currentUniversity);
    $("#resultAdd").text('לא מצאת את המרצה המבוקש? לחץ כאן להוספה');
    $("#searchTitle")[0].hidden = false;

    for (var i=0; i<arrayResult.length; i++) {
        var teacherData = document.createElement('div');
        teacherData.className = 'teacherData';

        var img = new Image();
        img.id = 'Img';
        img.src = 'teacherImg.jpg';
        teacherData.appendChild(img);

        var linkText = document.createTextNode(arrayResult[i].Name);
        linkText.className = 'teacherName';
        teacherData.id = arrayResult[i].Id
        teacherData.onclick = GoToTeacher;
        teacherData.appendChild(linkText);

        teacherData.appendChild(document.createElement("br"));

        var Score = document.createTextNode('כמות דירוגים: ' + arrayResult[i].Score);
        teacherData.appendChild(Score);

        teacherData.appendChild(document.createElement("br"));

        var Teach = document.createTextNode('קורסים: ');
        teacherData.appendChild(Teach);
        for (l in arrayResult[i].Courses) {

            if (l == arrayResult[i].Courses.length - 1) {
                var Course = document.createTextNode('.' + arrayResult[i].Courses[l]);
            } else {
                var Course = document.createTextNode(arrayResult[i].Courses[l] + ', ');
            }
            teacherData.appendChild(Course);
        }
        $("#content").append(teacherData);
        $("#content").append(newLine);
    }
}

function GoToTeacher() {
    window.location = "/AddTeacherComment/AddTeacherComment.html?University="+ currentUniversity+"&id=" + this.id;
}

function showCoursesData(arrayResult) {
    var newLine = '<br>';
    $("#resultAdd").attr("href", "/AddCourse/AddCourse.html?University="+ currentUniversity);
    $("#resultAdd").text('לא מצאת את הקורס המבוקש? לחץ כאן להוספה');
    $("#searchTitle")[0].hidden = false;

    for (i in arrayResult) {
        var courseData = document.createElement('div');
        courseData.className = 'courseData';

        var img = new Image();
        img.id = 'Img';
        img.src = 'courseImg.jpg';
        courseData.appendChild(img);

        var linkText = document.createTextNode(arrayResult[i].Name);
        linkText.className = 'teacherName';
        courseData.id = arrayResult[i].Id;
        courseData.onclick = GoToCourse;
      
        courseData.appendChild(linkText);

        courseData.appendChild(document.createElement("br"));

        var Score = document.createTextNode('כמות דירוגים: ' + arrayResult[i].Score);
        courseData.appendChild(Score);

        courseData.appendChild(document.createElement("br"));

        var isMandatory = arrayResult[i].IsMandatory;
        var mandatory;
        if (isMandatory == false) {
            mandatory = document.createTextNode('מסגרת: קורס בחירה');
            courseData.appendChild(mandatory);
        } else {
            mandatory = document.createTextNode('מסגרת: קורס חובה');
            courseData.appendChild(mandatory);
        }
        courseData.appendChild(document.createElement("br"));

        var year = document.createTextNode('שנה: ' + arrayResult[i].Year);
        courseData.appendChild(year);

        courseData.appendChild(document.createElement("br"));

        var faculty = document.createTextNode('פקולטה: ' + arrayResult[i].Faculty);
        courseData.appendChild(faculty);

        courseData.appendChild(document.createElement("br"));

        $("#content").append(courseData);
        $("#content").append(newLine);
    }
}

function GoToCourse() {
    window.location = "/AddCourseComment/AddCourseComment.html?University="+ currentUniversity + "&id=" + this.id;
}

function createPaging(resultsCounter) {
    var previousPage = document.createElement('li');
    previousPage.className = "pagebutton";
    previousPage.onclick = function () { changePage(parseInt($('#page').attr('value')) - 1); };
    var innerSpan = document.createElement('span');
    innerSpan.innerText = '»';

    previousPage.appendChild(innerSpan);
    $('#pages')[0].appendChild(previousPage);

    var currentPage = parseInt($('#page').attr('value'));

    var firstPage = Math.max(1, Math.min(resultsCounter - 10, currentPage - 5));
    var lastpage = Math.min(Math.max(currentPage + 5, 11), resultsCounter);
    
    for (var i = firstPage; i <= lastpage; i++) {
        var newPage = document.createElement('li');
        newPage.className = "pagebutton";
        newPage.id = i;
        
        innerSpan = document.createElement('span');
        innerSpan.innerText = i;

        newPage.onclick = GoToPage;

        if (i == currentPage) {
            newPage.className ='active';
        }

        newPage.appendChild(innerSpan);
        $('#pages')[0].appendChild(newPage);
    }

    var nextPage = document.createElement('li');
    nextPage.className = "pagebutton";

    nextPage.onclick = function () { changePage(parseInt($('#page').attr('value')) + 1); };
    innerSpan = document.createElement('span');
    innerSpan.innerText = '«';

    nextPage.appendChild(innerSpan);
    $('#pages')[0].appendChild(nextPage);
}

function GoToPage() {
    changePage(this.id);
}

function changePage(showPage) {
    if (showPage < 1) {
        $('#page').attr('value', 1);
    } else if (showPage > maxPages) {
        $('#page').attr('value', maxPages);
    } else {
        $('#page').attr('value', showPage);
        $(".active").addClass('pagebutton').removeClass('active');
        $('#' + showPage).removeClass('pagebutton').addClass('active');
        ChangeResults();
    }
}

function homePage() {
    window.location = "../HomePage/HomePage.html?University=" + currentUniversity;
}