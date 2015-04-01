var uri = 'api/HomePage/homePage';

$(document).ready(function () {
    $.getJSON(uri)
        .done(function (data) {
            $("#tags").autocomplete({
                source: data
            });
        });
});


