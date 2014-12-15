using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;

namespace Server.Models
{
    public class Teacher : IPersistent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public IList<University> Universities { get; set; }
        public IList<Course> Courses { get; set; }
        public IList<TeacherComment> TeacherComments { get; set; }

        public Teacher()
        {
            Universities = new List<University>();
            Courses = new List<Course>();
            TeacherComments = new List<TeacherComment>();
        }

        public Teacher(string name)
        {
            Id = new Guid();
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