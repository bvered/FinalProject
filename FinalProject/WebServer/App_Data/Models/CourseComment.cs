using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]
    public class CourseComment : Comment
    {
        public Course Course { get; set; }
        public CourseInSemester CourseInSemester { get; set; }
        public IList<CourseCriteriaRating> CriteriaRatings { get; set; }

        public CourseComment()
        {
            CriteriaRatings = coursesDefaultCriterias();
        }

        public CourseComment(string commentText, Course course, CourseInSemester courseInSemester, List<int> ratings) :
            base(commentText)
        {
            Course = course;
            CourseInSemester = courseInSemester;
            CriteriaRatings = coursesDefaultCriterias();
            populateRatings(ratings);
        }

        static internal List<CourseCriteriaRating> coursesDefaultCriterias() {
            List<CourseCriteriaRating> criterias = new List<CourseCriteriaRating>();
            foreach (string criteriaDisplayName in CriteriaList()) {
                CourseCriteriaRating rating = new CourseCriteriaRating(criteriaDisplayName, 0);
                criterias.Add(rating);
            }

            return criterias;
        }

        static internal List<string> CriteriaList() {
            return new List<string> {
                "Course Critera 1",
                "Course Critera 2",
                "Course Critera 3",
                "Course Critera 4",
                "Course Critera 5"
            };
        }

        private void populateRatings(List<int> ratings) {
            for (int i = 0; i < CriteriaRatings.Count; i++) {
                CriteriaRatings[i].Rating = ratings[i];
            }
        }
    }
}