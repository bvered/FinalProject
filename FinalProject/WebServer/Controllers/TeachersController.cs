using System;
using System.Collections.Generic;
using System.Web.Http;
using NHibernate;
using WebServer.App_Data;
using WebServer.App_Data.Models;


namespace WebServer.Controllers
{
    public class TeachersController : ApiController
    {
        public IEnumerable<Teacher> GetAllTeachers()
        {
            using (var session = DBHelper.OpenSession())
            {
                return session.QueryOver<Teacher>().List();
            }
        }

        public IHttpActionResult GetTeacher(Guid id)
        {
            using (var session = DBHelper.OpenSession())
            {
                var teacher = session.Get<Teacher>(id);

                if (teacher == null)
                {
                    return NotFound();
                }

                return Ok(teacher);
            }
        }

        [HttpGet]
        [ActionName("GetTeachers")]
        public IList<string> GetAllTeachersNames()
        {
            using (var session = DBHelper.OpenSession())
            {
                return session.QueryOver<Teacher>().Select(x => x.Name).List<string>();
            }
        }

        [HttpPost]
        [ActionName("AddTeacher")]
        public void AddTeacher([FromBody]CreateTeacherCommand createCommand)
        {
            using (var session = DBHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var university = session.QueryOver<University>().Where(x => x.Name == createCommand.UniversityName).SingleOrDefault();

                var teacher = new Teacher
                {
                    Name = createCommand.Name,
                    Universities = new[] {university},
                };

                session.Save(teacher);

                transaction.Commit();
            }
        }

        [HttpPost]
        [ActionName("AddComment")]
        public void AddComment([FromBody]CreateTeacherComment comment)
        {
            using (var session = DBHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var teacher = session.QueryOver<Teacher>().Where(x => x.Id.ToString() == comment.Id).SingleOrDefault();

                var teacherComment = new TeacherComment(User, comment.Comment, teacher, comment.Ratings); // Why not?

                session.Save(teacher);

                transaction.Commit();
            }
        }

        [HttpGet]
        [ActionName("GetCriterias")]
        public IList<string> GetAllCriterias()
        {
            return TeacherComment.criteriaList();
        } 

    }

    public class CreateTeacherCommand
    {
        public string Name { get; set; }
        public string UniversityName { get; set; }
    }

    public class CreateTeacherComment
    {
        public string Id { get; set; }
        public List<int> Ratings { get; set; }
        public string Comment { get; set; }
    }
}
