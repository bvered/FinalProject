using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

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
        public int Room { get; set; }
        [DataMember]
        public University University { get; set; }
        [DataMember]
        public string Cellphone { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public IList<TeacherComment> TeacherComments { get; set; }
        [DataMember]
        AverageRatings AverageCriteriaRatings { get; set; }

        public Teacher()
        {
            SetupTeacher();
        }

        public Teacher(string name, int room,string phone, string email)
        {
            SetupTeacher();
            Name = name;
            Room = room;
            Cellphone = phone;
            Email = email;
        }

        public void AddTeacherCommnet(TeacherComment tComment)
        {
            TeacherComments.Insert(0, tComment);
            var ratings = tComment.CriteriaRatings.Select(t => t.Rating).ToList();
            Score = AverageCriteriaRatings.AverageRatingsList.Sum() / TeacherComments.Count; ;
        }

        public void SetupTeacher() {
            TeacherComments = new List<TeacherComment>();
            AverageCriteriaRatings = new AverageRatings(TeacherComment.GetTeacherCommentCriterias().Count);
        }
    }
}