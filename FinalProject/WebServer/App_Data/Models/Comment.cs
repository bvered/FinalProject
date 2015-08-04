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
        [DataMember]
        public int TotalNumberOfLikes { get; set; }
        [DataMember]
        public int TotalNumberOfDislikes { get; set; }

        public Comment()
        {
            mutualCommentConst();
        }

        public Comment(string commentText)
        {
            mutualCommentConst();
            CommentText = commentText;
        }

        public void AddVote(Vote vote)
        {
            Votes.Add(vote);
            if (vote.Liked) {
                TotalNumberOfLikes++;
            } else {
                TotalNumberOfDislikes--;
            }
        }

        private void mutualCommentConst() {
            DateTime = DateTime.Now;
            Votes = new List<Vote>();
        }
    }
}