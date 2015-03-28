using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class User : IPersistent
    {
        public Guid Id { get; set; }
        public string LoginEmail { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string FacebookUserId { get; set; }
        public bool DefaultPublishRights { get; set; }
        public int Score { get; set; }
        public IList<Comment> Comments { get; set; }
        public IList<Vote> Votes { get; set; }
        
        public User()
        {
            Comments = new List<Comment>();
            Votes = new List<Vote>();
        }

        public void addComment(Comment comment)
        {
            Comments.Add(comment);
        }

        public void addVote(Vote vote)
        {
            Votes.Add(vote);
        }
    }
}