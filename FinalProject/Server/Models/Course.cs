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
        public int Score { get; set; }
        public Faculty Faculty { get; set; }
        public IList<Teacher> Teachers { get; set; }
        public IList<CourseInSemester> CourseInSemesters { get; set; }
        public IList<CourseComment> CourseComments { get; set; }

        public Course()
        {
            Teachers = new List<Teacher>();
            CourseInSemesters = new List<CourseInSemester>();
            CourseComments = new List<CourseComment>();
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
    }
}
