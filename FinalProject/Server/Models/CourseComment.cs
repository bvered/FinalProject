using System.Collections.Generic;

namespace Server.Models
{
    public class CourseComment : Comment
    {
        public Course Course { get; set; }
        public CourseInSemester CourseInSemester { get; set; }
        public IList<CourseCriteriaRating> CriteriaRatings { get; set; }

        public CourseComment()
        {
            CriteriaRatings = new List<CourseCriteriaRating>();
        }

        public CourseComment(User user, string commentText, Course course, CourseInSemester courseInSemester) :
            base(user, commentText)
        {
            Course = course;
            CourseInSemester = courseInSemester;
            CriteriaRatings = new List<CourseCriteriaRating>();
        }
    }
}