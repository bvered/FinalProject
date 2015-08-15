var queryString;
var currentUniversity;

$(document).ready(function () {
    queryString = getQuertyString();
    $('#University').attr('value', queryString["University"]);
    currentUniversity = queryString["University"];
});

function checkAndAdd() {
    if (DoesTheUniversityAlreadyExists()) {
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
    }
};

function homePage() {
    window.location = "../HomePage/HomePage.html?University=" + currentUniversity;
}

function DoesTheUniversityAlreadyExists() {
    var uri = '/api/University/CheckIfUniversityExists';

    $(function () {
        var university = {
            Name: $('#UniversityName').val(),
            Acronyms: $('#UniversityAcronyms').val(),
            WebAddress: $('#UniversitySite').val(),
        };

        var request = $.ajax({
            type: "POST",
            data: JSON.stringify(university),
            url: uri,
            contentType: "application/json"
        });

        request.done(function () {
            return true;
        });

        request.fail(function (jqXhr, textStatus) {
            $("#UniversityExists")[0].hidden = false;
            return false;
        });
    });
}