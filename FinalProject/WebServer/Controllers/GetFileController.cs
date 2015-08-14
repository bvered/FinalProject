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
                    if (CourseSemester.uploadedSyllabus != null || CourseSemester.uploadedGrades != null)
                    {
                        Guid syllabusId = CourseSemester.uploadedSyllabus.Id;
                        string semester = CourseSemester.uploadedSyllabus.Semster.ToString();
                        int year = CourseSemester.uploadedSyllabus.Year;
                   //     byte[] file = CourseSemester.Syllabus.File;
                        string fileName = CourseSemester.uploadedSyllabus.FileName;
                        result.Add(new ResultSyllabus(syllabusId, semester, year/*,file*/, fileName));
                    }
                    

                }
                return result;
            }
           
        }

        [HttpGet]
        [ActionName("GetSyllabussss")]
        public int GetSyllabussss([FromUri]string id)
        {
            using (var session = DBHelper.OpenSession())
            {
                var Syllabus = session.QueryOver<UplodedFile>().List();
                IList<ResultSyllabus> result = new List<ResultSyllabus>();
                string val;
                var requestedSyllabus = session.Load<UplodedFile>(new Guid(id));

                if (requestedSyllabus.isPic == false)
                {
                    val = Encoding.UTF8.GetString(requestedSyllabus.File);
                }
                else
                {
                    File.WriteAllBytes(@"C:\temp\pic.jpg", requestedSyllabus.File);
                }
                return 1;



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

        public class ResultSyllabus
        {
            public Guid Id { get; set; }
            public string Semester { get; set; }
            public int Year { get; set; }
          //  public byte[] File { get; set; }
            public string FileName { get; set; }

            public ResultSyllabus(Guid id, string semester, int year/*, byte[] file*/, string fileName)
            {
                Id = id;
                Semester = semester;
                Year = year;
//                File = file;
                FileName = fileName; 
            }
        }
    }
}