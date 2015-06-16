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

        public Faculty()
        {
        }

        public Faculty(string name)
        {
            Id = new Guid();
            Name = name;
        }
    }
}