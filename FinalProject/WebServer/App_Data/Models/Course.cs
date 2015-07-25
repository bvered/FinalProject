﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebServer.App_Data.Models.Enums;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class Course : IPersistent
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public int CourseId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Score { get; set; }
        [DataMember]
        private int AmountOfRating { get; set; }
        [DataMember]
        private int SumOfRating { get; set; }
        [DataMember]
        public Faculty Faculty { get; set; }
        [DataMember]
        public bool IsMandatory { get; set; }
        [DataMember]
        public AcademicDegree AcademicDegree { get; set; }
        [DataMember]
        public IntendedYear IntendedYear { get; set; }
        [DataMember]
        public IList<CourseInSemester> CourseInSemesters { get; set; }

        public Course()
        {
            CourseInSemesters = new List<CourseInSemester>();
        }

        public Course(int courseId, string name, Faculty faculy)
        {
            Id= new Guid();
            CourseId = courseId;
            Name = name;
            Faculty = faculy;
            CourseInSemesters = new List<CourseInSemester>();

        }

        public List<Teacher> GetTeachers()
        {
            return CourseInSemesters.Select(x => x.Teacher).Distinct().ToList();
        }

        public void AddCourseInSemester(CourseInSemester semester)
        {
            CourseInSemesters.Add(semester);
        }

        public void addCourseCommnet(CourseInSemester semester,CourseComment cComment)
        {
            semester.CourseComments.Insert(0, cComment);
            AmountOfRating++;
            SumOfRating += cComment.GetCriteriaRatingSummed();
            Score = SumOfRating / AmountOfRating;
        }
    }
}
