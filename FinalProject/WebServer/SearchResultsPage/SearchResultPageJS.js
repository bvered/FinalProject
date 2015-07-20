$(document).ready(function ()
{
    $('body').scroll(function () {
        $('#content').animate({ top: $(this).scrollTop() });
    });
    ShowResults();

});

var coursesArrayResult = [];
var teachersArrayResult = [];

function ShowResults() {

    var arrayResult = [];
    var isTeacher;

    var query_string = new Object();
    getQuertyString(query_string);
    
    createSearchText(query_string)

    if (query_string["search"] === "הכל") {
        getAllData(query_string);
    }
    else if (query_string["search"] === "קורסים") {
        getCourseData(query_string, arrayResult);
    } else if (query_string["search"] === "מרצים") {
        getTeacherData(query_string);
    }
}

function createSearchText(query_string) {
    var searchText = query_string["SearchText"];
    var res = searchText.split("+");
    var searchTextToReturn = new String();
    for (i in res) {
        searchTextToReturn = searchTextToReturn + res[i];
        
        if (res.length -1 != i) {
            searchTextToReturn = searchTextToReturn + ' ';
        }
    }

    query_string["SearchText"] = searchTextToReturn;
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

//need to fix this func so it will get answer from the comtroller
//right now the json gets 404 not found 
function getTeacherData(query_string) {
    $("#filter")[0].hidden = true;
    $("#filter").css({ "display": "none" });
    var dataToSearch = query_string["SearchText"];
    var uri = '/api/Teachers/GetAllSearchedTeachers/';//+ dataToSearch;
   
    var request = $.ajax({
        type: "GET",
        url: uri,
        contentType: "application/json; charset=UTF-8",
        success: function (data) {
            if (data.length == 0) {
                $("#NoMatches")[0].hidden = false;
            }
            else {
                showTeachersData(data);
            }
            succeed = true;
        },
        fail: function (data) {
         //   succeed = false;
        },
       // async: false
    });

    //var arrayResult = [];
    /*$.getJSON(uri)
    .done(function (data) {
        if (data.length == 0) {
        }
        else {
            showTeachersData(arrayResult);
        }
    });*/
    
}
/*
function getTeacherData(query_string) {
    var dataToSearch = query_string["SearchText"];
    var uri = '/api/Teachers/GetAllSearchedTeachers';
    var arrayResult = [];
    $.getJSON(uri)
    .done(function (data) {
        for (i in data) {
            if (dataToSearch == data[i].Name || data[i].Name.indexOf(dataToSearch) >= 0 || (data[i].Name.toLowerCase()).indexOf(dataToSearch) >= 0 || (data[i].Name.toLowerCase()).indexOf(dataToSearch.toLowerCase()) >= 0) {
                arrayResult.push(data[i]);
            }
        }
        if (arrayResult.length == 0) {
            $("#NoMatches")[0].hidden = false;
        }

         //      if (arrayResult.length == 1) { 
          //גיל .. תשנה איך שאתה רוצה שיקראו למשתנה GUID
            //   window.location = '/AddTeacherComment/AddTeacherComment.html?search=Teachers&SearchText=' + arrayResult[0].Id;
         //    }
        else {
            showTeachersData(arrayResult);
        }
    });
}
*/

function getCourseData(query_string) {
    $("#filter")[0].hidden = true;
    $("#filter").css({"display": "none" });
    var dataToSearch = query_string["SearchText"];
    var uri = '/api/Courses/GetAllSearchedCourses';
    var arrayResult = [];
    $.getJSON(uri)
    .done(function (data) {
        for (i in data) {
            if (dataToSearch == data[i].Name || data[i].Name.indexOf(dataToSearch) >= 0 || (data[i].Name.toLowerCase()).indexOf(dataToSearch) >= 0 || (data[i].Name.toLowerCase()).indexOf(dataToSearch.toLowerCase()) >= 0) {
                arrayResult.push(data[i]);
            }
        }
        if (arrayResult.length == 0) {
            $("#NoMatches")[0].hidden = false;
        }
        else {
            showCoursesData(arrayResult);
        }
    });
}

function getAllData(query_string) {
    var dataToSearch = query_string["SearchText"];
    var uri = '/api/Courses/GetAllSearchedCourses';
    var arrayResult = [];
    $.getJSON(uri)
    .done(function (data) {
        for (i in data) { //search for courses
            if (dataToSearch == data[i].Name || data[i].Name.indexOf(dataToSearch) >= 0 || (data[i].Name.toLowerCase()).indexOf(dataToSearch) >= 0 || (data[i].Name.toLowerCase()).indexOf(dataToSearch.toLowerCase()) >= 0) {
                coursesArrayResult.push(data[i]);
            }
        }

        uri = '/api/Teachers/GetAllSearchedTeachers';
        $.getJSON(uri)
        .done(function (data) {
            for (i in data) { //search for teachers
                if (dataToSearch == data[i].Name || data[i].Name.indexOf(dataToSearch) >= 0 || (data[i].Name.toLowerCase()).indexOf(dataToSearch) >= 0 || (data[i].Name.toLowerCase()).indexOf(dataToSearch.toLowerCase()) >= 0) {
                    teachersArrayResult.push(data[i]);
                }
            }
            $("#filter").css({ "display": "block" });

           if(coursesArrayResult.length > 0 && teachersArrayResult.length > 0){ //we found matches in corses and teachers
               $("#filter").show();
               showTeachersData(teachersArrayResult);
               showCoursesData(coursesArrayResult);
            }
            else if(coursesArrayResult.length == 0 && teachersArrayResult.length > 0){ // we found several matches in teachers
                $("#filter").show();
                showTeachersData(teachersArrayResult);                
            }
            else if(coursesArrayResult.length > 0 && teachersArrayResult.length == 0){ // we found several matches in courses
                $("#filter").show();
                showCoursesData(coursesArrayResult);
            }
            else if(coursesArrayResult.length == 0 && teachersArrayResult.length == 0)
            {// we didnt found any match
                $("#filter").hide();
                $("#NoMatches")[0].hidden = false;
                
            }
        });       
    });
}

function filterByParameter() {
    var filterBy = document.getElementById("DropdownFilter").value;
    if (filterBy == "All") {
        showAllWithoutFilter(coursesArrayResult, teachersArrayResult);
    } else if (filterBy == "Teachers") {
        filterByTeachers(teachersArrayResult);
    } else if (filterBy == "Courses") {
        filterByCourses(coursesArrayResult);
    }
}

function showAllWithoutFilter(coursesArrayResult, teachersArrayResult) {
    //clear all the results
    clearResults();
    if (teachersArrayResult.length != 0) {
        showTeachersData(teachersArrayResult);
    }
    if (coursesArrayResult.length != 0) {
        showCoursesData(coursesArrayResult);
    }
}

function filterByTeachers(arrayResult) {
    //clear all the results
    clearResults();
    showTeachersData(arrayResult);
}

function filterByCourses(arrayResult) {
    //clear all the results
    clearResults();
    if (arrayResult.length != 0) {
        showCoursesData(arrayResult);
    }
}

function clearResults() {
    var myNode = document.getElementById("content");
    while (myNode.firstChild) {
        myNode.removeChild(myNode.firstChild);
    }
}

function showTeachersData(arrayResult) {
    var uri = '/api/Teachers/GetAllSearchedTeachers';
    var newLine = '<br>';
    $("#resultAdd").attr("href", "/AddTeacher/AddTeacher.html"); 
    $("#resultAdd").text('לא מצאת את המרצה המבוקש? לחץ כאן להוספה');
    $("#searchTitle")[0].hidden = false;

    for (i in arrayResult) {
        var teacherData = document.createElement('div');
        //courseData.id = 'teacherData';
        teacherData.className = 'teacherData';
                    

                    //adding teacher image
                    var img = new Image();
                    img.id = 'Img';
                    img.src = 'teacherImg.jpg';
                    teacherData.appendChild(img);

                   // new line
                    teacherData.appendChild(document.createElement("br"));

                    //the teacher name
                    var a = document.createElement('a');
                    var linkText = document.createTextNode(arrayResult[i].Name);
                    a.title = arrayResult[i].Name;
                    a.href = "/AddTeacherComment/AddTeacherComment.html?id=" + arrayResult[i].Id;
                    a.appendChild(linkText);
                    teacherData.appendChild(a);

                    //new line
                    teacherData.appendChild(document.createElement("br"));

                    var Score = document.createTextNode(arrayResult[i].Score);
                    teacherData.appendChild(Score);

                    //new line
                    teacherData.appendChild(document.createElement("br"));

                    //adding the courses
                    for (l in arrayResult[i].Courses) {
                        if (l == arrayResult[i].Courses.length - 1) {
                            var Course = document.createTextNode('.' + arrayResult[i].Courses[l]);
                        }
                        else {
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
        //courseData.id = 'courseData';
        courseData.className = 'courseData';

        //adding course image
        var img = new Image();
        img.id = 'Img';
        img.src = 'courseImg.jpg';
        courseData.appendChild(img);

        //new line
        courseData.appendChild(document.createElement("br"));
        //the course name
        var a = document.createElement('a');
        var linkText = document.createTextNode(arrayResult[i].Name);
        a.title = arrayResult[i].Name;
        a.href = "/AddCourseComment/AddCourseComment.html?id=" + arrayResult[i].Id;
        a.appendChild(linkText);
        courseData.appendChild(a);

        //new line
        courseData.appendChild(document.createElement("br"));

        var Score = document.createTextNode(arrayResult[i].Score);
        courseData.appendChild(Score);

        //new line
        courseData.appendChild(document.createElement("br"));

        //is mandatory
        var isMandatory = arrayResult[i].IsMandatory;
        var mandatory;
        if (isMandatory == false) {
            mandatory = document.createTextNode('קורס בחירה');
            courseData.appendChild(mandatory);
        }
        else {
            mandatory = document.createTextNode('קורס חובה');
            courseData.appendChild(mandatory);
        }

        //new line
        courseData.appendChild(document.createElement("br"));

        //the course year
        var year = document.createTextNode(arrayResult[i].Year);
        courseData.appendChild(year);

        //new line
       courseData.appendChild(document.createElement("br"));


        //adding the faculty
        var Faculty = document.createTextNode(arrayResult[i].Faculty);
        courseData.appendChild(Faculty);

        //new line
        courseData.appendChild(document.createElement("br"));

        $("#content").append(courseData);
        $("#content").append(newLine);
    }


}