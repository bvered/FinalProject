using System;
using System.Collections.Generic;
using System.Web.Http;
using WebServer.App_Data;
using WebServer.App_Data.Models;

namespace WebServer.Controllers
{
    public class CoursesController : ApiController
    {
        [HttpGet]
        [ActionName("GetAllCourses")]
        public IList<Course> GetAllCourses()
        {
            using (var session = DBHelper.OpenSession())
            {
                return session.QueryOver<Course>().List();
            }
        }


        [HttpGet]
        [ActionName("GetAllSearchedCourses")]
        public IList<resultCourse> GetAllSearchedCourses()
        {
            using (var session = DBHelper.OpenSession())
            {
                IList<Course> courses = session.QueryOver<Course>().List();
                IList<resultCourse> result = new List<resultCourse>();
                foreach (var course in courses)
                {
                    result.Add(new resultCourse(course.Id, course.Name, course.University.Name, course.Faculty.Name));
                }

                return result;
            }
        }

        public class resultCourse
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string University { get; set; }
            public string Faculty { get; set; }

            public resultCourse(Guid id, string name, string university, string faculty)
            {
                Id = id;
                Name = name;
                University = university;
                Faculty = faculty;
            }
        }


        public IHttpActionResult GetCourse([FromUri]string id) {
            using (var session = DBHelper.OpenSession()) {
                Guid courseGuid;
                var didSucceedParsingGuid = Guid.TryParse(id, out courseGuid);
                Course course = null;
                if (didSucceedParsingGuid)
                {
                    course = session.Get<Course>(courseGuid);
                    if (course == null) {
                        return NotFound();
                    }
                }

                return Ok(course);
            }
        }

        [HttpGet]
        [ActionName("GetCourses")]
        public IList<string> GetAllCoursesNames()
        {
            using (var session = DBHelper.OpenSession())
            {
                return session.QueryOver<Course>().Select(x => x.Name).List<string>();
            }
        }

        [HttpPost]
        [ActionName("AddCourse")]
        public void AddCourse([FromBody]CreateCourseCommand createCommand) {
            using (var session = DBHelper.OpenSession())
            using (var transaction = session.BeginTransaction()) {
                var university = session.QueryOver<University>().Where(x => x.Name == createCommand.UniversityName).SingleOrDefault();
                var teacher = session.QueryOver<Teacher>().Where(x => x.Name == createCommand.TeacherName).SingleOrDefault();
                var course = new Course {
                    Name = createCommand.Name,
                    University = university,
                    Teachers = new []{teacher}
                };
                session.Save(course);
                transaction.Commit();
            }
        }

        [HttpGet]
        [ActionName("GetCriterias")]
        public IList<string> GetAllCriterias()
        {
            return CourseComment.CriteriaList();
        }

        [HttpPost]
        [ActionName("AddComment")]
        public IHttpActionResult AddComment([FromBody]CreateCourseComment comment)
        {
            CourseComment newComment = null;
            using (var session = DBHelper.OpenSession())
            using (var transaction = session.BeginTransaction()) {
                Guid semesterGuid;
                var didSemesterGuidParseSucceed = Guid.TryParse(comment.SemseterId, out semesterGuid);
                var semester = didSemesterGuidParseSucceed ? session.Load<CourseInSemester>(semesterGuid) : null;
                Guid courseGuid;
                var didCourseGuidParseSucceed = Guid.TryParse(comment.Id, out courseGuid);
                var course = didCourseGuidParseSucceed ? session.Load<Course>(courseGuid) : null;
                if (course != null) {
                    List<int> ratings = new List<int>();
                    foreach (var rating in comment.Ratings) {
                        ratings.Add(Convert.ToInt32(rating));
                    }
                    newComment = new CourseComment(comment.Comment, course, semester, ratings);
                    course.AddCourseComment(newComment);
                }
                session.Save(course);
                transaction.Commit();
            }
            if (newComment == null) { return NotFound(); } else { return Ok(newComment); }
        }

        [HttpGet]
        [ActionName("GetCommentById")]
        public IHttpActionResult GetCommentById([FromUri]string commentId) {
            using (var session = DBHelper.OpenSession()) {
                Guid courseCommentGuid;
                var didSuccedParsingCourseCommentGuid = Guid.TryParse(commentId, out courseCommentGuid);
                var comment = didSuccedParsingCourseCommentGuid ? session.Load<CourseComment>(courseCommentGuid) : null;
                if (comment == null) {
                    return NotFound();
                }

                return Ok(comment);
            }
        }

        [HttpGet]
        [ActionName("GetAllSemesters")]
        public IHttpActionResult GetAllSemesters()
        {
            using (var session = DBHelper.OpenSession())
            {
                var comment = session.QueryOver<CourseInSemester>().List();
                if (comment == null)
                {
                    return NotFound();
                }
                return Ok(comment);
            }
        }
    }

    public class CreateCourseCommand {
        public string Name { get; set; }
        public string UniversityName { get; set; }
        public string TeacherName { get; set; }
    }

    public class CreateCourseComment {
        public string Id { get; set; }
        public List<string> Ratings { get; set; }
        public string Comment { get; set; }
        public string SemseterId { get; set; }
    }
}
