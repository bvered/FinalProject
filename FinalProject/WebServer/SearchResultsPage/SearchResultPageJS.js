$(document).ready(function () { ShowResults() });

function ShowResults() {

    var arrayResult = [];
    var isTeacher;

    var query_string = new Object();
    getQuertyString(query_string);

    if (query_string["search"] === "All") {
        getAllData(query_string);
    }
    else if (query_string["search"] === "Courses") {
        getCourseData(query_string, arrayResult);
    } else if (query_string["search"] === "Teachers") {
        getTeacherData(query_string);
    }
}

function getQuertyString(query_string) {
    var query = window.location.search.substring(1);
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
            var textError = document.createTextNode('No matches found');
            $('body').append(textError);
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

function getCourseData(query_string) {
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
            var textError = document.createTextNode('No matches found');
            $('body').append(textError);
        }
            /* if (arrayResult.length == 1) {

            // גיללל המערך של התוצאות מכיל כרגע רק שמות, לא יישות של קורס.. צריך להביא מהשרת את הגויד לפי שם הקורס
                  window.location = '/AddCourseComment/AddCourseComment.html?search=Courses&SearchText=' + arrayResult[0].Id;

             }*/
        else {
            showCoursesData(arrayResult);
        }
    });
}

function getAllData(query_string) {
    var coursesArrayResult = [];
    var teachersArrayResult = [];
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

            if (coursesArrayResult.length == 1 && teachersArrayResult.length == 0) { //we found only one match of course
               //גיל
                   window.location = '/AddCourseComment/AddCourseComment.html?search=Courses&SearchText=' + coursesArrayResult[0].Id;
            }
            else if(coursesArrayResult.length == 0 && teachersArrayResult.length == 1)
            { //we found only one match of course
                //גיל
                   window.location = '/AddTeacherComment/AddTeacherComment.html?search=Courses&SearchText=' + teachersArrayResult[0].Id;
            }
            else if(coursesArrayResult.length > 0 && teachersArrayResult.length > 0){ //we found matches in corses and teachers
                filterByParameter(coursesArrayResult, teachersArrayResult);
                showCoursesData(coursesArrayResult);
                showTeachersData(teachersArrayResult);
            }
            else if(coursesArrayResult.length == 0 && teachersArrayResult.length > 1){ // we found several matches in teachers
                filterByParameter(coursesArrayResult, teachersArrayResult);
                showTeachersData(teachersArrayResult);                
            }
            else if(coursesArrayResult.length > 1 && teachersArrayResult.length == 0){ // we found several matches in courses
                filterByParameter(coursesArrayResult, teachersArrayResult);
                showCoursesData(coursesArrayResult);
            }
            else if(coursesArrayResult.length == 0 && teachersArrayResult.length == 0)
            {// we didnt found any match
                var textError = document.createTextNode('No matches found');
                $('body').append(textError);
            }
        });       
    });
}

function filterByParameter(coursesArrayResult, teachersArrayResult) {
    var teachersFilterBtn = document.createElement("BUTTON");
    var teacherText = document.createTextNode("filter by teachers");       // Create a text node
    teachersFilterBtn.appendChild(teacherText);
    teachersFilterBtn.onclick = function () { filterByTeachers(teachersArrayResult); };

    var coursesFilterBtn = document.createElement("BUTTON");
    var courseText = document.createTextNode("filter by courses");       // Create a text node
    coursesFilterBtn.appendChild(courseText);
    coursesFilterBtn.onclick = function () { filterByCourses(coursesArrayResult); };

    var noFilter = document.createElement("BUTTON");
    var noFilterText = document.createTextNode("show all results");       // Create a text node
    noFilter.appendChild(noFilterText);
    noFilter.onclick = function () { showAllWithoutFilter(coursesArrayResult, teachersArrayResult); };

    $("#filter").append(teachersFilterBtn);
    $("#filter").append(coursesFilterBtn);
    $("#filter").append(noFilter);
}

function showAllWithoutFilter(coursesArrayResult, teachersArrayResult) {
    //clear all the results
    clearResults();
    showTeachersData(teachersArrayResult);
    showCoursesData(coursesArrayResult);
}

function filterByTeachers(arrayResult) {
    //clear all the results
    clearResults();
    showTeachersData(arrayResult);
}

function filterByCourses(arrayResult) {
    //clear all the results
    clearResults();
    showCoursesData(arrayResult);
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
    $("#resultAdd").text('Cant find the requested teacher? CLICK HERE to add');
    $("#searchTitle")[0].hidden = false;

    for (i in arrayResult) {
                    var teacherData = $('<p />');
                    teacherData.addClass("lecturerData");

                    //adding teacher image
                    var img = new Image();
                    img.id = 'Img';
                    img.src = 'teacherImg.jpg';
                    teacherData.append(img);

                    //new line
                //    teacherData.append(newLine);

                    //the teacher name
                    var a = $('<a />');
                    ///////גיללל תשנה כאן איך שאתה רוצה לקרוא למשתנה שמעביר את הGUID
                    a.attr('href', "/AddTeacherComment/AddTeacherComment.html?search=Teachers&SearchText=" + arrayResult[i].Id);
 
                    a.text(arrayResult[i].Name);
                    teacherData.append(a);

                    //new line
                    teacherData.append(newLine);

                    //adding the universities
                    for (k in arrayResult[i].Universities) {
                        if (k == arrayResult[i].Universities.length - 1) {
                            var University = document.createTextNode(arrayResult[i].Universities[k] + '.');
                        }
                        else {
                            var University = document.createTextNode(arrayResult[i].Universities[k] + ', ');
                        }
                        teacherData.append(University);
                    }

                    //new line
                    teacherData.append(newLine);

                    //////////////לא מצליחה להציג את הקורסים למרות שבטסט הוספתי 2 קורסים למרצה
                    //adding the courses
                    for (l in arrayResult[i].Courses) {
                        if (k == arrayResult[i].Courses.length - 1) {
                            var Course = document.createTextNode(arrayResult[i].Courses[l] + '.');
                        }
                        else {
                            var Course = document.createTextNode(arrayResult[i].Courses[l] + ', ');
                        }
                        teacherData.append(Course);
                    }

        //  $('body').append(teacherData);    
                    $("#content").append(teacherData);
                }
}

function showCoursesData(arrayResult) {
    var uri = '/api/Courses/GetAllSearchedCourses';
    var newLine = '<br>';
    $("#resultAdd").attr("href", "/AddCourse/AddCourse.html");
    $("#resultAdd").text('Cant find the requested course? CLICK HERE to add');
    $("#searchTitle")[0].hidden = false;

    for (i in arrayResult) {
        var courseData = $('<p />');
        courseData.addClass("lecturerData");

        //adding course image
        var img = new Image();
        img.id = 'Img';
        img.src = 'courseImg.jpg';
        courseData.append(img);

        //new line
        courseData.append(newLine);

        //the course name
        var a = $('<a />');
        /////גיללל תשנה כאן איך שאתה רוצה לקרוא למשתנה שמעביר את הGUID
        a.attr('href', "/AddTeacherComment/AddTeacherComment.html?search=Courses&SearchText=" + arrayResult[i].Id);
        a.text(arrayResult[i].Name);
        courseData.append(a);

        //new line
        courseData.append(newLine);

        //adding the faculties
        var Faculty = document.createTextNode(arrayResult[i].Faculty);
        courseData.append(Faculty);

        //new line
        courseData.append(newLine);

        //adding the university
        var University = document.createTextNode(arrayResult[i].University);
        courseData.append(University);

        //   $('body').append(courseData);
        $("#content").append(courseData);
    }
}