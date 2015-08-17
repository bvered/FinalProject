$(document).ready()
{
    downloadFile();
}

var uri;

function downloadFile() {
    var quertyString = getQuertyString();

    if (quertyString['type'] = 'Syllabus') {
        uri = "/api/GetFile/GetSyllabussss/" + quertyString['id'];
        var ajaxRequest = $.ajax({
            type: "GET",
            url: uri,
            contentType: "application/json",

            success: function (data)
            {
                if (data.isPic == true) {
                    $("#fileImg").attr("src", "../Images/filePic.jpg");
                }
                else if (data.isPic == false) {
                    $("#textFile").append(data.str);
                  //  download(data.str, "test.pdf", "aplication/pdf");
                }
                
         //       download(data.str, "test.pdf", "aplication/pdf");

            },
            fail: function (data)
            {
                //   succeed = false;
            },
       });
    }
    else if (quertyString[type] = Grades) {


    }
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
