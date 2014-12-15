using System;

namespace Server.Models
{
    public class Vote : IPersistent
    {
        public Guid Id { get; set; }
        public bool Liked { get; set; }
        public User User { get; set; }
        public Comment Comment { get; set; }

        public Vote()
        {
        }

        public Vote(bool liked, User user, Comment comment)
        {
            Id = new Guid();
            Liked = liked;
            User = user;
            Comment = comment;
        }
    }
}