using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class Course : IPersistent
    {
        public Guid Id { get; set; }
        public University University { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public Faculty Faculty { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<CourseInSemester> CourseInSemesters { get; set; }
        public List<CourseComment> CourseComments { get; set; }
    }
}
