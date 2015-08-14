using System;

namespace WebServer.App_Data.Models
{
    public class University : IPersistent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] BackgroundImage { get; set; }
        public string Acronyms { get; set; }
        public string SiteAddress { get; set; }
    }
}