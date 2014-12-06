using System.Collections.Generic;

namespace Server.Models
{
    public class CourseComment : Comment
    {
        public Course Course { get; set; }
        public CourseInSemester CourseInSemester { get; set; }
        public Dictionary<CourseCriteria, int> CriteriaRatings { get; set; }
    }
}