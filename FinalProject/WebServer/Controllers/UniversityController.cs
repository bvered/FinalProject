using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using WebServer.App_Data;
using WebServer.App_Data.Models;

namespace WebServer.Controllers
{
    public class UniversityController : ApiController
    {
        [HttpGet]
        [ActionName("GetUniversities")]
        public IList<ReturnUniversity> GetAllCoursesNames()
        {
            using (var session = DBHelper.OpenSession())
            {
                var universities = session.QueryOver<University>().List();

                return
                    universities.Select(
                        university => new ReturnUniversity(university.Name, university.Acronyms, university.SiteAddress))
                        .ToList();
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

                    string universityName = HttpContext.Current.Request.Form["UniversityName"];
                    string universityAcronyms = HttpContext.Current.Request.Form["UniversityAcronyms"];
                    string universitySite = HttpContext.Current.Request.Form["UniversitySite"];

                    if (string.IsNullOrWhiteSpace(universityName) ||
                        string.IsNullOrWhiteSpace(universityAcronyms) ||
                        string.IsNullOrWhiteSpace(universitySite))
                    {
                        return BadRequest();
                    }

                    universitySite = universitySite.ToLower();
                    if (!universitySite.Contains("http://") && !universitySite.Contains("https://"))
                    {
                        universitySite = "http://" + universitySite;
                    }

                    if (CheckIfUniversityExists(universityName, universityAcronyms, universitySite))
                    {
                        return Conflict();
                    }

                    using (var session = DBHelper.OpenSession())
                    using (var transaction = session.BeginTransaction())
                    {
                        {
                            var newUniversity = new University
                            {
                                Acronyms = universityAcronyms,
                                Name = universityName,
                                SiteAddress = universitySite,
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
        public ReturnUniversity GetUvinersitryPicture([FromUri] string id)
        {
            using (var session = DBHelper.OpenSession())
            {
                var university = session.QueryOver<University>().List().SingleOrDefault(x => x.Acronyms == id);
                var base64String = Convert.ToBase64String(university.BackgroundImage, 0,
                    university.BackgroundImage.Length);
                ReturnUniversity newUniversity = new ReturnUniversity
                {
                    WebAddress = university.SiteAddress,
                    UniversityName = university.Name,
                    Base64 = string.Format("data:image/{0};base64,{1}", university.FileExtention, base64String),
                };
                return newUniversity;
            }
        }

        public bool CheckIfUniversityExists(string universityName, string universityAcronyms, string universitySite)
        {
            using (var session = DBHelper.OpenSession())
            {
                int query =
                    session.QueryOver<University>()
                        .Where(
                            x =>
                                x.Acronyms == universityAcronyms || x.SiteAddress == universitySite ||
                                x.Name == universityName).RowCount();

                if (query == 0) return false;
                return true;
            }
        }

        public class ReturnUniversity
        {
            public string WebAddress { get; set; }
            public string UniversityName { get; set; }
            public string Base64 { get; set; }
            public string Acronyms { get; set; }

            public ReturnUniversity(string name, string acronyms, string webAddress)
            {
                UniversityName = name;
                Acronyms = acronyms;
                WebAddress = webAddress;
            }

            public ReturnUniversity()
            {
            }
        }
    }
}
