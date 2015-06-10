using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]  
    public class Comment : IPersistent
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public DateTime DateTime { get; set; }
        [DataMember]
        public IList<Vote> Votes { get; set; }
        [DataMember]
        public int Reports { get; set; }
        [DataMember]
        public int Rating { get; set; }
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