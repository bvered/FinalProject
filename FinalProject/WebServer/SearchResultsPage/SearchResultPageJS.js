/// לאחר שמציגים תוצאות, להוסיף בתחתית העמוד :
//לא מוצא את המרצה שאתה מחפש , לחץ כאן להוספה
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
            document.write("No matches found");
        }
        if (arrayResult.length == 1) { /////במקרה ויש תוצאה אחת צריך לבצע הפניה לדף של גיל
            window.location = '/AddTeacberComment/AddTeacherComment.html&Teacher=' + arrayResult[0];
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
            document.write("No matches found");
        }
            /* if (arrayResult.length == 1) { /////במקרה ויש תוצאה אחת צריך לבצע הפניה לדף של גיל
                 window.location = '/HomePage/HomePage.html?search=Teachers&SearchText=' + arrayResult[0];
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
            window.location = '/HomePage/HomePage.html?search=Teachers&SearchText=' + coursesArrayResult[0];
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
                    window.location = '/HomePage/HomePage.html?search=Teachers&SearchText=' + teachersArrayResult[0];
                }
                else if (teachersArrayResult.length == 0) { // אין תוצאות קורסים ואין תוצאות מרצים.
                    document.write("No matches found");
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

    $("#resultAdd").attr("href", "/AddTeacher/AddTeacher.html"); ///////לעשות העברה לעמוד של ורדי של הוספת קורס!!!!! ולא מרצה!!!
    $("#resultAdd").text('Cant find the requested teacher? CLICK HERE to add');

    $.getJSON(uri)
        .done(function (data) {
            var j = 0;
            for (i in data) {
                if (data[i].Name == arrayResult[j]) {

                    var newButton = document.createElement('input');
                    newButton.type = 'button';
                    newButton.value = data[i].Name;
                    newButton.type = 'submit';
                    newButton.id = 'Teacher';
                    newButton.onclick = function () { //////////להעביר לעמוד הצגת מרצה של גיללללללל
                        window.location = '/HomePage/HomePage.html?search=Teachers&SearchText=' + newButton.value;
                    }
                    document.body.appendChild(newButton);

                    for (k in data[i].Universities) {
                        var University = document.createTextNode(data[i].Universities[k].Name + ', '); //אוניברסיטה
                        document.body.appendChild(University);
                    }

                    ////מה זה לא מראה לי רשימה באורך 2 זה מראה לי שאיןקורסים בכלל למרות שבשרת יש
                    for (l in data[i].Courses) {
                        var course = document.createTextNode(data[i].Courses[l].Name + ', '); //קורסים
                        document.body.appendChild(course);
                    }
                }
            }
        });
}

function showCoursesData(arrayResult) {
    var uri = '/api/Courses/GetAllCourses';

    $("#resultAdd").attr("href", "/AddTeacher/AddTeacher.html"); ///////לעשות העברה לעמוד של ורדי של הוספת קורס!!!!! ולא מרצה!!!
    $("#resultAdd").text('Cant find the requested course? CLICK HERE to add');

    $.getJSON(uri)
        .done(function (data) { //כל הקורסים
            var j = 0;
            for (i in data) {
                if (data[i].Name == arrayResult[j]) {
                    var newButton = document.createElement('input');
                    newButton.type = 'button';
                    newButton.value = data[i].Name;
                    newButton.type = 'submit';
                    newButton.id = 'Course';
                    newButton.onclick = function () { ////להעביר לעמוד הצגת קורס של גיללללל
                        window.location = '/HomePage/HomePage.html?search=Courses&SearchText=' + newButton.value;
                    }
                    document.body.appendChild(newButton);

                    //////לא מצליחה להציג את הפקולטות!!! זה עושה UNDEFINED
                    var Faculty = document.createTextNode(data[i].Faculty.Name + ', ');// פקולטות
                    document.body.appendChild(Faculty);

                    var University = document.createTextNode(data[i].University.Name);
                    document.body.appendChild(University);
                }
            }
        });
}