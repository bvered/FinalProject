using System.Collections.Generic;

namespace Server.Models
{
    public class TeacherComment : Comment
    {
        public Teacher Teacher { get; set; }
        public Dictionary<TeacherCriteria, int> CriteriaRatings { get; set; }
    }
}