using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class TeacherComment : Comment
    {
        [DataMember]
        public IList<TeacherCriteriaRating> CriteriaRatings { get; set; }

        public TeacherComment()
        {
            CriteriaRatings = new List<TeacherCriteriaRating>();
        }
    }
}