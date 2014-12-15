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

        public CourseInSemester()
        {
        }

        public CourseInSemester(Semester semester, int year)
        {
            Id = new Guid();
            Semester = semester;
            Year = year;
        }

        public void AddSyllabus(string syllabus)
        {
            Syllabus = syllabus;
        }

        public void AddGrades(byte[] grades)
        {
            GradesDistribution = grades;
        }
    }
}