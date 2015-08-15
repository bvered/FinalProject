using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using NHibernate.Linq;
using WebServer.App_Data;
using WebServer.App_Data.Models;
using WebServer.App_Data.Models.Enums;

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

        [HttpPost]
        [ActionName("AddUniversity")]
        public IHttpActionResult AddUniversity()
        {
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                // Get the uploaded image from the Files collection
                var httpPostedFile = HttpContext.Current.Request.Files["UploadedFile"];

                if (httpPostedFile != null)
                {
                    byte[] buffer;

                    using (var br = new BinaryReader(httpPostedFile.InputStream, Encoding.UTF8))
                    {
                        buffer = br.ReadBytes(httpPostedFile.ContentLength);
                    }

                    string UniversityName = HttpContext.Current.Request.Form["UniversityName"];
                    string UniversityAcronyms = HttpContext.Current.Request.Form["UniversityAcronyms"];
                    string UniversitySite = HttpContext.Current.Request.Form["UniversitySite"];

                    if (string.IsNullOrWhiteSpace(UniversityName) ||
                        string.IsNullOrWhiteSpace(UniversityAcronyms) ||
                        string.IsNullOrWhiteSpace(UniversitySite))
                    {
                        return BadRequest();
                    }

                    using (var session = DBHelper.OpenSession())
                    using (var transaction = session.BeginTransaction())
                    {
                        {
                            var newUniversity = new University
                            {
                                Acronyms = UniversityAcronyms,
                                Name = UniversityName,
                                SiteAddress = UniversitySite,
                                BackgroundImage = buffer,
                            };
                            session.SaveOrUpdate(newUniversity);
                            transaction.Commit();
                        }
                    }
                }
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [ActionName("CheckIfUniversityExists")]
        public IHttpActionResult CheckIfUniversityExists([FromBody] resultUniversity recivedUniversity)
        {
            using (var session = DBHelper.OpenSession())
            {
                var queryAcronyms = session.QueryOver<University>().List().Where(x => x.Acronyms == recivedUniversity.Acronyms).ToList();
                var queryWeb = session.QueryOver<University>().List().Where(x => x.SiteAddress == recivedUniversity.WebAddress).ToList();
                var queryName = session.QueryOver<University>().List().Where(x => x.Name == recivedUniversity.Name).ToList();

                if (queryAcronyms.Count <= 0 && queryWeb.Count <= 0 && queryName.Count <= 0) return Ok();
                return BadRequest();
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
