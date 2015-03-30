using System;
using System.Collections.Generic;
using System.Web.Http;
using WebServer.App_Data;
using WebServer.App_Data.Models;

namespace WebServer.Controllers
{
    public class CoursesController : ApiController
    {
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
    }
}
