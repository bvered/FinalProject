﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class TeacherComment : Comment
    {
        [DataMember]
        public Teacher Teacher { get; set; }
        [DataMember]
        public IList<TeacherCriteriaRating> CriteriaRatings { get; set; }

        public TeacherComment()
        {
            CriteriaRatings = teachersDefaultCriterias();
        }

        public TeacherComment(string commentText, Teacher teacher) :
            base(commentText)
        {
            Teacher = teacher;
            CriteriaRatings = teachersDefaultCriterias();
        }

        public TeacherComment(string commentText, Teacher teacher, List<int> ratings) :
            base(commentText)
        {
            Teacher = teacher;
            CriteriaRatings = teachersDefaultCriterias();
            populateRatings(ratings);
        }

        static internal List<TeacherCriteriaRating> teachersDefaultCriterias() {
            List<TeacherCriteriaRating> criterias = new List<TeacherCriteriaRating>();
            foreach(string criteriaDisplayName in CriteriaList()) {
                TeacherCriteriaRating rating = new TeacherCriteriaRating(criteriaDisplayName, 0);
                criterias.Add(rating);
            }

            return criterias;
        }

        static internal List<string> CriteriaList() {
            return new List<string> {
                "Student- teacher relationship",
                "Teaching ability",
                "Teachers knowlegde level",
                "The teacher Encouregment for self learning",
                "The teacher interest level"
            };
        }

        private void populateRatings(List<int> ratings) {
            for (int i = 0; i < CriteriaRatings.Count; i++) {
                CriteriaRatings[i].Rating = ratings[i];
            }
        }
    }
}