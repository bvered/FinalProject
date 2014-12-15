using System;

namespace Server.Models
{
    public class CourseCriteriaRating : IPersistent
    {
        public Guid Id { get; set; }
        public int Rating { get; set; }
        public CourseCriteria Criteria { get; set; }

        public CourseCriteriaRating()
        {
        }
    }
}