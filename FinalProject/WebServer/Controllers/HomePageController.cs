using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebServer.App_Data;
using WebServer.App_Data.Models;

namespace WebServer.Controllers
{
    public class HomePageController : ApiController
    {
        [HttpGet]
        [ActionName("GetCourses")]
        public IList<string> GetAllCoursesNames()
        {
            using (var session = DBHelper.OpenSession())
            {
                return session.QueryOver<Course>().Select(x => x.Name).List<string>();
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
    }
}
