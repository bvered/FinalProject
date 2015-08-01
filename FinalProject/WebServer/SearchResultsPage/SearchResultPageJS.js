$(document).ready(function() {
    $('body').scroll(function() {
        $('#content').animate({ top: $(this).scrollTop() });
    });
    ShowResults();

});

var coursesArrayResult = [];
var teachersArrayResult = [];

function ShowResults() {
    var query_string = new Object();
    getQuertyString(query_string);
    createSearchText(query_string);

    initFilterValues(query_string);

    if (query_string["search"] === "All") {
        getAllData();
    } else if (query_string["search"] === "Courses") {
        getCourseData();
    } else if (query_string["search"] === "Teachers") {
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

function getQuertyString(query_string) {
    var query = window.location.search.substring(1);
    query = decodeURI(query);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        // If first entry with this name
        if (typeof query_string[pair[0]] === "undefined") {
            query_string[pair[0]] = pair[1];
            // If second entry with this name
        } else if (typeof query_string[pair[0]] === "string") {
            var arr = [query_string[pair[0]], pair[1]];
            query_string[pair[0]] = arr;
            // If third or later entry with this name
        } else {
            query_string[pair[0]].push(pair[1]);
        }
    }
}

function getTeacherData() {
    var uri = '/api/Teachers/GetAllSearchedTeachers';
    var searchCourse = {
        SearchText: $('#searchText').attr('value'),
        counter: $('#page').attr('value') - 1,
}
    var request = $.ajax({
        type: "POST",
        url: uri,
        data: JSON.stringify(searchCourse),
        contentType: "application/json", //; charset=UTF-8",
        success: function(data) {
            if (data.Results.length == 0) {
                $("#NoMatches")[0].hidden = false;
                $("#footer").hide();
            } else {
                createPaging(data.TotalCount);
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
        SearchPreferences: $.jStorage.get("SearchPreferences")
    };

    var request = $.ajax({
        type: "POST",
        url: uri,
        data: JSON.stringify(searchCourse),
        contentType: "application/json", //; charset=UTF-8",

        success: function (data) {
            $.jStorage.set("SearchPreferences", data.SearchPreferences);
            if (data.AllResults.length == 0) {
                $("#NoMatches")[0].hidden = false;
                $("#footer").hide();
            } else {
                $("#footer").show();
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
    var searchText = $('#searchText').attr('value');
    var uri = '/api/SmartSearch/GetCourseByFilter';

    var searchCourse = {
        SearchText: searchText,
        Faculty: $('input[name=faculty]:checked').val(),
        IsMandatory: $('input[name=mandatory]:checked').val(),
        AcademicDegree: $('input[name=degree]:checked').val(),
        IntendedYear: $('input[name=year]:checked').val(),
        SearchPreferences: $.jStorage.get("SearchPreferences")
    };
    var request = $.ajax({
        type: "POST",
        url: uri,
        data: JSON.stringify(searchCourse),
        contentType: "application/json", //; charset=UTF-8",
        success: function (coursesArrayResult) {
            $.jStorage.set("SearchPreferences", coursesArrayResult.SearchPreferences);
            var uri2 = '/api/Teachers/GetAllSearchedTeachers';
            var request2 = $.ajax({
                type: "POST",
                url: uri2,
                data: JSON.stringify(searchText),
                contentType: "application/json", //; charset=UTF-8",
                success: function(teachersArrayResult) {
                    if (coursesArrayResult.AllResults.length > 0 && teachersArrayResult.length > 0) { //we found matches in corses and teachers
                        $("#footer").show();

                        showTeachersData(teachersArrayResult);
                        showCoursesData(coursesArrayResult.AllResults);
                    } else if (coursesArrayResult.AllResults.length == 0 && teachersArrayResult.length > 0) { // we found several matches in teachers
                        $("#footer").show();
                        showTeachersData(teachersArrayResult);
                    } else if (coursesArrayResult.AllResults.length > 0 && teachersArrayResult.length == 0) { // we found several matches in courses
                        $("#footer").show();
                        showCoursesData(coursesArrayResult.AllResults);
                    } else if (coursesArrayResult.AllResults.length == 0 && teachersArrayResult.length == 0) { // we didnt found any match
                        $("#NoMatches")[0].hidden = false;
                        $("#footer").hide();
                    }
                    succeed = true;
                },
                fail: function(data) {
                    //   succeed = false;
                },
                // async: false
            });
            succeed = true;
        },
        fail: function(coursesArrayResult) {
            //   succeed = false;
        },
        // async: false
    });
}

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
}

function showTeachersData(arrayResult) {
    var newLine = '<br>';
    $("#resultAdd").attr("href", "/AddTeacher/AddTeacher.html");
    $("#resultAdd").text('לא מצאת את המרצה המבוקש? לחץ כאן להוספה');
    $("#searchTitle")[0].hidden = false;

    $("#page").attr("value", arrayResult.length / 5 + 1);

    for ( i=0; i<arrayResult.length; i++) {
        var teacherData = document.createElement('div');
        //courseData.id = 'teacherData';
        teacherData.className = 'teacherData';


        //adding teacher image
        var img = new Image();
        img.id = 'Img';
        img.src = 'teacherImg.jpg';
        teacherData.appendChild(img);

        //the teacher name
        var a = document.createElement('a');
        var linkText = document.createTextNode(arrayResult[i].Name);
        a.title = arrayResult[i].Name;
        a.href = "/AddTeacherComment/AddTeacherComment.html?id=" + arrayResult[i].Id;
        a.appendChild(linkText);
        teacherData.appendChild(a);

        //new line
        teacherData.appendChild(document.createElement("br"));

        var Score = document.createTextNode('ניקוד: ' + arrayResult[i].Score);
        teacherData.appendChild(Score);

        //new line
        teacherData.appendChild(document.createElement("br"));

        //adding the courses
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

        //the course name
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

function createPaging(ResultsCounter) {
    var PreviousPage = document.createElement('li');
    var a = document.createElement('a');
    a.href = '#';
    var innerSpan = document.createElement('span');
    innerSpan.text = '&raquo';
    $('#pages').appendChild(PreviousPage.appendChild(a.appendChild(innerSpan)));

    for (var i = 0; i < ResultsCounter; i++) {
        var newPage = document.createElement('li');
        a = document.createElement('a');
        a.href = '#';
        innerSpan = document.createElement('span');
        innerSpan.text = i;

        if (i == 1) {
            newPage.className = 'active';
        }
        $('#pages').appendChild(newPage.appendChild(a.appendChild(innerSpan)));
    }

    var NextPage = document.createElement('li');
    a = document.createElement('a');
    a.href = '#';
    innerSpan = document.createElement('span');
    innerSpan.text = '&laquo;';
    $('#pages').appendChild(NextPage.appendChild(a.appendChild(innerSpan)));
}