using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using NHibernate.Linq;
using WebServer.App_Data;
using WebServer.App_Data.Models;

namespace WebServer.Controllers
{
    public class CoursesController : ApiController
    {
        [HttpGet]
        [ActionName("GetAllSearchedCourses")]
        public IList<ResultCourse> GetAllSearchedCourses()
        {
            using (var session = DBHelper.OpenSession())
            {
                return
                    session.Query<Course>()
                        .Select(x => new ResultCourse(x.Id, x.Name, x.Faculty.Name, x.Score))
                        .ToList();
            }
        }

        public class ResultCourse
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Faculty { get; set; }
            public int Score { get; set; }

            public ResultCourse(Guid id, string name, string faculty, int score)
            {
                Id = id;
                Name = name;
                Faculty = faculty;
                Score = score;
            }
        }


        public IHttpActionResult GetCourse([FromUri] string id)
        {
            using (var session = DBHelper.OpenSession())
            {
                Guid courseGuid;

                if (!Guid.TryParse(id, out courseGuid))
                {
                    return NotFound();
                }

                var course = session.Get<Course>(courseGuid);

                if (course != null)
                {
                    return Ok(course);
                }

                return NotFound();
            }
        }

        [HttpGet]
        [ActionName("GetCourses")]
        public IList<string> GetAllCoursesNames()
        {
            using (var session = DBHelper.OpenSession())
            {
                return session.Query<Course>().Select(x => x.Name).ToList();
            }
        }

        [HttpPost]
        [ActionName("AddCourse")]
        public void AddCourse([FromBody] CreateCourseCommand createCommand)
        {
            using (var session = DBHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                //TODO: Fill all fields
                var course = new Course
                {
                    Name = createCommand.Name,
                };

                course.AddCourseInSemester(new CourseInSemester
                {
                    Course = course
                });
                session.Save(course);
                transaction.Commit();
            }
        }

        [HttpGet]
        [ActionName("GetCriterias")]
        public IList<string> GetAllCriterias()
        {
            using (var session = DBHelper.OpenSession())
            {
                return session.Query<CourseCriteria>().Select(x => x.DisplayName).ToList();
            }
        }

        [HttpPost]
        [ActionName("AddComment")]
        public IHttpActionResult AddComment([FromBody] CreateCourseComment comment)
        {
            using (var session = DBHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                Guid semesterGuid;
                var didSemesterGuidParseSucceed = Guid.TryParse(comment.SemseterId, out semesterGuid);

                var courseInSemester = didSemesterGuidParseSucceed ? session.Load<CourseInSemester>(semesterGuid) : null;

                if (courseInSemester == null)
                {
                    return NotFound();
                }

                var courseCriterias = session.QueryOver<CourseCriteria>().List();

                var ratings = new List<CourseCriteriaRating>();

                var newComment = new CourseComment
                {
                    CommentText = comment.Comment,
                    CriteriaRatings = ratings,
                    DateTime = DateTime.Now
                };

                for (int index = 0; index < comment.Ratings.Count; index++)
                {
                    newComment.CriteriaRatings.Add(new CourseCriteriaRating
                    {
                        Criteria = courseCriterias[index],
                        Rating = comment.Ratings[index]
                    });
                }

                courseInSemester.CourseComments.Add(newComment);
                session.Save(courseInSemester);

                transaction.Commit();

                return Ok(newComment);
            }
        }

        [HttpGet]
        [ActionName("GetCommentById")]
        public IHttpActionResult GetCommentById([FromUri] string commentId)
        {
            using (var session = DBHelper.OpenSession())
            {
                Guid courseCommentGuid;
                var didSuccedParsingCourseCommentGuid = Guid.TryParse(commentId, out courseCommentGuid);
                var comment = didSuccedParsingCourseCommentGuid ? session.Load<CourseComment>(courseCommentGuid) : null;
                if (comment == null)
                {
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

    public class CreateCourseCommand
    {
        public string Name { get; set; }
        public string TeacherName { get; set; }
    }

    public class CreateCourseComment
    {
        public string Id { get; set; }
        public List<int> Ratings { get; set; }
        public string Comment { get; set; }
        public string SemseterId { get; set; }
    }
}