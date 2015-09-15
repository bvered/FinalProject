function checkAndAdd() {
    var files = $("#fileUpload").get(0).files;

    var data = new FormData();

    if (files.length == 1) {
        data.append("UploadedFile", files[0]);
    }

    var universityName = $('#UniversityName').val();
    var universityAcronyms = $('#UniversityAcronyms').val();
    var universitySite = $('#UniversitySite').val();

    data.append("UniversityName", universityName);
    data.append("UniversityAcronyms", universityAcronyms);
    data.append("UniversitySite", universitySite);

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

    ajaxRequest.done(function () {
        $("#addUniversitySuccessfuly")[0].hidden = false;
        $("#UniversityExists")[0].hidden = true;
        $("#moveToNew")[0].hidden = false;
        $("#moveToNew")[0].href = "/HomePage/HomePage.html?University=" + universityAcronyms;
    });
};

function currentHomePage() {
    window.location = "/Universities/Universities.html";
}