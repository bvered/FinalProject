using System;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class CourseCriteriaRating : IPersistent
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public int Rating { get; set; }
        [DataMember]
        public CourseCriteria Criteria { get; set; }

        public CourseCriteriaRating()
        {

        }

        public CourseCriteriaRating(string i_DisplayName, int i_Rating)
        {
            Criteria = new CourseCriteria(i_DisplayName);
            Rating = i_Rating;
        }
    }
}