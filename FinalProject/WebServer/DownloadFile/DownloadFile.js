$(document).ready()
{
    downloadFile();
}

var uri;

function downloadFile() {
   // 
    var quertyString = getQuertyString();

        uri = "/api/GetFile/GetSyllabussss/" + quertyString['id'];
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
                        $("#fileWord")[0].hidden = false;
                        $("#fileWord").attr("href" , "../Images/fileWord.doc");
                    }else if( data.ext == ".docx") {
                        $("#fileWord")[0].hidden = false;
                        $("#fileWord").attr("href", "../Images/fileWord.docx");
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

function download(strData, strFileName, strMimeType) {
    var D = document, A = arguments, a = D.createElement("a"),
         d = A[0], n = A[1], t = A[2] || "aplication/pdf";

    //build download link:
    a.href = "data:" + strMimeType + "," + escape(strData);

    if ('download' in a) {
        a.setAttribute("download", n);
        a.innerHTML = "downloading...";
        D.body.appendChild(a);
        setTimeout(function () {
            var e = D.createEvent("MouseEvents");
            e.initMouseEvent(
                 "click", true, false, window, 0, 0, 0, 0, 0
                 , false, false, false, false, 0, null
            );
            a.dispatchEvent(e);
            D.body.removeChild(a);
        }, 66);
        return true;
    };

    var f = D.createElement("iframe");
    D.body.appendChild(f);
    f.src = "data:" + (A[2] ? A[2] : "application/octet-stream")+ (window.btoa?";base64"+(window.btoa?window.btoa:escape):(strData));
    setTimeout(function () { D.body.removeChild(f); }, 333);
    return true;
}
