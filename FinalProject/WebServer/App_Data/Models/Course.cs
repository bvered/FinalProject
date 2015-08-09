using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebServer.App_Data.Models.Enums;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class Course : IPersistent
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public int CourseId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Score { get; set; }
        [DataMember]
        public University University { get; set; }
        [DataMember]
        public Faculty Faculty { get; set; }
        [DataMember]
        public bool IsMandatory { get; set; }
        [DataMember]
        public AcademicDegree AcademicDegree { get; set; }
        [DataMember]
        public IntendedYear IntendedYear { get; set; }
        [DataMember]
        public IList<CourseInSemester> CourseInSemesters { get; set; }
        [DataMember]
        public AverageRatings AverageCriteriaRatings { get; set; }

        public Course()
        {
            SetupCourse();
        }

        public Course(int courseId, string name, Faculty faculy)
        {
            SetupCourse();
            Id= new Guid();
            CourseId = courseId;
            Name = name;
            Faculty = faculy;
        }

        public List<Teacher> GetTeachers()
        {
            return CourseInSemesters.Select(x => x.Teacher).Distinct().ToList();
        }

        public void AddCourseInSemester(CourseInSemester semester)
        {
            CourseInSemesters.Add(semester);
        }

        public void AddCourseCommnet(CourseInSemester semester, CourseComment cComment)
        {
            semester.CourseComments.Insert(0, cComment);
            var ratings = new List<int>();
            for (var i = 0; i < cComment.CriteriaRatings.Count; i++)
            {
                ratings.Add(cComment.CriteriaRatings[i].Rating);
            }
            AverageCriteriaRatings.AddRatings(ratings);
            Score = AverageCriteriaRatings.GetAverageOfRatings();
        }

        private void SetupCourse() {
            CourseInSemesters = new List<CourseInSemester>();
            AverageCriteriaRatings = new AverageRatings(CourseComment.GetCourseCommentCriterias().Count);
        }
    }
}
