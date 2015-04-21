using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class University : IPersistent
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public IList<Teacher> Teachers { get; set; }
        [DataMember]
        public IList<Course> Courses { get; set; }
        [DataMember]
        public IList<Faculty> Faculties { get; set; }

        public University()
        {
            Teachers = new List<Teacher>();
            Courses = new List<Course>();
            Faculties = new List<Faculty>();
        }

        public University(string name)
        {
            Id = new Guid();
            Name = name;
            Teachers = new List<Teacher>();
            Courses = new List<Course>();
            Faculties = new List<Faculty>();
        }

        public void addTeacher(Teacher teacher)
        {
            Teachers.Add(teacher);
        }

        public void addCourse(Course course)
        {
            Courses.Add(course);
        }

        public void addFaculty(Faculty faculty)
        {
            Faculties.Add(faculty);
        }
    }
}