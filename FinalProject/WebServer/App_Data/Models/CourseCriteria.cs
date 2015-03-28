using System;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class CourseCriteria : IPersistent
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }

        public CourseCriteria()
        {
        }

        public CourseCriteria(string name)
        {
            Id = new Guid();
            DisplayName = name;
        }
    }
}