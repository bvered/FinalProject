using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class Teacher : IPersistent
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Score { get; set; }
        [DataMember]
        private int AmountOfRating { get; set; }
        [DataMember]
        private int SumOfRating { get; set; }
        [DataMember]
        public int Room { get; set; }
        [DataMember]
        public string Cellphone { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public IList<TeacherComment> TeacherComments { get; set; }

        public Teacher()
        {
            TeacherComments = new List<TeacherComment>();
        }

        public Teacher(string name, int room,string phone, string email)
        {
            Name = name;
            Room = room;
            Cellphone = phone;
            Email = email;
            TeacherComments = new List<TeacherComment>();
        }

        public void addTeacherCommnet(TeacherComment tComment)
        {
            TeacherComments.Insert(0, tComment);
            AmountOfRating++;
            SumOfRating += tComment.GetCriteriaRatingSummed();
            Score = SumOfRating / AmountOfRating;
        }
    }
}