var queryString;
var currentUniversity;

$(document).ready(function () {
    queryString = getQuertyString();

    $('#University').attr('value', queryString["University"]);
    currentUniversity = queryString["University"];

    ifNoUniversity(currentUniversity);
    setUniversityInfo();

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
                    source: function (request, response) {
                        var results = $.ui.autocomplete.filter(data, request.term);
                        response(results.slice(0, 10));
                    }
                });
            });
};

function BestCourses() {
    window.location = "../SearchResultsPage/SearchResultPage.html?University=" + currentUniversity + "&search=Courses&isTop=true&SearchText=&degree=&year=&faculty=&mandatory=";
}

function BestTeachers() {
    window.location = "../SearchResultsPage/SearchResultPage.html?University=" + currentUniversity + "&search=Teachers&isTop=true&SearchText=&degree=&year=&faculty=&mandatory=";
}

function change1(choose) {
    $("#dropDownRes1").html($(choose).text() + '<span class=\caret\"></span>');
    $("#dropDownRes1").attr('value', $(choose).attr('value'));
    $("#search").attr('value', $(choose).attr('value'));
    SmartSearch();
}

$(document).ready(function () {
    $('#advancedSearch').on('click', function () {
        $('#advancedSearchDiv').toggle('show');
    });

    $("#advanced-search-btn").click(function () {
        $(".advanced-filter").toggle("slow");
    });
});

function sumbit() {
    if (event.keyCode == 13) {
        $("#sendButton").click();
    }
}

function changeUniversity() {
    window.location = "../Universities/Universities.html";
}

function setUniversityInfo() {
    var uri = '/api/University/GetUvinersitryPicture/' + currentUniversity;
    $.ajax({
        type: "GET",
        url: uri,
        contentType: "application/json",
        success: function(data) {
            SetBackgroundImage(data);
            document.title = "Rate My School- " + currentUniversity;
            $("#UniversityInfo")[0].innerText += " " + data.UniversityName;
            $("#UniversityInfo")[0].onclick = function() {
                var win = window.open("http://" + data.WebAddress, "_blank");
                win.focus();
            }
        }
    });
};
