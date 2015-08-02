$(document).ready(function() {
    $('body').scroll(function() {
        $('#content').animate({ top: $(this).scrollTop() });
    });
    ShowResults();
});

var coursesArrayResult = [];
var teachersArrayResult = [];
var maxPages;

function ShowResults() {

    var queryString = getQuertyString();
    createSearchText(queryString);

    initFilterValues(queryString);
    $('#page').attr('value', 1);

    if (queryString["search"] === "All") {
        getAllData();
    } else if (queryString["search"] === "Courses") {
        getCourseData();
    } else if (queryString["search"] === "Teachers") {
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
        Name: $('#searchText').attr('value'),
        counter: $('#page').attr('value') - 1,
    };
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
            //   succeed = false;
        },
        // async: false
    });
}

function getCourseData() {
    var uri = '/api/SmartSearch/GetCourseByFilter';

    var searchCourse = {
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
            //   succeed = false;
        },
        // async: false
    });
}

function getAllData() {
    var uri = '/api/SmartSearch/GetAnyResults';

    var searchQuery = {
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
        contentType: "application/json", //; charset=UTF-8",
        success: function (searchResult) {
            $.jStorage.set("SearchPreferences", searchResult.SearchPreferences);

            if (searchResult.TotalCount == 0) { // we didnt found any match
                $("#NoMatches")[0].hidden = false;
                $("#footer").hide();
            } else {
                maxPages = Math.ceil(searchResult.TotalCount / 5);

                createPaging(maxPages);
                showTeachersData(searchResult.TeacherResults);
                showCoursesData(searchResult.CourseResults);
            }
        },
        fail: function(data) {
            //   succeed = false;
        },
        // async: false
    });
};

function ChangeResults() {
    clearResults();

    var filterType = $('input[name=type]:checked').val();
    if (filterType == "All") {
        getAllData();
    } else if (filterType == "Teachers") {
        getTeacherData();
    } else if (filterType == "Courses") {
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
    $("#resultAdd").attr("href", "/AddTeacher/AddTeacher.html");
    $("#resultAdd").text('לא מצאת את המרצה המבוקש? לחץ כאן להוספה');
    $("#searchTitle")[0].hidden = false;

    $("#page").attr("value", Math.round(arrayResult.length / 5));

    for ( i=0; i<arrayResult.length; i++) {
        var teacherData = document.createElement('div');
        teacherData.className = 'teacherData';

        var img = new Image();
        img.id = 'Img';
        img.src = 'teacherImg.jpg';
        teacherData.appendChild(img);

        var a = document.createElement('a');
        var linkText = document.createTextNode(arrayResult[i].Name);
        a.title = arrayResult[i].Name;
        a.href = "/AddTeacherComment/AddTeacherComment.html?id=" + arrayResult[i].Id;
        a.appendChild(linkText);
        teacherData.appendChild(a);

        teacherData.appendChild(document.createElement("br"));

        var Score = document.createTextNode('ניקוד: ' + arrayResult[i].Score);
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

function showCoursesData(arrayResult) {
    var uri = '/api/Courses/GetAllSearchedCourses';
    var newLine = '<br>';
    $("#resultAdd").attr("href", "/AddCourse/AddCourse.html");
    $("#resultAdd").text('לא מצאת את הקורס המבוקש? לחץ כאן להוספה');
    $("#searchTitle")[0].hidden = false;

    for (i in arrayResult) {
        var courseData = document.createElement('div');
        courseData.className = 'courseData';

        var img = new Image();
        img.id = 'Img';
        img.src = 'courseImg.jpg';
        courseData.appendChild(img);

        var a = document.createElement('a');
        var linkText = document.createTextNode(arrayResult[i].Name);
        a.title = arrayResult[i].Name;
        a.href = "/AddCourseComment/AddCourseComment.html?id=" + arrayResult[i].Id;
        a.appendChild(linkText);
        courseData.appendChild(a);

        courseData.appendChild(document.createElement("br"));

        var Score = document.createTextNode('ניקוד: ' + arrayResult[i].Score);
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

function createPaging(resultsCounter) {
    var previousPage = document.createElement('li');
    var a = document.createElement('a');
    a.onclick = function () { changePage(parseInt($('#page').attr('value')) - 1); };
    var innerSpan = document.createElement('span');
    innerSpan.innerText = '»';
    a.appendChild(innerSpan);
    previousPage.appendChild(a);
    $('#pages')[0].appendChild(previousPage);
   
    for (var i = 0; i < resultsCounter; i++) {
        var newPage = document.createElement('li');
        a = document.createElement('a');
        a.onclick = function () { changePage(parseInt(i)+1); };
        innerSpan = document.createElement('span');
        innerSpan.innerText = i+1;

        if (i == 0) {
            newPage.className ='active';
        }
        a.appendChild(innerSpan);
        newPage.appendChild(a);
        $('#pages')[0].appendChild(newPage);
    }

    var nextPage = document.createElement('li');
    a = document.createElement('a');
    a.onclick = function () { changePage(parseInt($('#page').attr('value')) + 1); };
    innerSpan = document.createElement('span');
    innerSpan.innerText = '«';

    a.appendChild(innerSpan);
    nextPage.appendChild(a);
    $('#pages')[0].appendChild(nextPage);
}

function changePage(showPage) {
    if (showPage < 1) {
        $('#page').attr('value', 1);
    } else if (showPage > maxPages) {
        $('#page').attr('value', maxPages);
    } else {
        $('#page').attr('value', showPage);
        ChangeResults();
    }
}