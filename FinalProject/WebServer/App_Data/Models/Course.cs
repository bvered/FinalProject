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
        public Guid Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public Faculty Faculty { get; set; }
        public bool IsMandatory { get; set; }
        public AcademicDegree AcademicDegree { get; set; }
        public IntendedYear IntendedYear { get; set; }
        
        
        public IList<CourseInSemester> CourseInSemesters { get; set; }

        public Course()
        {
            CourseInSemesters = new List<CourseInSemester>();
        }

        public Course(int courseId, string name, Faculty faculy)
        {
            Id= new Guid();
            CourseId = courseId;
            Name = name;
            Faculty = faculy;
            CourseInSemesters = new List<CourseInSemester>();

        }

        public List<Teacher> GetTeachers()
        {
            return CourseInSemesters.Select(x => x.Teacher).Distinct().ToList();
        }

        public void AddCourseInSemester(CourseInSemester semester)
        {
            CourseInSemesters.Add(semester);
        }
    }
}
