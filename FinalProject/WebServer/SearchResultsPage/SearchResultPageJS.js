
$(document).ready(function () { ShowResults() });

function ShowResults() {

    var arrayResult = [];
    var isTeacher;

   var query_string = {};
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

    if (query_string["optionsRadio"] === "All")
    {
        var teachersArrayResult = [];
        var coursesArrayResult = [];
        uri = '/api/Teachers/GetTeachers';
        teachersArrayResult = getData(uri, query_string);
        uri = '/api/Courses/GetCourses';
        coursesArrayResult = getData(uri, query_string);

        if (teachersArrayResult.length == 0 && coursesArrayResult.length == 0) {
        } else if (teachersArrayResult.length == 1 && coursesArrayResult.length == 0) {
            window.location = '/HomePage/HomePage.html?optionsRadio=Teachers&SearchText=' + teachersArrayResult[0];
        } else if (teachersArrayResult.length == 0 && coursesArrayResult.length == 1) {
            window.location = '/HomePage/HomePage.html?optionsRadio=Courses&SearchText=' + coursesArrayResult[0];
        } else {
            showTeachersData(teachersArrayResult);
            showCoursesData(coursesArrayResult);
        }
    }
    else if (query_string["optionsRadio"] === "Courses")
    {
        uri = '/api/Courses/GetCourses';
        arrayResult = getData(uri, query_string);
        if (arrayResult.length == 1) {  /////במקרה ויש תוצאה אחת צריך לבצע הפניה לדף של גיל
                 window.location = '/HomePage/HomePage.html?optionsRadio=Courses&SearchText=' + arrayResult[0];
        }
        showCoursesData(arrayResult);
    } else {
        arrayResult = [];
        uri = '/api/Teachers/GetTeachers';
        arrayResult = getData(uri, query_string);
        if (arrayResult.length == 1) {/////במקרה ויש תוצאה אחת צריך לבצע הפניה לדף של גיל
              window.location = '/HomePage/HomePage.html?optionsRadio=Teachers&SearchText=' + arrayResult[0];
        }
        showTeachersData(arrayResult);
    }

};
    function getData(uri, query_string) {
        var arrayResult = [];
        var dataToSearch = query_string["SearchText"];
        $.getJSON(uri)
        .done(function (data) {
            var j = 0;
            for (i in data) {
                if (dataToSearch == data[i] || data[i].indexOf(dataToSearch) >= 0 || (data[i].toLowerCase()).indexOf(dataToSearch) >= 0 || (data[i].toLowerCase()).indexOf(dataToSearch.toLowerCase()) >= 0) {
                    arrayResult.push(data[i]);
                    j++;
                }
            }
            if (arrayResult.length == 0) {
                document.write("No matches found");
            }
        });

        return arrayResult;
    }

    function showTeachersData(arrayResult) {
        var uri = '/api/Teachers/GetAllTeachers';
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
                        newButton.onclick = function () {
                            window.location = '/HomePage/HomePage.html?optionsRadio=Teachers&SearchText=' + newButton.value;
                        }
                        document.body.appendChild(newButton);

                        for (k in data[i].Universities) {
                            var University = document.createTextNode(data[i].Universities[k].Name + ', '); //אוניברסיטה
                            document.body.appendChild(University);
                        }
                        k = 0;

                        ////מה זה לא מראה לי רשימה באורך 2 זה מראה לי שאיןקורסים בכלל למרות שבשרת יש
                        for (l in data[i].Courses) {
                            var course = document.createTextNode(data[i].Courses[l].Name + ', '); //קורסים
                            document.body.appendChild(course);
                        }
                        l = 0;
                    }
                }
            });
    }

    function showCoursesData(arrayResult) {
        var uri = '/api/Courses/GetAllCourses';
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
                        newButton.onclick = function () {
                            window.location = '/HomePage/HomePage.html?optionsRadio=Courses&SearchText=' + newButton.value;
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


