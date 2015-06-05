using System;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class TeacherCriteriaRating : IPersistent
    {
        public Guid Id { get; set; }

        public int Rating { get; set; }
        public TeacherCriteria Criteria { get; set; }

        public TeacherCriteriaRating()
        {
            Criteria = new TeacherCriteria();
        }

        public TeacherCriteriaRating(string i_DisplayName, int i_Rating)
        {
            Criteria = new TeacherCriteria(i_DisplayName);
            Rating = i_Rating;
        }
    }
}