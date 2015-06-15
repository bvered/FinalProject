using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class Vote : IPersistent
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public bool Liked { get; set; }
        [DataMember]
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