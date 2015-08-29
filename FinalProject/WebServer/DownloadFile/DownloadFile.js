var queryString;
var currentUniversity;
var fileType;

$(document).ready(function() {
    queryString = getQuertyString();
    $('#University').attr('value', queryString["University"]);
    currentUniversity = queryString["University"];
    fileType = queryString["type"];
    getBackground(currentUniversity);
    downloadFile();
});
var uri;

function downloadFile() {
    var quertyString = getQuertyString();

        uri = "/api/GetFile/GetSpecificFile/" + quertyString['id'];
        $.ajax({
            type: "GET",
            url: uri,
            contentType: "application/json",

            success: function (data)
            {
                if (data.isPic == true) {
                    $("#fileImg")[0].hidden = false;
                    $('#meital')[0].click();
                    $('#ortal')[0].click();
                   // $("#meital").click();
                    $("#fileImg").attr("src", "../Images/filePic.jpg");
                //    window.open("../Images/filePic.jpg", '_blank', '');
                 //   window.open("../Images/filePic.jpg");
                  //  document.location = "../Images/filePic.jpg";
                    
                }
                else if (data.isPic == false) {
                    if (data.ext == ".pdf") {
                        $("#filePdf").attr("data", "../Images/filePdf.pdf");
                        $("#filePdf")[0].hidden = false;
                     //   document.location.href = "../Images/filePdf.pdf";
                    }
                    else if (data.ext == ".doc")
                    {
                        changeText();
                        $("#fileWord")[0].hidden = false;
                        $("#fileWord").attr("href", "../Images/fileWord.doc");
                        document.location.href = "../Images/fileWord.doc";
                        $("#link")[0].hidden = false;
                    } else if (data.ext == ".docx") {
                        changeText();
                        $("#fileWord")[0].hidden = false;
                        $("#fileWord").attr("href", "../Images/fileWord.docx");
                        $("#link")[0].hidden = false;
                        document.location.href = "../Images/fileWord.docx";
                    }
                    else if (data.ext == ".txt") {
                        $("#textFile").append(data.str);
                    }
                }
            },
            fail: function ()
            {}
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
