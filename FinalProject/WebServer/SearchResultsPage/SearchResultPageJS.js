
$(document).ready(function () { ShowResults() });

    function ShowResults() {
        var uri;
        //לבדוק מה נבחר, קורסים או מרצים
        uri = '/api/SearchResult/GetCourses';
        var arrayResult = [];
        var courseName = "Math"; //צריך לקחת את השם שקיבלתתי בעמוד הקודם


        $.getJSON(uri)
            .done(function (data) { //פה אני מקבלת בדטה את כל המרצים או קורסים
                var j = 0;
                for (i in data) {
                    if (courseName == data[i].Name || courseName.indexOf(data[i].Name, 0) > -1) {
                        arrayResult.push(data[i].Name);

                        document.write(arrayResult[j].toString());
                        j++;
                    }
                }
            });
    };