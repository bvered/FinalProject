using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]  
    public abstract class Comment : IPersistent
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public DateTime DateTime { get; set; }
        [DataMember]
        public IList<Vote> Votes { get; set; }

        [DataMember]
        public string CommentText { get; set; }

        public Comment()
        {
            DateTime = DateTime.Now;
            Votes = new List<Vote>();
        }

        public Comment(string commentText)
        {
            DateTime = DateTime.Now;
            Votes= new List<Vote>();
            CommentText = commentText;
        }

        public void AddVote(Vote vote)
        {
            Votes.Add(vote);
        }


    }
}