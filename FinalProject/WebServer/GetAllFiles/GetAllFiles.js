var queryString;
var currentUniversity;

$(document).ready(function () {
    queryString = getQuertyString();
    $('#University').attr('value', queryString["University"]);
    currentUniversity = queryString["University"];

    getBackground(currentUniversity);

    initFilterValues();

    getAllFiles();
});


function initFilterValues() {
    var typeSelect = $('input:radio[name=type]');
    if (typeSelect.is(':checked') === false) {
        var valueString = '[value=All]';
        typeSelect.filter(valueString).prop('checked', true);
    }

    var typeSelect = $('input:radio[name=semester]');
    if (typeSelect.is(':checked') === false) {
        var valueString = '[value=All]';
        typeSelect.filter(valueString).prop('checked', true);
    }
}
var allFiles;
var grades=[];
var syllabus=[];

function getAllFiles()
{
    var quertyString = getQuertyString();

    var id = quertyString["courseId"];
    var uri = "/api/GetFile/GetAllFiles/" + id;
    var ajaxRequest = $.ajax({
        type: "GET",
        url: uri,
        contentType: "application/json",

        success: function (data)
        {
            allFiles = data;
            for (var i = 0; i < data.length; i++)
            {
                var rowCount = $('#resultsTable tr').length;
                var table = document.getElementById("resultsTable");
                var row = table.insertRow(rowCount);
                var cell1 = row.insertCell(0);
                var cell2 = row.insertCell(1);
                var cell3 = row.insertCell(2);
                var cell4 = row.insertCell(3);
                var fileType;

                var newlink = document.createElement('a');
                newlink.text = data[i].FileName;
                cell1.appendChild(newlink);
                cell2.innerHTML = data[i].Year;
                cell3.innerHTML = data[i].Semester;
                var isSyllabus = data[i].IsSyllabus;
                if (isSyllabus == false) {
                    fileType = 'Grades';
                    cell4.innerHTML = "התפלגות ציונים";
                    grades.push(data[i]);
                }
                else {
                    fileType = 'Syllabus';
                    cell4.innerHTML = "סילבוס";
                    syllabus.push(data[i]);
                }
                newlink.setAttribute('href', '/DownloadFile/DownloadFile.html?University=' + queryString["University"] + '&type=' + fileType + '&course=' + data[i].FileName + '&id=' + data[i].Id);

                cell4.innerHTML 

            }
        },
        fail: function(data) {
            //   succeed = false;
        },

    });
}

function ChangeResults() {
    clearResults();
    var filterType = $('input[name=type]:checked').val();
    var filterSemester = $('input[name=semester]:checked').val();
    var resultData = [];
    var requestedData = [];
    if (filterType == "All") 
    {
        resultData = allFiles;
    }
    else if (filterType == "Syllabus") {
        resultData = syllabus;
    }

    else if (filterType == "Grades") {
        resultData = grades;
    }


    if (filterSemester == "All") {
        showResults(resultData);

    }
    else if (filterSemester == "A") {
        requestedData = checkSemester(resultData, "א");
        showResults(requestedData);
    }
    else if (filterSemester == "B") {
        requestedData = checkSemester(resultData, "ב");
        showResults(requestedData);
    }
    else if (filterSemester == "Summer") {
        requestedData = checkSemester(resultData, "קיץ");
        showResults(requestedData);
    }

}

function checkSemester(requestedData, semester) {
    var resultData = [];
    for (var i = 0; i < requestedData.length; i++) {
        if (requestedData[i].Semester == semester) {
            resultData.push(requestedData[i]);
        }
    }
    return resultData;
}

function showResults(data) {
    for (var i = 0; i < data.length; i++) {
        var rowCount = $('#resultsTable tr').length;
        var table = document.getElementById("resultsTable");
        var row = table.insertRow(rowCount);
        var cell1 = row.insertCell(0);
        var cell2 = row.insertCell(1);
        var cell3 = row.insertCell(2);
        var cell4 = row.insertCell(3);

        var newlink = document.createElement('a');
        newlink.text = data[i].FileName;
        cell1.appendChild(newlink);
        newlink.setAttribute('href', '/DownloadFile/DownloadFile.html?type=Syllabus&course=' + data[i].FileName + '&id=' + data[i].Id);
        cell2.innerHTML = data[i].Year;
        cell3.innerHTML = data[i].Semester;
        var isSyllabus = data[i].IsSyllabus;
        if (isSyllabus == false) {
            cell4.innerHTML = "התפלגות ציונים";
        }
        else {
            cell4.innerHTML = "סילבוס";
        }
        cell4.innerHTML
    }
}

function clearResults() {
    for (var i = document.getElementById("resultsTable").rows.length -1; i > 1; i--) {
        document.getElementById("resultsTable").deleteRow(i);
    }
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