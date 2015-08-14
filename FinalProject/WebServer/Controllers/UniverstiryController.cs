using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NHibernate.Linq;
using WebServer.App_Data;
using WebServer.App_Data.Models;

namespace WebServer.Controllers
{
    public class UniverstiryController : ApiController
    {
        [HttpGet]
        [ActionName("GetUniversities")]
        public IList<resultUniversity> GetAllCoursesNames()
        {
            using (var session = DBHelper.OpenSession())
            {
                var universities = session.QueryOver<University>().List();

                return universities.Select(university => new resultUniversity(university.Name, university.SiteAddress)).ToList();;
            }
        }

        public class resultUniversity
        {
             public string Name { get; set; }
             public string WebAddress { get; set; }

            public resultUniversity(string name, string web)
            {
                Name = name;
                WebAddress = web;
            }
        }
    }
}
