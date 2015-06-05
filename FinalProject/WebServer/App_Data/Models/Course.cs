using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class Course : IPersistent
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public University University { get; set; }
        [DataMember]
        public int CourseId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Score { get; set; }
        [DataMember]
        public Faculty Faculty { get; set; }
        [DataMember]
        public IList<Teacher> Teachers { get; set; }
        [DataMember]
        public IList<CourseInSemester> CourseInSemesters { get; set; }
        [DataMember]
        public IList<CourseComment> CourseComments { get; set; }
        [DataMember]
        public IList<Syllabus> Syllabuses { get; set; }
        [DataMember]
        public IList<GradesDestribution> GradesDestributions { get; set; }

        public Course()
        {
            Teachers = new List<Teacher>();
            CourseInSemesters = new List<CourseInSemester>();
            CourseComments = new List<CourseComment>();
            Syllabuses = new List<Syllabus>();
            GradesDestributions = new List<GradesDestribution>();
        }

        public Course(University university, int courseId, string name, Faculty faculy)
        {
            Id= new Guid();
            University = university;
            CourseId = courseId;
            Name = name;
            Faculty = faculy;
            Teachers = new List<Teacher>();
            CourseInSemesters = new List<CourseInSemester>();
            CourseComments = new List<CourseComment>();
        }

        public void AddTeacherToCourse(Teacher teacher)
        {
            Teachers.Add(teacher);
        }

        public void AddCourseInSemester(CourseInSemester semester)
        {
            CourseInSemesters.Add(semester);
        }

        public void AddCourseComment(CourseComment comment)
        {
            CourseComments.Add(comment);
        }

        public void AddSyllabus(Syllabus newSyllabus)
        {
            Syllabuses.Add(newSyllabus);
        }

        public void AddGradesDestribustion(GradesDestribution newGrades)
        {
            GradesDestributions.Add(newGrades);
        }
    }
}
