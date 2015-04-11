function SmartSearch() {
    var uri;
    if (($("#optionsRadioCourses")[0]).checked) {
        uri = '/api/HomePage/GetCourses';
    } else {
        uri = '/api/HomePage/GetTeachers';
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