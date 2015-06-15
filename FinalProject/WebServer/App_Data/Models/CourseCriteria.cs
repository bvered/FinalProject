using System;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class CourseCriteria : IPersistent
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
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