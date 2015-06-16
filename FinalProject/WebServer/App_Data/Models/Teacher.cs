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
        public IList<TeacherComment> TeacherComments { get; set; }

        public Teacher()
        {
            TeacherComments = new List<TeacherComment>();
        }

        public Teacher(string name)
        {
            Name = name;
            TeacherComments = new List<TeacherComment>();
        }

        public void addTeacherCommnet(TeacherComment tComment)
        {
            TeacherComments.Add(tComment);
        }
    }
}