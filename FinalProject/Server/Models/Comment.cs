using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class Comment : IPersistent
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public DateTime DateTime { get; set; }
        public IList<Vote> Votes { get; set; }
        public int Reports { get; set; }
        public int Rating { get; set; }
        public string CommentText { get; set; }

        public Comment()
        {
        }

        public Comment(User user, string commentText)
        {
            Id = new Guid();
            User = user;
            DateTime = DateTime.Now;
            Votes= new List<Vote>();
            Reports = 0;
            Rating = 0;
            CommentText = commentText;
        }

        public void AddVote(Vote vote)
        {
            Votes.Add(vote);
        }
    }
}