using System;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class CourseCriteriaRating : IPersistent
    {
        public Guid Id { get; set; }
        public int Rating { get; set; }
        public CourseCriteria Criteria { get; set; }

        public CourseCriteriaRating() {
            Criteria = new CourseCriteria();
        }

        public CourseCriteriaRating(string i_DisplayName, int i_Rating) {
            Criteria = new CourseCriteria(i_DisplayName);
            Rating = i_Rating;
        }
    }
}