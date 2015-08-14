var queryString;
var currentUniversity;

$(document).ready(function () {
    queryString = getQuertyString();
    $('#University').attr('value', queryString["University"]);
    currentUniversity = queryString["University"];

    getAllSillabus();
});


function getAllSillabus()
{
   
    var quertyString = getQuertyString();

    var id = quertyString["courseId"];
    var uri = "/api/GetFile/GetSyllabus/" + id;
    var ajaxRequest = $.ajax({
        type: "GET",
        url: uri,
        contentType: "application/json",

        success: function (data)
        {
            for (var i = 0; i < data.length; i++)
            {
                var rowCount = $('#resultsTable tr').length;
                var table = document.getElementById("resultsTable");
                var row = table.insertRow(rowCount);
                var cell1 = row.insertCell(0);
                var cell2 = row.insertCell(1);
                var cell3 = row.insertCell(2);


                var newlink = document.createElement('a');
                newlink.text = data[i].FileName;
                cell1.appendChild(newlink);
                newlink.setAttribute('href', '/DownloadFile/DownloadFile.html?type=Syllabus&course=' + data[i].FileName + '&id=' + data[i].Id);

               // cell1.innerHTML = data[i].FileName;
                cell2.innerHTML = data[i].Year;
                cell3.innerHTML = data[i].Semester;
                console.log("hi");
            }
        },
        fail: function(data) {
            //   succeed = false;
        },

        //contentType: false,
        //processData: false,
        //data: data
    });
}

function createCell(cell, text, style) {
    var div = document.createElement('div'), // create DIV element
        txt = document.createTextNode(text); // create text node
    div.appendChild(txt);                    // append text node to the DIV
    div.setAttribute('class', style);        // set DIV class attribute
    div.setAttribute('className', style);    // set DIV class attribute for IE (?!)
    cell.appendChild(div);                   // append DIV to the table cell
}

function homePage() {
    window.location = "../HomePage/HomePage.html?University=" + currentUniversity;
}