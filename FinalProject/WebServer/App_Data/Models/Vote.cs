using System;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class Vote : IPersistent
    {
        public Guid Id { get; set; }
        public bool Liked { get; set; }
        public Comment Comment { get; set; }

        public Vote()
        {
        }

        public Vote(bool liked, Comment comment)
        {
            Id = new Guid();
            Liked = liked;
            Comment = comment;
        }
    }
}