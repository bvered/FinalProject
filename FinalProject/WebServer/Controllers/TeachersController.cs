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
    public class TeachersController : ApiController
    {
        [HttpGet]
        [ActionName("GetAllTeachers")]
        public IList<Teacher> GetAllTeachers()
        {
            using (var session = DBHelper.OpenSession())
            {
                var teachers = session.QueryOver<Teacher>().List();
                return teachers;
            }
        }

        [HttpGet]
        [ActionName("GetAllTeacherNamesAndIds")]
        public IHttpActionResult GetAllTeacherNamesAndIdsByFaculty(string id)
        {
            using (var session = DBHelper.OpenSession())
            {
                int facultyNumber;
                Faculty faculty;
                if(!Int32.TryParse(id, out facultyNumber)) {
                    return NotFound();
                }
                if(Enum.IsDefined(typeof(Faculty), facultyNumber)) {
                    faculty = (Faculty)facultyNumber;
                } else {
                    return NotFound();
                }
                var teachers = session.Query<Teacher>()
                    .Where(x => x.Faculties.Contains(faculty))
                    .ToList();
                var teacherEssentials = teachers.Select(teacher => new TeacherEssentials
                {
                    TeacherId = teacher.Id.ToString(), TeacherName = teacher.Name
                }).ToList();
                return Ok(teacherEssentials);
            }

        }

        [HttpGet]
        [ActionName("GetTeacher")]
        public IHttpActionResult GetTeacher([FromUri]string id)
        {
            using (var session = DBHelper.OpenSession())
            {
                Guid teacherGuid;
                if (!Guid.TryParse(id, out teacherGuid))
                    return NotFound();

                var teacher = session.Get<Teacher>(teacherGuid);

                if (teacher == null)
                    return NotFound();

                return Ok(teacher);
            }
        }

        [HttpGet]
        [ActionName("GetTeachersNames")]
        public IList<string> GetAllTeachersNames([FromUri] string id)
        {
            using (var session = DBHelper.OpenSession())
            {
                var res = session.Query<Teacher>().Where(x => x.University.Acronyms == id).Select(x => x.Name).ToList();
                return res;
            }
        }

        [HttpGet]
        [ActionName("GetCommentById")]
        public IHttpActionResult GetCommentById([FromUri]string commentId)
        {
            using (var session = DBHelper.OpenSession())
            {
                Guid teacherCommentGuid;
                Guid.TryParse(commentId, out teacherCommentGuid);
                var comment = session.Load<TeacherComment>(teacherCommentGuid);
                if (comment == null)
                    return NotFound();

                return Ok(comment);
            }
        }

        [HttpPost]
        [ActionName("AddTeacher")]
        public IHttpActionResult AddTeacher([FromBody]CreateTeacherCommand createCommand)
        {
            using (var session = DBHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Teacher>().Where(x => x.Name == createCommand.Name).ToList();
                if (query.Count != 0)
                {
                    const HttpStatusCode MyStatusCode = (HttpStatusCode)222;
                    return Content(MyStatusCode, query.Select(x => x.Id).FirstOrDefault());
                }

                var teacher = new Teacher
                {
                    University = session.Query<University>().SingleOrDefault(x => x.Name == createCommand.University),
                    Name = createCommand.Name,
                    Room = createCommand.Room,
                    Cellphone = createCommand.Cellphone,
                    Email = createCommand.Email,
                };
                session.Save(teacher);
                transaction.Commit();

                return Ok(teacher.Id);
            }
        }

        [HttpPost]
        [ActionName("AddComment")]
        public IHttpActionResult AddComment([FromBody] CreateTeacherComment comment)
        {
            using (var session = DBHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                Guid teacherGuid;
                var didTeacherGuidParseSucceed = Guid.TryParse(comment.Id, out teacherGuid);
                var teacher = didTeacherGuidParseSucceed ? session.Load<Teacher>(teacherGuid) : null;

                if (teacher == null)
                {
                    return NotFound();
                }

                session.QueryOver<TeacherCriteria>().List();

                var newComment = new TeacherComment
                {
                    CommentText = comment.Comment,
                    DateTime = DateTime.Now,
                };
                var criteriasDisplayName = TeacherComment.GetTeacherCommentCriterias();
                for (var index = 0; index < criteriasDisplayName.Count; index++)
                {
                    newComment.CriteriaRatings.Add(new TeacherCriteriaRating(criteriasDisplayName[index], comment.Ratings[index]));
                }
                teacher.AddTeacherCommnet(newComment);
                session.Save(teacher);
                transaction.Commit();

                return Ok(newComment);
            }
        }

        [HttpGet]
        [ActionName("GetCriterias")]
        public IHttpActionResult GetAllCriterias()
        {
            using (DBHelper.OpenSession())
            {
                return Ok(TeacherComment.GetTeacherCommentCriterias());
            }
        }

        [HttpPost]
        [ActionName("GetSortedTeacherComments")]
        public IHttpActionResult GetSortedTeacherComments([FromBody] SortedTeacherComment sortBy)
        {
            using (var session = DBHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                IList<TeacherComment> sortedComments;
                Guid teacherGuid;
                Teacher teacher;
                var successfullteacherGuidParse = Guid.TryParse(sortBy.TeacherId, out teacherGuid);
                if (!successfullteacherGuidParse)
                {
                    return NotFound();
                }
                teacher = session.Get<Teacher>(teacherGuid);
                if (teacher == null)
                {
                    return NotFound();
                }
                sortedComments = teacher.TeacherComments;
                if (sortBy.sortByDate)
                {
                    return Ok(sortedComments.OrderByDescending(t => t.DateTime));
                }
                else if (sortBy.sortByLikes)
                {
                    return Ok(sortedComments.OrderByDescending(t => t.TotalNumberOfLikes));
                }
                return Ok(sortedComments);
            }

        }

        [HttpPost]
        [ActionName("GetTeacherCourses")]
        public IHttpActionResult GetTeacherCourses([FromUri]string id)
        {
            using (var session = DBHelper.OpenSession())
            {
               // Guid tGuid;
               // var parseSucced = Guid.TryParse(id, out tGuid);
               // //List<TeacherCoursesByTeacherId> teacherCourses =
               //List<TeacherCoursesByTeacherId> teacherCourses =
               //     session.Query<CourseInSemester>()
               //     .Where(x => x.Teacher.Id == tGuid)
               //     .Select(x => new TeacherCoursesByTeacherId
               //     {
               //         CourseId = x.Course.Id,
               //         CourseName = x.Course.Name,
               //     })
               //     .ToList();
                return Ok();
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

                var comment = session.Get<TeacherComment>(commentId);

                if (comment == null) return NotFound();
                var v = new Vote(vote.Liked);
                session.Save(v);
                comment.AddVote(v);
                session.Save(comment);
                transaction.Commit();
                return Ok(vote.Liked ? comment.TotalNumberOfLikes : comment.TotalNumberOfDislikes);
            }
        }
    }

    public class CreateTeacherCommand
    {
        public string University { get; set; }
        public string Name { get; set; }
        public int Room { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
    }

    public class CreateTeacherComment
    {
        public string Id { get; set; }
        public List<int> Ratings { get; set; }
        public string Comment { get; set; }
    }

    public class VoteCommand
    {
        public string commentId { get; set; }
        public bool Liked { get; set; }
    }

    public class TeacherEssentials
    {
        public string TeacherId { get; set; }
        public string TeacherName { get; set; }
    }

    public class SortedTeacherComment
    {
        public string TeacherId { get; set; }
        public bool sortByDate { get; set; }
        public bool sortByLikes { get; set; }
    }

    public class TeacherCoursesByTeacherId
    {
        public string CourseName { get; set; }
        public Guid CourseId { get; set; }
    }
}
