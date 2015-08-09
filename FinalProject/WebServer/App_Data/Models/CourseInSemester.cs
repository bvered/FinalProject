using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WebServer.App_Data.Models.Enums;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class CourseInSemester : IPersistent
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public Course Course { get; set; }
        [DataMember]
        public IList<Teacher> Teachers { get; set; }
        [DataMember]
        public Semester Semester { get; set; }
        [DataMember]
        public int Year { get; set; }
        [DataMember]
        public IList<CourseComment> CourseComments { get; set; }
        [DataMember]
        public Syllabus Syllabus { get; set; }
        [DataMember]
        public GradesDestribution GradesDestribution { get; set; }

        public CourseInSemester()
        {
            CourseComments = new List<CourseComment>();
            Teachers = new List<Teacher>();
        }

        public CourseInSemester(Semester semester, int year)
        {
            Id = new Guid();
            Semester = semester;
            Year = year;
            Teachers = new List<Teacher>();
        }

    }
}