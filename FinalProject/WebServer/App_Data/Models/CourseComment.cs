using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class CourseComment : Comment
    {
        [DataMember]
        public IList<CourseCriteriaRating> CriteriaRatings { get; set; }

        public CourseComment()
        {
            CriteriaRatings = new List<CourseCriteriaRating>();
        }
    }
}