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
    var uri = '/api/Teachers/GetTeachers';
    var arrayResult = [];
    $.getJSON(uri)
    .done(function (data) {
        for (i in data) {
            if (dataToSearch == data[i] || data[i].indexOf(dataToSearch) >= 0 || (data[i].toLowerCase()).indexOf(dataToSearch) >= 0 || (data[i].toLowerCase()).indexOf(dataToSearch.toLowerCase()) >= 0) {
                arrayResult.push(data[i]);
            }
        }
        if (arrayResult.length == 0) {
            var textError = document.createTextNode('No matches found');
            $('body').append(textError);
        }
     //   if (arrayResult.length == 1) { /////במקרה ויש תוצאה אחת צריך לבצע הפניה לדף של גיל
     //   window.location = '/AddTeacherComment/AddTeacherComment.html?search=Teachers&SearchText=' + arrayResult[0].Id;
       // }
        else {
            showTeachersData(arrayResult);
        }
    });
}

function getCourseData(query_string) {
    var dataToSearch = query_string["SearchText"];
    var uri = '/api/Courses/GetCourses';
    var arrayResult = [];
    $.getJSON(uri)
    .done(function (data) {
        for (i in data) {
            if (dataToSearch == data[i] || data[i].indexOf(dataToSearch) >= 0 || (data[i].toLowerCase()).indexOf(dataToSearch) >= 0 || (data[i].toLowerCase()).indexOf(dataToSearch.toLowerCase()) >= 0) {
                arrayResult.push(data[i]);
            }
        }
        if (arrayResult.length == 0) {
            var textError = document.createTextNode('No matches found');
            $('body').append(textError);
        }
            /* if (arrayResult.length == 1) { /////במקרה ויש תוצאה אחת צריך לבצע הפניה לדף של גיל
                  window.location = '/AddTeacherComment/AddTeacherComment.html?search=Courses&SearchText=' + arrayResult[0].Id;

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
    var uri = '/api/Courses/GetCourses';
    var arrayResult = [];
    $.getJSON(uri)
    .done(function (data) {
        for (i in data) { //search for courses
            if (dataToSearch == data[i] || data[i].indexOf(dataToSearch) >= 0 || (data[i].toLowerCase()).indexOf(dataToSearch) >= 0 || (data[i].toLowerCase()).indexOf(dataToSearch.toLowerCase()) >= 0) {
                coursesArrayResult.push(data[i]);
            }
        }
        if (coursesArrayResult.length == 1) {///////// להעביר לעמוד של הצגת קורס של גיל!!!!
            window.location = '/AddTeacherComment/AddTeacherComment.html?search=Courses&SearchText=' + coursesArrayResult[0].Id;

          //  window.location = '/HomePage/HomePage.html?search=Teachers&SearchText=' + coursesArrayResult[0];
        }
        else if (coursesArrayResult.length > 1) {
            showCoursesData(coursesArrayResult);
        }
        else if (coursesArrayResult.length == 0) {
            uri = '/api/Teachers/GetTeachers';
            $.getJSON(uri)
            .done(function (data) {
                for (i in data) { //search for teachers
                    if (dataToSearch == data[i] || data[i].indexOf(dataToSearch) >= 0 || (data[i].toLowerCase()).indexOf(dataToSearch) >= 0 || (data[i].toLowerCase()).indexOf(dataToSearch.toLowerCase()) >= 0) {
                        teachersArrayResult.push(data[i]);
                    }
                }
                if (teachersArrayResult.length == 1) { //אין תוצאות קורסים, ויש תוצאה אחת למרצים
                    ///////להעביר לעמוד הצגת תוצאות מרצים של גילללללללל!!!
                    window.location = '/AddTeacherComment/AddTeacherComment.html?search=Teachers&SearchText=' + teachersArrayResult[0].Id;
                 //   window.location = '/HomePage/HomePage.html?search=Teachers&SearchText=' + teachersArrayResult[0];
                }
                else if (teachersArrayResult.length == 0) { // אין תוצאות קורסים ואין תוצאות מרצים.
                    var textError = document.createTextNode('No matches found');
                    $('body').append(textError);
                }
                else {
                    showTeachersData(teachersArrayResult);
                }
            });
        }
    });
}

function showTeachersData(arrayResult) {
    var uri = '/api/Teachers/GetAllTeachers';
    var newLine = '<br>';
    $("#resultAdd").attr("href", "/AddTeacher/AddTeacher.html"); 
    $("#resultAdd").text('Cant find the requested teacher? CLICK HERE to add');

    $.getJSON(uri)
        .done(function (data) {
            for (j in arrayResult) {
            for (i in data) {
                if (data[i].Name == arrayResult[j]) {

                    var teacherData = $('<p />');
                    teacherData.addClass("lecturerData");

                    //adding teacher image
                    var img = new Image();
                    img.id = 'Img';
                    img.src = 'teacherImg.jpg';
                    teacherData.append(img);

                    //new line
                    teacherData.append(newLine);

                    //the teacher name
                    var a = $('<a />');
                    ///////גיללל תשנה כאן איך שאתה רוצה לקרוא למשתנה שמעביר את הGUID
                    a.attr('href', "/AddTeacherComment/AddTeacherComment.html?search=Teachers&SearchText=" + data[i].Id);
                  //  a.attr('href', "/HomePage/HomePage.html?search=Teachers&SearchText=" + data[i].Id); 
              //    
                    a.text(data[i].Name);
                    teacherData.append(a);

                    //new line
                    teacherData.append(newLine);

                    //adding the universities
                    for (k in data[i].Universities) {
                        if (k == data[i].Universities.length - 1) {
                            var University = document.createTextNode(data[i].Universities[k].Name + '.');
                        }
                        else {
                            var University = document.createTextNode(data[i].Universities[k].Name + ', ');
                        }
                        teacherData.append(University);
                    }

                    //new line
                    teacherData.append(newLine);

                    //////////////לא מצליחה להציג את הקורסים למרות שבשרת יש רשימה באורך 2
                    //adding the courses
                    for (l in data[i].Courses) {
                        if (k == data[i].Courses.length - 1) {
                            var Course = document.createTextNode(data[i].Courses[l].Name + '.');
                        }
                        else {
                            var Course = document.createTextNode(data[i].Courses[l].Name + ', ');
                        }
                        teacherData.append(Course);
                    }

                    $('body').append(teacherData);

                }
            }
            }
        });
}

function showCoursesData(arrayResult) {
    var uri = '/api/Courses/GetAllCourses';
    var newLine = '<br>';
    $("#resultAdd").attr("href", "/AddCourse/AddCourse.html");
    $("#resultAdd").text('Cant find the requested course? CLICK HERE to add');

    $.getJSON(uri)
        .done(function (data) { //כל הקורסים
            for (j in arrayResult) {
                for (i in data) {
                    if (data[i].Name == arrayResult[j]) {

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
                        a.attr('href', "/AddTeacherComment/AddTeacherComment.html?search=Courses&SearchText=" + data[i].Id);
                       // a.attr('href', "/HomePage/HomePage.html?search=Courses&SearchText=" + data[i].Name);
                        a.text(data[i].Name);
                        courseData.append(a);

                        //new line
                        courseData.append(newLine);

                        //////לא מצליחה להציג את הפקולטות!!! זה עושה UNDEFINED
                        var Faculty = document.createTextNode(data[i].Faculty.Name);// פקולטות
                        courseData.append(Faculty);

                        //new line
                        courseData.append(newLine);

                        //adding the university
                        var University = document.createTextNode(data[i].University.Name);
                        courseData.append(University);

                        $('body').append(courseData);
                    }
                }
            }
        });
}