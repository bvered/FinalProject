using System;

namespace Server.Models
{
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