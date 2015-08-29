function getQuertyString() {

    var queryString = [];

    var query = window.location.search.substring(1);
    query = decodeURI(query);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (typeof queryString[pair[0]] === "undefined") {
            queryString[pair[0]] = pair[1];
        } else if (typeof queryString[pair[0]] === "string") {
            var arr = [queryString[pair[0]], pair[1]];
            queryString[pair[0]] = arr;
        } else {
            queryString[pair[0]].push(pair[1]);
        }
    }

    return queryString;
}

function getBackground(currentUniversity) {
    var uri = '/api/University/GetUvinersitryPicture/' + currentUniversity;
    $.ajax({
        type: "GET",
        url: uri,
        contentType: "application/json",
        success: SetBackgroundImage
    });
}

function SetBackgroundImage(data) {
    $(".background").css('background-image', 'url(' + data.Base64 + ')');
}

function homePage() {
    var queryString = getQuertyString();
    var currentUniversity = queryString["University"];
    window.location = "/HomePage/HomePage.html?University=" + currentUniversity;
}