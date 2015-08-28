$(document).ready(function () {
    GetUniversities();
});

function GetUniversities() {
    var uri = '/api/University/GetUniversities';

    var request = $.ajax({
        type: "GET",
        url: uri,
        contentType: "application/json",

        success: function (data) {
            showUniversities(data);
        }
    });
}

function showUniversities(universities) {
    for (var i = 0; i < universities.length; i++) {
        var newUniversity = document.createElement('li');
        newUniversity.innerText = universities[i].Name;
        newUniversity.id = universities[i].Acronyms;

        newUniversity.onclick = changePage;

        $('#Universities')[0].appendChild(newUniversity);
    }
}

function addUniversity() {
    window.location = "../AddUniversity/AddUniversity.html?University=" + 'MTA';
}

function changePage() {
    window.location = "HomePage.html?University=" + this.id;
}