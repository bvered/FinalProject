﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class University : IPersistent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<Teacher> Teachers { get; set; }
        public IList<Course> Courses { get; set; }
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