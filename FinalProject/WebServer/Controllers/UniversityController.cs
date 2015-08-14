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
    public class UniversityController : ApiController
    {
        [HttpGet]
        [ActionName("GetUniversities")]
        public IList<resultUniversity> GetAllCoursesNames()
        {
            using (var session = DBHelper.OpenSession())
            {
                var universities = session.QueryOver<University>().List();

                return universities.Select(university => new resultUniversity(university.Name, university.Acronyms, university.SiteAddress)).ToList(); ;
            }
        }

        [HttpGet]
        [ActionName("GetUniversityWebByAcronyms")]
        public string GetUniversityWebByAcronyms([FromUri] string id)
        {
            using (var session = DBHelper.OpenSession())
            {
                return session.QueryOver<University>().List().Where(x => x.Acronyms == id).Select(x => x.SiteAddress).SingleOrDefault();
            }
        }

        public class resultUniversity
        {
             public string Name { get; set; }
             public string Acronyms { get; set; }
             public string WebAddress { get; set; }

             public resultUniversity(string name, string acronyms, string webAddress)
            {
                Name = name;
                Acronyms = acronyms;
                 WebAddress = webAddress;
            }
        }
    }
}
