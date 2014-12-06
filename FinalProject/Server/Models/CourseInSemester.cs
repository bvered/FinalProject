using System;

namespace Server.Models
{
    public class CourseInSemester : IPersistent
    {
        public Guid Id { get; set; }
        public Semester Semester { get; set; }
        public int Year { get; set; }
        public string Syllabus { get; set; }
        public byte[] GradesDistribution { get; set; }
    }
}