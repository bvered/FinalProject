using System.Collections.Generic;

namespace Server.Models
{
    public class TeacherComment : Comment
    {
        public Teacher Teacher { get; set; }
        public IList<TeacherCriteriaRating> CriteriaRatings { get; set; }

        public TeacherComment()
        {
            CriteriaRatings = new List<TeacherCriteriaRating>();
        }

        public TeacherComment(User user, string commentText, Teacher teacher) :
            base(user, commentText)
        {
            Teacher = teacher;
            CriteriaRatings = new List<TeacherCriteriaRating>();
        }
    }
}