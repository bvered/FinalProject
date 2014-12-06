using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class Comment : IPersistent
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public DateTime DateTime { get; set; }
        public List<Vote> Votes { get; set; }
        public int Reports { get; set; }
        public int Rating { get; set; }
        public string CommentText { get; set; }
    }
}