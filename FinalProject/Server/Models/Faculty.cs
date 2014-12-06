using System;

namespace Server.Models
{
    public class Faculty: IPersistent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public University University { get; set; }
    }
}