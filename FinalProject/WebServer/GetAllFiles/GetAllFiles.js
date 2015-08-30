var queryString;
var currentUniversity;
var allFiles;
var grades = [];
var syllabus = [];

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
    var valueString;
    if (typeSelect.is(':checked') === false) {
         valueString = '[value=All]';
        typeSelect.filter(valueString).prop('checked', true);
    }
    typeSelect = $('input:radio[name=semester]');
    if (typeSelect.is(':checked') === false) {
         valueString = '[value=All]';
        typeSelect.filter(valueString).prop('checked', true);
    }
}
function getAllFiles()
{
    var id = queryString["courseId"];
    var uri = "/api/GetFile/GetAllFiles/" + id;
    $.ajax({
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

                var newlink = document.createElement('a');
                newlink.text = data[i].FileName;
                cell1.appendChild(newlink);
                cell2.innerHTML = data[i].Year;
                cell3.innerHTML = data[i].Semester;
                var isSyllabus = data[i].IsSyllabus;
                cell4.innerHTML = data[i].Type;
                if (isSyllabus == false) {
                    
                    grades.push(data[i]);
                }
                else {
                    
                    syllabus.push(data[i]);
                }
                newlink.setAttribute('href', "javascript:download('"+data[i].Id+"')");
            }
        },
        fail: function() {
        }
    });
}

function download(fileId) {
    var uri = "/api/GetFile/GetSpecificFile/" + fileId;
     $.ajax({
        type: "GET",
        url: uri,
        contentType: "application/json",

        success: function (data) {
                $("#downloadFile").attr("href", data.str);
                $("#downloadFile").attr("download", data.str);
                $('#downloadFile')[0].click();
        },
        fail: function ()
        { }
    });
}

function ChangeResults() {
    clearResults();
    var filterType = $('input[name=type]:checked').val();
    var filterSemester = $('input[name=semester]:checked').val();
    var resultData = [];
    var requestedData;
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
