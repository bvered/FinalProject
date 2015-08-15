var queryString;
var currentUniversity;

$(document).ready(function () {
    queryString = getQuertyString();
    $('#University').attr('value', queryString["University"]);
    currentUniversity = queryString["University"];
    getBackground(currentUniversity);
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
        data: data,

        statusCode: {
            409: function () {
                $("#UniversityExists")[0].hidden = false;
                $("#addUniversitySuccessfuly")[0].hidden = true;
            }
        }
    });

    ajaxRequest.done(function (xhr, textStatus) {
        $("#addUniversitySuccessfuly")[0].hidden = false;
        $("#UniversityExists")[0].hidden = true;

    });
};

function homePage() {
    window.location = "../HomePage/HomePage.html?University=" + currentUniversity;
}
