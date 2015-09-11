using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WebServer.App_Data.Models.Enums;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class Teacher : IPersistent
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public int TeacherId { get; set; }
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
        public AverageRatings AverageCriteriaRatings { get; set; }

        public IList<Faculty> Faculties { get; set; }

        public Teacher()
        {
            Faculties = new List<Faculty>();
            TeacherComments = new List<TeacherComment>();
            AverageCriteriaRatings = new AverageRatings(TeacherComment.GetTeacherCommentCriterias().Count);
        }

        public Teacher(int teacherId, string name, int room,string phone, string email, University university) : this()
        {
            TeacherId = teacherId;
            Name = name;
            Room = room;
            Cellphone = phone;
            Email = email;
            University = university;
        }

        public void AddTeacherCommnet(TeacherComment tComment)
        {
            TeacherComments.Insert(0, tComment);
            List<int> ratings = new List<int>();
            for (int i = 0; i < tComment.CriteriaRatings.Count; i++) {
                ratings.Add(tComment.CriteriaRatings[i].Rating);
            }
            AverageCriteriaRatings.AddRatings(ratings);
            Score = AverageCriteriaRatings.GetAverageOfRatings();
         }
    }
}