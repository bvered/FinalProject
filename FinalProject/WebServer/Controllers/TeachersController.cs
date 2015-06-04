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
        [HttpGet]
        [ActionName("GetAllTeachers")]
        public IEnumerable<Teacher> GetAllTeachers()
        {
            using (var session = DBHelper.OpenSession())
            {
                return session.QueryOver<Teacher>().List();
            }
        }

        [HttpGet]
        [ActionName("GetTeacher")]
        public IHttpActionResult GetTeacher([FromUri]string id)
        {
            using (var session = DBHelper.OpenSession()) {
                var teacher =  session.QueryOver<Teacher>()
                .Where(t => t.Name == id)
                .List();
                if (teacher == null) {
                    return NotFound();
                }
                
                return Ok(teacher);
            }
        }

        [HttpGet]
        [ActionName("GetTeachers")]
        public IList<string> GetAllTeachersNames() {
            using (var session = DBHelper.OpenSession()) {
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

                if (teacher != null)
                {
                    List<int> ratings = new List<int>();
                    foreach(var rating in comment.Ratings) {
                        ratings.Add(Convert.ToInt32(rating));
                    }
                    teacher.addTeacherCommnet(new TeacherComment(null, comment.Comment, teacher, ratings));
                }
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
        public List<string> Ratings { get; set; }
        public string Comment { get; set; }
    }
}
