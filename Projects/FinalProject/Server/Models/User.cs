using System;
using System.Collections.Generic;

namespace Server.Models
{
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
        public List<Comment> Comments { get; set; }
        public List<Vote> Votes { get; set; }
    }
}