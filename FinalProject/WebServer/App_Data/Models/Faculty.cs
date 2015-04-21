using System;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class Faculty: IPersistent
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public University University { get; set; }

        public Faculty()
        {
        }

        public Faculty(string name, University university)
        {
            Id = new Guid();
            Name = name;
            University = university;
        }
    }
}