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
        public IList<returnUniversity> GetAllCoursesNames()
        {
            using (var session = DBHelper.OpenSession())
            {
                var universities = session.QueryOver<University>().List();

                return
                    universities.Select(
                        university => new returnUniversity(university.Name, university.Acronyms, university.SiteAddress))
                        .ToList();
                ;
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

                    UniversitySite = UniversitySite.ToLower();
                    if (!UniversitySite.Contains("http://") && !UniversitySite.Contains("https://"))
                    {
                        UniversitySite = "http://" + UniversitySite;
                    }

                    if (CheckIfUniversityExists(UniversityName, UniversityAcronyms, UniversitySite))
                    {
                        return Conflict();
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
                                FileExtention = Path.GetExtension(httpPostedFile.FileName)
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

        [HttpGet]
        [ActionName("GetUvinersitryPicture")]
        public returnUniversity GetUvinersitryPicture([FromUri] string id)
        {
            using (var session = DBHelper.OpenSession())
            {
                var university = session.QueryOver<University>().List().SingleOrDefault(x => x.Acronyms == id);
                var base64String = Convert.ToBase64String(university.BackgroundImage, 0,
                    university.BackgroundImage.Length);
                returnUniversity newUniversity = new returnUniversity
                {
                    WebAddress = university.SiteAddress,
                    UniversityName = university.Name,
                    Base64 = string.Format("data:image/{0};base64,{1}", university.FileExtention, base64String),
                };
                return newUniversity;
            }
        }

        public bool CheckIfUniversityExists(string UniversityName, string UniversityAcronyms, string UniversitySite)
        {
            using (var session = DBHelper.OpenSession())
            {
                int query =
                    session.QueryOver<University>()
                        .Where(
                            x =>
                                x.Acronyms == UniversityAcronyms || x.SiteAddress == UniversitySite ||
                                x.Name == UniversityName).RowCount();

                if (query == 0) return false;
                return true;
            }
        }

        public class returnUniversity
        {
            public string WebAddress { get; set; }
            public string UniversityName { get; set; }
            public string Base64 { get; set; }
            public string Acronyms { get; set; }

            public returnUniversity(string name, string acronyms, string webAddress)
            {
                UniversityName = name;
                Acronyms = acronyms;
                WebAddress = webAddress;
            }

            public returnUniversity()
            {
            }
        }
    }
}
