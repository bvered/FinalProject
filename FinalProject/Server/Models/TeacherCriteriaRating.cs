using System;

namespace Server.Models
{
    public class TeacherCriteriaRating : IPersistent
    {
        public Guid Id { get; set; }

        public int Rating { get; set; }
        public TeacherCriteria Criteria { get; set; }
    }
}