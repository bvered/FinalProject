
$(document).ready(function () { ShowResults() });

function ShowResults() {

    var arrayResult = [];
    var isTeacher;

    //get the data from the url
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

    if (query_string["optionsRadio"] === "Courses") {
        uri = '/api/SearchResult/GetCourses';
        isTeacher = 0;
    } else {
        uri = '/api/SearchResult/GetTeachers';
        isTeacher = 1;
    }

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
            else {
                //הצגת מידע על מרצים
                if (isTeacher == 1) {
                    var uri = '/api/SearchResult/GetAllTeachers';
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
                                        var University = document.createTextNode(data[i].Universities[k].Name + ', ' ); //אוניברסיטה

                                        document.body.appendChild(University);
                                    }
                                    k = 0;

                                    ////מה זה לא מראה לי רשימה באורך 2 זה מראה לי שאיןקורסים בכלל למרות שבשרת יש
                                    for (l in data[i].Courses) {
                                 //       document.write(data[i].Courses[l].Name);
                                   //     document.write(" ");
                                    }
                                    l = 0;
                                }
                            }

                        });
                }
                    //הצגת מידע על קורסים
                else {
                    var uri = '/api/SearchResult/GetAllAllCourses';
                    $.getJSON(uri)
                        .done(function (data) { //כל הקורסים
                            var j = 0;
                            for (i in data) {
                                if (data[i].Name == arrayResult[j]) {
                                    document.write(arrayResult[j].toString());
                                    document.write("</br>");

                                    //////לא מצליחה להציג את הפקולטות!!! זה עושה UNDEFINED
                                    document.write(data[i].Faculty.Name + ", " + data[i].University.Name);
                                    document.write(" ");
                                    }
                                    document.write("</br>");
                                }
                        });
                }
            }

        });
};