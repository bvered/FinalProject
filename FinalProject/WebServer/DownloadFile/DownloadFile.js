var queryString;
var currentUniversity;
var fileType;

$(document).ready()
{
    queryString = getQuertyString();
    $('#University').attr('value', queryString["University"]);
    currentUniversity = queryString["University"];
    fileType = queryString["type"];
    getBackground(currentUniversity);
    downloadFile();
}

var uri;

function downloadFile() {
   // 
    var quertyString = getQuertyString();

        uri = "/api/GetFile/GetSpecificFile/" + quertyString['id'];
        var ajaxRequest = $.ajax({
            type: "GET",
            url: uri,
            contentType: "application/json",

            success: function (data)
            {
                if (data.isPic == true) {
                    $("#fileImg")[0].hidden = false;
                    $("#fileImg").attr("src", "../Images/filePic.jpg");
                }
                else if (data.isPic == false) {
                    if (data.ext == ".pdf") {
                        $("#filePdf").attr("data", "../Images/filePdf.pdf");
                        $("#filePdf")[0].hidden = false;
                    }
                    else if (data.ext == ".doc")
                    {
                        changeText();
                        $("#fileWord")[0].hidden = false;
                        $("#fileWord").attr("href", "../Images/fileWord.doc");
                        $("#link")[0].hidden = false;
                    } else if (data.ext == ".docx") {
                        changeText();
                        $("#fileWord")[0].hidden = false;
                        $("#fileWord").attr("href", "../Images/fileWord.docx");
                        $("#link")[0].hidden = false;
                    }
                    else if (data.ext == ".txt") {
                        $("#textFile").append(data.str);
                    }
                }
            },
            fail: function (data)
            {},
       });
    }

function changeText() {
    if (fileType == 'Grades') {
        $("#fileWord").text('לחץ כאן כדי להוריד את התפלגות הציונים');
    }
    else if (fileType == 'Syllabus') {
        $("#fileWord").text('לחץ כאן כדי להוריד את הסילבוס');
    }
}

function homePage() {
    window.location = "../HomePage/HomePage.html?University=" + currentUniversity;
}