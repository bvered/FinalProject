/// <reference path="../AddUniversity/AddUniversity.html" />
var queryString;
var currentUniversity;

$(document).ready(function () {
    queryString = getQuertyString();
    if (queryString["University"] == undefined) {
        var url = window.location.href + "?University=MTA";
        window.location.href = url;
    } else {
        $('#University').attr('value', queryString["University"]);

        currentUniversity = queryString["University"];
        GetUniversities();
        SmartSearch();
    }
});

function SmartSearch() {

    var uri;
    if (($("#dropDownRes1")[0]).value.trim() == "Courses") {
        uri = '/api/Courses/GetCoursesNames/' + currentUniversity;
    } else if (($("#dropDownRes1")[0]).value.trim() == "Teachers") {
        uri = '/api/Teachers/GetTeachersNames/' + currentUniversity;
    }
    else if (($("#dropDownRes1")[0]).value.trim() == "All") {
        uri = '/api/SmartSearch/GetAllNames/' + currentUniversity;
    }

    $.getJSON(uri)
            .done(function (data) {
                console.log(data);

                $("#tags").autocomplete({
                    source: function (request, response) {
                        var results = $.ui.autocomplete.filter(data, request.term);
                        response(results.slice(0, 10));
                    }
                });
            });
};

function BestCourses() {
    window.location = "../SearchResultsPage/SearchResultPage.html?search=Courses&isTop=true&SearchText=&degree=&year=&faculty=&mandatory=";
}

function BestTeachers() {
    window.location = "../SearchResultsPage/SearchResultPage.html?search=Teachers&isTop=true&SearchText=&degree=&year=&faculty=&mandatory=";
}

function change1(choose) {

    $("#dropDownRes1").html($(choose).text() + '<span class=\caret\"></span>');
    $("#dropDownRes1").attr('value', $(choose).attr('value'));
    $("#search").attr('value', $(choose).attr('value'));
    SmartSearch();
}

$(document).ready(function () {
    $('#advancedSearch').on('click', function (event) {
        $('#advancedSearchDiv').toggle('show');
    });

    $("#advanced-search-btn").click(function () {
        $(".advanced-filter").toggle("slow");
    });
});

function homePage() {
    window.location = "HomePage.html?University=" + currentUniversity;
}

function sumbit() {
    if (event.keyCode == 13) {
        $("#sendButton").click();
    }
}

function GoToSchool() {
    window.location = "https://www.mta.ac.il/he-il/";
}

function GetUniversities() {
    var uri = '/api/University/GetUniversities';

    var request = $.ajax({
        type: "GET",
        url: uri,
        contentType: "application/json",

        success: function (data) {
            showUniversities(data);
        }
    });
}

function showUniversities(universities) {
    for (var i = 0; i < universities.length; i++) {
        var newUniversity = document.createElement('li');
        newUniversity.innerText = universities[i].Name;
        newUniversity.id = universities[i].WebAddress;

        newUniversity.onclick = changePage;

        $('#Universities')[0].appendChild(newUniversity);
    }
}

function changePage() {
    window.location = "http://" + this.id;
}

function addUniversity() {
    window.location = "../AddUniversity/AddUniversity.html";
}

