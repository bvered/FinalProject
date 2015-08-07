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

        protected Comment()
        {
            MutualCommentConst();
        }

        protected Comment(string commentText)
        {
            MutualCommentConst();
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

        private void MutualCommentConst() {
            DateTime = DateTime.Now;
            Votes = new List<Vote>();
        }
    }
}