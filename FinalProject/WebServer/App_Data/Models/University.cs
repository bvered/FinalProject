using System;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    public class University : IPersistent
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public byte[] BackgroundImage { get; set; }
        [DataMember]
        public string Acronyms { get; set; }
        [DataMember]
        public string SiteAddress { get; set; }
        [DataMember]
        public string FileExtention { get; set; }
    }
}