using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class Teacher : IPersistent
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Score { get; set; }
        [DataMember]
        public IList<University> Universities { get; set; }
        [DataMember]
        public IList<Course> Courses { get; set; }
        [DataMember]
        public IList<TeacherComment> TeacherComments { get; set; }

        public Teacher()
        {
            Universities = new List<University>();
            Courses = new List<Course>();
            TeacherComments = new List<TeacherComment>();
        }

        public Teacher(string name)
        {
            Name = name;
            Universities = new List<University>();
            Courses = new List<Course>();
            TeacherComments = new List<TeacherComment>();
        }

        public void addUniversity(University university)
        {
            Universities.Add(university);
        }

        public void addCourse(Course course)
        {
            Courses.Add(course);
        }

        public void addTeacherCommnet(TeacherComment tComment)
        {
            TeacherComments.Add(tComment);
        }
    }
}