using System;
using System.Collections.Generic;
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
    public class GetFileController : ApiController
    {
        [HttpGet]
        [ActionName("GetSyllabus")]
        public IList<ResultSyllabus> GetSyllabus([FromUri]string id)
        {
            using (var session = DBHelper.OpenSession())
            {
                var Syllabus = session.QueryOver<UplodedFile>().List();
                IList<ResultSyllabus> result = new List<ResultSyllabus>();

                var course = session.Load<Course>(new Guid(id));

                foreach (var CourseSemester in course.CourseInSemesters)
                {
                    if (CourseSemester.uploadedSyllabus != null)
                    {
                        Guid syllabusId = CourseSemester.uploadedSyllabus.Id;
                        string semester = CourseSemester.uploadedSyllabus.Semster.ToString();
                        if (semester == "A") {
                            semester = "א";
                        }
                        else if (semester == "B") {
                            semester = "ב";
                        }
                        else if (semester == "Summer") {
                            semester = "קיץ";
                        }
                        int year = CourseSemester.uploadedSyllabus.Year;
                        string fileName = CourseSemester.uploadedSyllabus.FileName;
                        bool isSyllabus = true;
                        string extenstion = CourseSemester.uploadedSyllabus.ext;
                        result.Add(new ResultSyllabus(syllabusId, semester, year, fileName, isSyllabus, extenstion));
                    }
                    if (CourseSemester.uploadedGrades != null)
                    {
                        Guid syllabusId = CourseSemester.uploadedGrades.Id;
                        string semester = CourseSemester.uploadedGrades.Semster.ToString();
                        if (semester == "A")
                        {
                            semester = "א";
                        }
                        else if (semester == "B")
                        {
                            semester = "ב";
                        }
                        else if (semester == "Summer")
                        {
                            semester = "קיץ";
                        }
                        int year = CourseSemester.uploadedGrades.Year;
                        //     byte[] file = CourseSemester.Syllabus.File;
                        string fileName = CourseSemester.uploadedGrades.FileName;
                        bool isSyllabus = false;
                        string extenstion = CourseSemester.uploadedGrades.ext;
                        result.Add(new ResultSyllabus(syllabusId, semester, year, fileName, isSyllabus, extenstion));
                    }

                }
                return result;
            }
           
        }

        [HttpGet]
        [ActionName("GetSyllabussss")]
        public RequestedFile GetSyllabussss([FromUri]string id)
        {
            using (var session = DBHelper.OpenSession())
            {
                var Syllabus = session.QueryOver<UplodedFile>().List();
                IList<ResultSyllabus> result = new List<ResultSyllabus>();
                string val;
                var requestedSyllabus = session.Load<UplodedFile>(new Guid(id));
                RequestedFile file = new RequestedFile();

                if (requestedSyllabus.isPic == false)
                {
                    val = Encoding.UTF8.GetString(requestedSyllabus.File);
                    file.isPic = false;
                    file.str = val;
                }
                else
                {
                    File.WriteAllBytes(@"C:\Users\מיטל\Desktop\לימודים\שנה ג\סדנה\FinalProject\FinalProject\WebServer\Images\filePic" + Path.GetExtension(requestedSyllabus.FileName), requestedSyllabus.File);
                    file.isPic = true;
                    file.str = @"../Images/filePic.jpg";
                }
                return file;
            }
        }

        [ActionName("GetSpecificSyllabus")]
        public byte[] GetSpecificSyllabus([FromUri]string id)
        {
            using (var session = DBHelper.OpenSession())
            {
                var Syllabus = session.QueryOver<UplodedFile>().List();
                IList<ResultSyllabus> result = new List<ResultSyllabus>();

                var requestedSyllabus = session.Load<UplodedFile>(new Guid(id));

                return requestedSyllabus.File;
            }
        }

        public class RequestedFile
        {
            public bool isPic { get; set; }
            public string str { get; set; }
        }

        public class ResultSyllabus
        {
            public Guid Id { get; set; }
            public string Semester { get; set; }
            public int Year { get; set; }
            public string FileName { get; set; }
            public bool IsSyllabus { get; set; }
            public string Extention { get; set; }

            public ResultSyllabus(Guid id, string semester, int year, string fileName, bool isSyllabus, string extention)
            {
                Id = id;
                Semester = semester;
                Year = year;
                FileName = fileName;
                IsSyllabus = isSyllabus;
                Extention = extention;
            }
        }
    }
}