using System;
using System.Collections.Generic;
using System.Web.Http;
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
    }
}
