using System;

namespace Server.Models
{
    public class CourseCriteria : IPersistent
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }
}