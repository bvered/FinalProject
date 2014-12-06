using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class University : IPersistent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Course> Courses { get; set; }
        public List<Faculty> Faculties { get; set; }
    }
}