using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using NHibernate.Linq;
using WebServer.App_Data;
using WebServer.App_Data.Models;


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
        public IList<string> GetAllTeachersNames([FromUri] string UnvierstiryName)
        {
            using (var session = DBHelper.OpenSession())
            {
                return session.Query<Teacher>().Where(x => x.University.Name == UnvierstiryName).Select(x => x.Name).ToList();
            }
        }

        [HttpGet]
        [ActionName("GetCommentById")]
        public IHttpActionResult GetCommentById([FromUri]string commentId) {
            using (var session = DBHelper.OpenSession()) {
                Guid teacherCommentGuid;
                var didSuccedParsingTeacherCommentGuid = Guid.TryParse(commentId, out teacherCommentGuid);
                var comment = session.Load<TeacherComment>(teacherCommentGuid);
                if (comment == null)
                    return NotFound();

                return Ok(comment);
            }
        }

        [HttpPost]
        [ActionName("AddTeacher")]
        public void AddTeacher([FromBody]CreateTeacherCommand createCommand)
        {
            using (var session = DBHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var teacher = new Teacher
                {
                    Name = createCommand.Name,
                    Room = createCommand.Room,
                    Cellphone = createCommand.Cellphone,
                    Email = createCommand.Email,
                };
                session.Save(teacher);
                transaction.Commit();
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

                var teacherCriterias = session.QueryOver<TeacherCriteria>().List();

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
        public IHttpActionResult GetAllCriterias() {
            using (var session = DBHelper.OpenSession())
            {
                return Ok(TeacherComment.GetTeacherCommentCriterias());
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

    public class CreateTeacherCommand {
        public string Name { get; set; }
        public int Room { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
    }

    public class CreateTeacherComment {
        public string Id { get; set; }
        public List<int> Ratings { get; set; }
        public string Comment { get; set; }
    }

    public class VoteCommand
    {
        public string commentId { get; set; }
        public bool Liked { get; set; }
    }
}
