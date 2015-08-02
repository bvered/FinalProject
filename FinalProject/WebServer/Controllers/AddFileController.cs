using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebServer.App_Data;
using WebServer.App_Data.Models;
using WebServer.App_Data.Models.Enums;

namespace WebServer.Controllers
{
    public class AddFileController : ApiController
    {
        [HttpPost]
        [ActionName("AddSyllabus")]
        public IHttpActionResult AddSyllabus()
        {
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                // Get the uploaded image from the Files collection
                var httpPostedFile = HttpContext.Current.Request.Files["UploadedFile"];

                if (httpPostedFile != null)
                {
                    byte[] buffer;

                    using (var br = new BinaryReader(httpPostedFile.InputStream))
                    {
                        buffer = br.ReadBytes(httpPostedFile.ContentLength);
                    }

                    Guid courseId;
                    Semester semster;
                    int year;

                    var hasCourseId = Guid.TryParse(HttpContext.Current.Request.Form["courseId"], out courseId);
                    var hasSemster = Enum.TryParse(HttpContext.Current.Request.Form["semester"], out semster);
                    var hasYear = int.TryParse(HttpContext.Current.Request.Form["year"], out year);

                    if (!hasCourseId || !hasSemster || !hasYear)
                    {
                        return BadRequest();
                    }

                    using (var session = DBHelper.OpenSession())
                    using (var transaction = session.BeginTransaction())
                    {
                        var courseInSemester = session.QueryOver<CourseInSemester>()
                            .Where(x => x.Course.Id == courseId &&
                                        x.Semester == semster &&
                                        x.Year == year)
                            .SingleOrDefault();

                        if (courseInSemester == null)
                        {
                            courseInSemester = new CourseInSemester
                            {
                                Course = session.Load<Course>(courseId),
                                Semester = semster,
                                Year = year
                            };
                        }

                        var course = session.Load<Course>(courseId);

                        courseInSemester.Syllabus = new Syllabus
                        {
                            File = buffer,
                            FileName =
                                string.Format("{0}_{1}_{2}{3}", course.Name, year, semster,
                                    Path.GetExtension(httpPostedFile.FileName)),
                            Semster = semster,
                            Year = year
                        };

                        session.SaveOrUpdate(courseInSemester);

                        transaction.Commit();
                    }
                }

                return Ok();
            }

            return BadRequest();
        }

        public class CreateSyllabusFile
        {
            public Semester Semster { get; set; }
            public int Year { get; set; }
            public byte[] File { get; set; }
            public String FileName { get; set; }
        }
    }
}