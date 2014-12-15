using System;

namespace Server.Models
{
    public class Faculty: IPersistent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
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