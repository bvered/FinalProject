using System;

namespace Server.Models
{
    public class Vote : IPersistent
    {
        public Guid Id { get; set; }
        public bool Liked { get; set; }
        public User User { get; set; }
        public Comment Comment { get; set; }
    }
}