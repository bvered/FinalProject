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

        public int GetCriteriaRatingSummed()
        {
            int sum = 0;
            foreach (CourseCriteriaRating rating in CriteriaRatings)
            {
                sum = rating.Rating;
            }

            return sum;
        }

        public static List<string> GetCourseCommentCriterias()
        {
            return new List<string>
            {
                "Material ease",
                "Time investment for home-work",
                "Number of home-work submissions",
                "Time investment for test learning",
                "Course usability",
                "Course grades average",
                "Does the attendance is mandatory",
                "Does the test has open material/reference Pages",
            };
        }
    }
}