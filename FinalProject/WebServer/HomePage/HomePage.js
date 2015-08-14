var queryString;
var currentUniversity;

$(document).load(function () {
    queryString = getQuertyString();
    $('#University').attr('value', queryString["University"]);

    currentUniversity = queryString["University"];
    GetUniversities();
    SmartSearch();
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
                source: data
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
    window.location = "HomePage.html";
}

function sumbit(){
    if (event.keyCode == 13) {
        $("#sendButton").click();
    }
}

function GoToSchool() {
    window.location = "https://www.mta.ac.il/he-il/";
}

function GetUniversities() {
    var uri = '/api/UniverstiryController/GetUniversities';

    var request = $.ajax({
        type: "GET",
        url: uri,
        contentType: "application/json",

        success: function(data) {
            var showUniversities = function(data1) { throw new Error("Not implemented"); };
            showUniversities(data);
        }
    });
}

