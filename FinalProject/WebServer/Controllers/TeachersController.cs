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

                Guid teacherGuid;
                var didSucceedParsingTeacherGuid = Guid.TryParse(id, out teacherGuid);
                var teacher = didSucceedParsingTeacherGuid ? session.Load<Teacher>(teacherGuid) : null;
                if (teacher == null) {
                    return NotFound();
                }
                
                return Ok(teacher);
            }
        }

        [HttpGet]
        [ActionName("GetTeacherId")]
        public IEnumerable<string> GetTeacherId(string name)
        {
            using (var session = DBHelper.OpenSession())
            {
                //Teacher teacher;
                IList<Teacher> teachersList = session.QueryOver<Teacher>().List();
                foreach(Teacher teacher in teachersList)
                {
                    if(teacher.Name == name)
                    {
                   ////     return teacher.Id; 
                        //לא סגורה על מה צריך להחזיר פה :(
                    }
                }
            }
            return null;
        }
        
        [HttpGet]
        [ActionName("GetTeachers")]
        public IList<string> GetAllTeachersNames() {
            using (var session = DBHelper.OpenSession()) {
                return session.QueryOver<Teacher>().Select(x => x.Name).List<string>();
            }
        }

        [HttpGet]
        [ActionName("GetCommentById")]
        public IHttpActionResult GetCommentById([FromUri]string commentId) {
            using (var session = DBHelper.OpenSession()) {
                Guid teacherCommentGuid;
                var didSuccedParsingTeacherCommentGuid = Guid.TryParse(commentId, out teacherCommentGuid);
                var comment = session.Load<TeacherComment>(teacherCommentGuid);
                if (comment == null) {
                    return NotFound();
                }

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
        public void AddComment([FromBody]CreateTeacherComment comment) {
            using (var session = DBHelper.OpenSession())
            using (var transaction = session.BeginTransaction()) {
                Guid teacherGuid;
                var didTeacherGuidParseSucceed = Guid.TryParse(comment.Id, out teacherGuid);
                var teacher = didTeacherGuidParseSucceed ? session.Load<Teacher>(teacherGuid) : null;
                if (teacher != null) {
                    List<int> ratings = new List<int>();
                    foreach(var rating in comment.Ratings) {
                        ratings.Add(Convert.ToInt32(rating));
                    }
                    teacher.addTeacherCommnet(new TeacherComment(comment.Comment, teacher, ratings));
                }
                session.Save(teacher);
                transaction.Commit();
            }
        }

        [HttpGet]
        [ActionName("GetCriterias")]
        public IHttpActionResult GetAllCriterias() {
            var criterias = TeacherComment.CriteriaList();
            if (criterias == null)
                return NotFound();
            else
                return Ok(criterias);
        }
    }

    public class CreateTeacherCommand {
        public string Name { get; set; }
        public string UniversityName { get; set; }
    }

    public class CreateTeacherComment {
        public string Id { get; set; }
        public List<string> Ratings { get; set; }
        public string Comment { get; set; }
    }
}
