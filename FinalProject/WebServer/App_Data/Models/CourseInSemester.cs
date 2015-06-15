using System;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class CourseInSemester : IPersistent
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public Semester Semester { get; set; }
        [DataMember]
        public int Year { get; set; }
        [DataMember]
        public string Syllabus { get; set; }
        [DataMember]
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