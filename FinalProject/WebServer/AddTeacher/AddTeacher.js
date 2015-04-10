
var uri = '/api/Courses/GetCourses';
var uri2 = '/api/Universities/GetUniversities';

var allCourses = $.getJSON(uri).done((data));
var allUniversites = $.getJSON(uri2).done((data));


function SmartSearch() {
    console.log(allCourses);
    $("#CourseName").autocomplete({
        source: allCourses
    });

    console.log(allUniversites);
    $("#UniversityName").autocomplete({
        source: allUniversites
    });
};


function checkAndAdd() {  
};

function checkUniveristy() {
    if (allUniversites.indexOf($("#UniversityName").val()) > 0) 

}

$(document).load(SmartSearch());