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
    }
}