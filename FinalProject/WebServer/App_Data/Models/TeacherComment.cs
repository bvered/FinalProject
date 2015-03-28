using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
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