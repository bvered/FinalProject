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
            var sum = 0;
            foreach (var rating in CriteriaRatings)
            {
                sum = rating.Rating;
            }

            return sum;
        }

        public static List<string> GetCourseCommentCriterias()
        {
            return new List<string>
            {
                "קלות החומר",
                "השקעה נדרשת בשיעורי בית",
                "ממוצע ציונים",
                "חשיבות אקדמית",
                "חשיבות מקצועית",
                "עניין בקורס",
            };
        }
    }
}