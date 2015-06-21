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
                IList<Teacher> teachers = session.QueryOver<Teacher>().List();
                return teachers;
            }
        }


        [HttpGet]
        [ActionName("GetAllSearchedTeachers")]
        public IList<ResultTeacher> GetAllSearchedTeachers([FromUri]string teacherName)
        {
            using (var session = DBHelper.OpenSession())
            {
                IList<Teacher> teachers = session.QueryOver<Teacher>().List();  // get all the teachers
                IList<ResultTeacher> result = new List<ResultTeacher>();

                foreach (var teacher in teachers)
                {
                    if (teacherName == teacher.Name || (teacher.Name).IndexOf(teacherName) >= 0 || ((teacher.Name).ToLower()).IndexOf(teacherName) >= 0 || ((teacher.Name).ToLower()).IndexOf(teacherName.ToLower()) >= 0)
                    {
                        IList<string> courses = session.Query<CourseInSemester>()
                            .Where(x => x.Teacher.Id == teacher.Id)
                            .Select(x => x.Course.Name).Distinct()
                            .ToList();

                        result.Add(new ResultTeacher(teacher.Id, teacher.Name, courses, teacher.Score));
                    }
                }

                return result;
            }
        }

    /*    [HttpGet]
        [ActionName("GetAllSearchedTeachers")]
        public IList<ResultTeacher> GetAllSearchedTeachers()
        {
            using (var session = DBHelper.OpenSession())
            {
                IList<Teacher> teachers = session.QueryOver<Teacher>().List();
                IList<ResultTeacher> result = new List<ResultTeacher>();

                foreach (var teacher in teachers)
                {
                    IList<string> courses = session.Query<CourseInSemester>()
                        .Where(x => x.Teacher.Id == teacher.Id)
                        .Select(x => x.Course.Name).Distinct()
                        .ToList();

                    result.Add(new ResultTeacher(teacher.Id, teacher.Name, courses, teacher.Score));
                }

                return result;
            }
        }
        */
        [HttpGet]
        [ActionName("GetTeacher")]
        public IHttpActionResult GetTeacher([FromUri]string id)
        {
            using (var session = DBHelper.OpenSession())
            {
                Guid teacherGuid;
                if (!Guid.TryParse(id, out teacherGuid))
                {
                    return NotFound();
                }

                var teacher = session.Get<Teacher>(teacherGuid);

                if (teacher == null)
                {
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
                var teacher = new Teacher
                {
                    Name = createCommand.Name,
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

                for (int index = 0; index < teacherCriterias.Count; index++)
                {
                    newComment.CriteriaRatings.Add(new TeacherCriteriaRating
                    {
                        Criteria = teacherCriterias[index],
                        Rating = comment.Ratings[index]
                    });
                }

                teacher.TeacherComments.Add(newComment);

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
                return Ok(session.Query<TeacherCriteria>().Select(x => x.DisplayName).ToList());
            }
        }
    }

    public class ResultTeacher
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<string> Courses { get; set; }
        public int Score { get; set; }

        public ResultTeacher(Guid id, string name, IList<string> courses, int score)
        {
            Id = id;
            Name = name;
            Courses = courses;
            Score = score;
        }
    }

    public class CreateTeacherCommand {
        public string Name { get; set; }
    }

    public class CreateTeacherComment {
        public string Id { get; set; }
        public List<int> Ratings { get; set; }
        public string Comment { get; set; }
    }
}
