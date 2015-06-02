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
        public IEnumerable<Course> GetAllCourses()
        {
            using (var session = DBHelper.OpenSession())
            {
                return session.QueryOver<Course>().List();
            }
        }

        public IHttpActionResult GetCourse(Guid id)
        {
            using (var session = DBHelper.OpenSession())
            {
                var course = session.Get<Course>(id);

                if (course == null)
                {
                    return NotFound();
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
        public void AddCourse([FromBody]CreateCourseCommand createCommand)
        {
            using (var session = DBHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var university = session.QueryOver<University>().Where(x => x.Name == createCommand.UniversityName).SingleOrDefault();
                var teacher = session.QueryOver<Teacher>().Where(x => x.Name == createCommand.TeacherName).SingleOrDefault();

                var course = new Course
                {
                    Name = createCommand.Name,
                    University = university,
                    Teachers = new []{teacher}
                };

                session.Save(course);

                transaction.Commit();
            }
        }
    }

    public class CreateCourseCommand
    {
        public string Name { get; set; }
        public string UniversityName { get; set; }
        public string TeacherName { get; set; }
    }
}
