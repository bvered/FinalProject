﻿using System;
using System.IO;
using System.Linq;
using System.Text;
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
        [ActionName("AddFile")]
        public IHttpActionResult AddFile()
        {
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var httpPostedFile = HttpContext.Current.Request.Files["UploadedFile"];

                if (httpPostedFile != null)
                {
                    byte[] buffer;

                    using (var br = new BinaryReader(httpPostedFile.InputStream, Encoding.UTF8))
                    {
                        buffer = br.ReadBytes(httpPostedFile.ContentLength);
                    }

                    Guid courseId;
                    Semester semster;
                    int year;
                    bool isSyllabus = false;

                    var hasCourseId = Guid.TryParse(HttpContext.Current.Request.Form["courseId"], out courseId);
                    var hasSemster = Enum.TryParse(HttpContext.Current.Request.Form["semester"], out semster);
                    var hasYear = int.TryParse(HttpContext.Current.Request.Form["year"], out year);
                    string isSyll = (HttpContext.Current.Request.Form["isSyllabus"]);
                    if (isSyll == "1")
                    {
                        isSyllabus = true;
                    }

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
                            .SingleOrDefault() ?? new CourseInSemester
                            {
                                Course = session.Load<Course>(courseId),
                                Semester = semster,
                                Year = year
                            };

                        var course = session.Load<Course>(courseId);
                        string extension = Path.GetExtension(httpPostedFile.FileName);
                        if (string.IsNullOrEmpty(extension))
                            throw new ArgumentException(string.Format("Unable to determine file extension for fileName: {0}", httpPostedFile.FileName));
                        bool isPic = false;

                        switch (extension.ToLower())
                        {
                            case @".bmp":
                            case @".gif":
                            case @".ico":
                            case @".jpg":
                            case @".jpeg":
                            case @".png":
                            case @".tif":
                            case @".tiff":
                            case @".wmf":
                                isPic = true;
                                break;
                        }

                        if (!isSyllabus)
                        {
                            courseInSemester.uploadedGrades = new UplodedFile
                            {
                                File = buffer,
                                FileName =
                                    string.Format("{0}_{1}_{2}{3}", course.Name, year, semster,
                                        Path.GetExtension(httpPostedFile.FileName)),
                                Semster = semster,
                                Year = year,
                                isSylabus = false,
                                isPic = isPic,
                                ext = extension
                            };
                        }


                        if (isSyllabus)
                        {
                            courseInSemester.uploadedSyllabus = new UplodedFile
                            {
                                File = buffer,
                                FileName =
                                    string.Format("{0}_{1}_{2}{3}", course.Name, year, semster,
                                        Path.GetExtension(httpPostedFile.FileName)),
                                Semster = semster,
                                Year = year,
                                isSylabus = true,
                                isPic = isPic,
                                ext = extension
                            };
                        }
                        session.SaveOrUpdate(courseInSemester);

                        transaction.Commit();
                        return Ok(course.Id);
                    }
                }
            }
            return BadRequest();
        }

        public class CreateFile
        {
            public Semester Semster { get; set; }
            public int Year { get; set; }
            public byte[] File { get; set; }
            public String FileName { get; set; }
            public bool isSyllabus { get; set; }
        }
    }
}