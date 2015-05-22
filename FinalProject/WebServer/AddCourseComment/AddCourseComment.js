
var uri = '/api/Teachers/GetCriterias';
var uri2 = '/api/Teachers/AddComment';
var uri3 = '/api/Teachers/GetTeachers';   
var uri4 = '/api/Teachers/AddComment'; 
var uri5 = '/api/Teachers/GetTeacher';

var teacherFound;
var allCriterias;
var allTeachers;
var teacherInfoList;

var TeacherInfoDiv = document.getElementById("TeacherInfoDiv");
var teacherCommentsDiv = document.getElementById("TeacherCommentsDiv");

$.getJSON(uri).done(function(data) {
    allCriterias = data;
    console.log(allCriterias);
    populateCriterias();
});

$.getJSON(uri3).done(function(data) {
    allTeachers = data;
});

function printInformationOfTeacher() {
    $.getJSON(uri5).done(function(data) {
	    teacher = data;
	    showTeacherInfoToUser(teacher);
	    showTeacherCommentsByTeacher(teacher);
	});
}

function showTeacherInfoToUser(teacher) {
	addPropertyToDiv(teacher.Name);
	addPropertyToDiv(teacher.Score);
}

function addPropertyToDiv(key, value)
{
	var keyLabelElement = document.createElement("Label");
	keyLabelElement.id = "propertyLabelDescriptionId"+key;
	keyLabelElement.innerHTML = key;
	TeacherInfoDiv.appendChild(keyLabelElement);

	var ValueLabelElement = document.createElement("Label");
	ValueLabelElement.id = "propertyLabelValueId"+key;
	ValueLabelElement.innerHTML = value;
	TeacherInfoDiv.appendChild(ValueLabelElement);
}

function printUniversities(teacher)
{
	var universityLabel = document.createElement("Label");
	universityLabel.id = "universityLabel";
	universityLabel.innerHTML = "Universities";
	TeacherInfoDiv.appendChild(universityLabel);
	if (teacher.Universities.length > 0)
	{
		for (university in teacher.Universities)
		{
		    var universityButton = document.createElement("BUTTON");
	    	universityButton.id = "universityLink"+university;
		    universityButton.appendChild(document.createTextNode(teacher.Universities[university].Name));
	    	universityButton.onclick = // TODO: Send to correct University;
	    	TeacherInfoDiv.appendChild(universityButton);
		}
	}
	else 
	{
		var noUniversityLabel = document.createElement("Label");
		noUniversityLabel.id = "noUniversityLabel";
		noUniversityLabel.innerHTML = "No universities found for: " + Teacher.Name + ".";
		TeacherInfoDiv.appendChild(noUniversityLabel);
	}
}

function printCourses(teacher)
{
	var courseLabel = document.createElement("Label");
	courseLabel.id = "courseLabel";
	courseLabel.innerHTML = "Courses";
	TeacherInfoDiv.appendChild(courseLabel);
	if (teacher.Courses.length > 0)
	{
		for (course in teacher.Courses)
		{
		    var courseButton = document.createElement("BUTTON");
	    	courseButton.id = "universityLink"+university;
		    courseButton.appendChild(document.createTextNode(teacher.Courses[course].Name));
	    	courseButton.onclick = // TODO: Send to correct University;
	    	TeacherInfoDiv.appendChild(courseButton);
		}
	}
	else 
	{
		var noCourseLabel = document.createElement("Label");
		noCourseLabel.id = "noCourseLabel";
		noCourseLabel.innerHTML = "No courses found for: " + Teacher.Name + ".";
		TeacherInfoDiv.appendChild(noUniversityLabel);
	}
}

function showTeacherCommentsByTeacher(teacher) {
    var allComments = teacher.TeacherComments;
    for (comment in allComments) {
        printComment(teacherCommentDiv, allComments[comment], comment);
    }
}

function checkAndAdd() {
    var addComment = new Array();
    addComment.push($("#TeacherId").val());
    addComment.push($("#CommentText").val());
    for(criteria in allCriterias) {
        addComment.push($("Rating"+criteria).val());
    }
};

function addComment() {
    $(function() {
        var ratings = [];
        for(criteria in allCriterias) {
            ratings.push(document.getElementById("criteriaRating"+criteria).value);
        }

        var comment = {
            Id: UserId,
            teacher: document.getElementById("TeacherId").value,
            Comment: document.getElementById("TeacherNewCommetBox").value,
            Ratings: ratings
        };

        var request = $.ajax({
            type: "POST",
            data: JSON.stringify(comment),
            url: uri4,
            contentType: "application/json"
        });
        request.done(function() {
            document.getElementById("newCommentForm").fadeOut("slow");
            divToAppend.display = none;
        });
        request.fail(function(jqXhr, textStatus) {
            alert("Request failed: " + textStatus);
        });
        var divToAppend = document.getElementById("newCommentForm");
    });
}

function populateCriterias() {
    var divToAppend = document.getElementById("newCommentForm");
    var commentBox = document.createElement("textarea");
    commentBox.placeholder = "Write something..";
    commentBox.id = "TeacherNewCommetBox";
    divToAppend.appendChild(commentBox);
    divToAppend.appendChild(document.createElement("BR"));
    for (ratingText in allCriterias) {
        var criteriaText = allCriterias[ratingText];
        var labelInput = document.createElement("Label");
        labelInput.id = "criteriaText" + ratingText;
        labelInput.innerHTML = criteriaText;

        var inputText = document.createElement("INPUT");
        inputText.id = "criteriaRating" + ratingText;
        inputText.name = "criteriaRating" + ratingText;
        inputText.type = "number";
        inputText.min = "0";
        inputText.max = "5";

        divToAppend.appendChild(labelInput);
        divToAppend.appendChild(inputText);

        divToAppend.appendChild(document.createElement("BR"));
    }
    var sendButton = document.createElement("BUTTON");
    sendButton.onclick = addComment();
    divToAppend.appendChild(sendButton);
}

function printComment(divToAppend, comment, itr) {
    var commentBox = document.createElement("textarea");
    commentBox.placeholder = comment.CommentText;
    commentBox.id = "TeacherCommet" + itr;
    divToAppend.appendChild(commentBox);
    for (rating in CriteriaRatings) {
        var ratingString = CriteriaRatings[rating].Criteria.DisplayName;
        var ratingNumber = CriteriaRatings[rating].Criteria.Rating;

        var labelForRatingString = document.createElement("Label");
        labelForRatingString.id = "CommentNumber" + itr + "RatingString" + rating;
        labelForRatingString.innerHTML = ratingString;

        var labelForRatingNumber = document.createElement("Label");
        labelForRatingNumber.id = "CommentNumber" + itr + "RatingNumber" + rating;
        labelForRatingNumber.innerHTML = ratingString;
    }
}

