var queryString;
var currentUniversity;

$(document).ready(function() {
    queryString = getQuertyString();
    $('#University').attr('value', queryString["University"]);
    currentUniversity = queryString["University"];
});

function checkAndAdd() {

    var files = $("#fileUpload").get(0).files;

    var data = new FormData();

    if (files.length == 1) {
        data.append("UploadedFile", files[0]);
    }


    var UniversityName = $('#UniversityName').val();
    var UniversityAcronyms = $('#UniversityAcronyms').val();
    var UniversitySite = $('#UniversitySite').val();
    

    data.append("UniversityName", UniversityName);
    data.append("UniversityAcronyms", UniversityAcronyms);
    data.append("UniversitySite", UniversitySite);

    var ajaxRequest = $.ajax({
        type: "POST",
        url: "/api/University/AddUniversity",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (xhr, textStatus) {
        $("#addUniversitySuccessfuly")[0].hidden = false;
    });
};

function homePage() {
    window.location = "../HomePage/HomePage.html?University=" + currentUniversity;
}

