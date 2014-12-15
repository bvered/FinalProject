using System;

namespace Server.Models
{
    public class TeacherCriteria : IPersistent
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }

        public TeacherCriteria()
        {
        }

        public TeacherCriteria(string name)
        {
            Id = new Guid();
            DisplayName = name;
        }
    }
}