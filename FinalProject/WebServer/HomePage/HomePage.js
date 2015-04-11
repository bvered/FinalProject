function SmartSearch() {
    var uri;
    if (($("#optionsRadioCourses")[0]).checked) {
        uri = '/api/Courses/GetCourses';
    } else {
        uri = '/api/Teachers/GetTeachers';
    }
    $.getJSON(uri)
        .done(function (data) {
            console.log(data);
            $("#tags").autocomplete({
                source: data
            });
        });
};

$(document).load(SmartSearch());