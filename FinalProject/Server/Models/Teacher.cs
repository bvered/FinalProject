using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class Teacher : IPersistent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<University> Universities { get; set; }
        public List<Course> Courses { get; set; }
        public List<TeacherComment> TeacherComments { get; set; }
    }
}