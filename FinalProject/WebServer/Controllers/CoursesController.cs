using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using NHibernate.Linq;
using WebServer.App_Data;
using WebServer.App_Data.Models;
using WebServer.App_Data.Models.Enums;

namespace WebServer.Controllers
{
    public class CoursesController : ApiController
    {
        [HttpGet]
        [ActionName("GetAllSearchedCourses")]
        public IList<ResultCourse> GetAllSearchedCourses([FromUri]string id)
        {
            using (var session = DBHelper.OpenSession())
            {
                var courses = session.QueryOver<Course>().List();
                IList<ResultCourse> result = new List<ResultCourse>();

                foreach (var course in courses)
                {
                    if (id == course.Name || (course.Name).IndexOf(id) >= 0 || ((course.Name).ToLower()).IndexOf(id) >= 0 || ((course.Name).ToLower()).IndexOf(id.ToLower()) >= 0)
                    {
                        var year = course.IntendedYear.ToString();
                        var returnYear = "";
                        switch (year)
                        {
                            case "Any":
                                returnYear = "כל שנה";
                                break;
                            case "First":
                                returnYear = "שנה ראשונה";
                                break;
                            case "Second":
                                returnYear = "שנה שניה";
                                break;
                            case "Third":
                                returnYear = "שנה שלישית";
                                break;
                            case "Forth":
                                returnYear = "שנה רביעית";
                                break;
                        }


                        result.Add(new ResultCourse(course.Id, course.Name, course.Faculty.ToString(), course.Score, course.IsMandatory, returnYear));
                    }
                }
                return result;
            }
        }

        public class ResultCourse
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Faculty { get; set; }
            public int Score { get; set; }
            public bool IsMandatory { get; set; }
            public string Year { get; set; }


            public ResultCourse(Guid id, string name, string faculty, int score, bool isMandatory, string year)
            {
                Id = id;
                Name = name;
                Faculty = faculty;
                Score = score;
                IsMandatory = isMandatory;
                Year = year;
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
        [ActionName("GetCoursesNames")]
        public IList<string> GetAllCoursesNames([FromUri] string id)
        {
            using (var session = DBHelper.OpenSession())
            {
                return session.Query<Course>().Where(x => x.University.Acronyms == id).Select(x => x.Name).ToList();
            }
        }

        [HttpPost]
        [ActionName("AddCourse")]
        public IHttpActionResult AddCourse([FromBody] CreateCourseCommand createCommand)
        {
            using (var session = DBHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Course>().Where(x => x.Name == createCommand.Name).ToList();
                if (query.Count != 0)
                {
                    const HttpStatusCode MyStatusCode = (HttpStatusCode) 222;
                    return Content(MyStatusCode, query.Select(x => x.Id).FirstOrDefault());
                }
                var course = new Course
                    {
                        University = session.Query<University>().SingleOrDefault(x => x.Name == createCommand.University),
                        Name = createCommand.Name,
                        Faculty = createCommand.Faculty,
                        IsMandatory = createCommand.IsMandatory,
                        AcademicDegree = createCommand.AcademicDegree,
                        IntendedYear = createCommand.IntendedYear,
                    };

                course.AddCourseInSemester(new CourseInSemester
                {
                    Course = course,
                    Teacher = session.Query<Teacher>().SingleOrDefault(x => x.Name == createCommand.TeacherName),
                });
                session.Save(course);
                transaction.Commit();

                return Ok(course.Id);
            }
        }

        [HttpGet]
        [ActionName("GetCriterias")]
        public IList<string> GetAllCriterias()
        {
            using (var session = DBHelper.OpenSession())
            {
                return CourseComment.GetCourseCommentCriterias();
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
                var courseCriterias = CourseComment.GetCourseCommentCriterias();
                var ratings = new List<CourseCriteriaRating>();
                var newComment = new CourseComment
                {
                    CommentText = comment.Comment,
                    CriteriaRatings = ratings,
                    DateTime = DateTime.Now
                };
                for (int index = 0; index < courseCriterias.Count; index++)
                {
                    newComment.CriteriaRatings.Add(new CourseCriteriaRating(courseCriterias[index], comment.Ratings[index]));
                }
                session.Save(newComment);
                courseInSemester.Course.AddCourseCommnet(courseInSemester, newComment);
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

        [HttpPost]
        [ActionName("AddVote")]
        public IHttpActionResult Vote([FromBody] VoteCommand vote)
        {
            using (var session = DBHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                Guid commentId;
                if (!Guid.TryParse(vote.commentId, out commentId))
                {
                    return NotFound();
                }
                var comment = session.Get<CourseComment>(commentId);
                if (comment == null) return NotFound();
                var v = new Vote(vote.Liked);
                session.Save(v);
                comment.AddVote(v);
                session.Save(comment);
                transaction.Commit();
                return Ok(vote.Liked ? comment.TotalNumberOfLikes : comment.TotalNumberOfDislikes);
            }
        }

        [HttpPost]
        [ActionName("GetCommentsForCourse")]
        public IHttpActionResult GetCommentsForCourse([FromBody] CourseCommentRequest commentRequest)
        {
            using (var session = DBHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var courseComments = new List<CourseComment>();
                Guid courseId, teacherId;
                Course course;
                if (!Guid.TryParse(commentRequest.courseId, out courseId))
                {
                    return NotFound();
                }
                course = session.Get<Course>(courseId);
                if (course == null) {
                    return NotFound();
                }
                Guid.TryParse(commentRequest.teacherId, out teacherId);
                var teacher = session.Get<Teacher>(teacherId);
                foreach (var courseInSemester in course.CourseInSemesters)
                {
                    if ((teacher != null) && (courseInSemester.Teacher != teacher))
                    {
                        continue;
                    }
                    if ((commentRequest.year != CourseCommentRequest.kNoInfoProvided) && (courseInSemester.Year != commentRequest.year))
                    {
                        continue;
                    }
                    if (((int)commentRequest.semester != CourseCommentRequest.kNoInfoProvided) && (courseInSemester.Semester != commentRequest.semester))
                    {
                        continue;
                    }
                    courseComments.AddRange(courseInSemester.CourseComments);
                }
                if (commentRequest.sortNew)
                {
                    courseComments.OrderByDescending(x => x.DateTime);
                }
                return Ok(courseComments);
                //return Ok(courseComments.Skip(CourseCommentRequest.kNoOfCommentsPerPage * request.page).Take(CourseCommentRequest.kNoOfCommentsPerPage));
            }
        }
    }


    public class CreateCourseCommand
    {
        public string University { get; set; }
        public string Name { get; set; }
        public string TeacherName { get; set; }
        public Faculty Faculty { get; set; }
        public bool IsMandatory { get; set; }
        public AcademicDegree AcademicDegree { get; set; }
        public IntendedYear IntendedYear { get; set; }
    }

    public class CreateCourseComment
    {
        public string Id { get; set; }
        public List<int> Ratings { get; set; }
        public string Comment { get; set; }
        public string SemseterId { get; set; }
    }

    public class CourseCommentRequest
    {
        public string courseId { get; set; }
        public string teacherId { get; set; }
        public int year { get; set; }
        public Semester semester { get; set; }
        public int page { get; set; }
        public bool sortNew { get; set; }

        public const int kNoInfoProvided = -1;
        public const int kNoOfCommentsPerPage = 5;
    }
}