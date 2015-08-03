function SmartSearch() {
    
    var uri;
    if (($("#dropDownRes1")[0]).value.trim() == "Courses") {
        uri = '/api/Courses/GetCourses';
    } else if (($("#dropDownRes1")[0]).value.trim() == "Teachers") {
        uri = '/api/Teachers/GetTeachers';
    }
    else if (($("#dropDownRes1")[0]).value.trim() == "All") {
        uri = '/api/SmartSearch/GetAll';
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

