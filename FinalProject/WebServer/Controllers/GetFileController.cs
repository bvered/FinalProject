using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        [ActionName("GetAllFiles")]
        public IList<ResultSyllabus> GetAllFiles([FromUri]string id)
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
                        semester = getSpecificSemester(semester);
                        
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
                        semester = getSpecificSemester(semester);
                        int year = CourseSemester.uploadedGrades.Year;
                        string fileName = CourseSemester.uploadedGrades.FileName;
                        bool isSyllabus = false;
                        string extenstion = CourseSemester.uploadedGrades.ext;
                        result.Add(new ResultSyllabus(syllabusId, semester, year, fileName, isSyllabus, extenstion));
                    }

                }
                return result;
            }
           
        }

        private string getSpecificSemester(string semester)
        {
            string specificSemester = null;

            if (semester == "A")
            {
                specificSemester = "א";
            }
            else if (semester == "B")
            {
                specificSemester = "ב";
            }
            else if (semester == "Summer")
            {
                specificSemester = "קיץ";
            }

            return specificSemester;
        }

        [HttpGet]
        [ActionName("GetSpecificFile")]
        public RequestedFile GetSpecificFile([FromUri]string id)
        {
            using (var session = DBHelper.OpenSession())
            {
                var Syllabus = session.QueryOver<UplodedFile>().List();
                IList<ResultSyllabus> result = new List<ResultSyllabus>();
                string val;
                var requestedSyllabus = session.Load<UplodedFile>(new Guid(id));
                RequestedFile file = new RequestedFile();


                if (requestedSyllabus.ext.ToLower() == ".pdf")
                {
                    System.IO.FileStream stream = new FileStream(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory.ToString()) + @"\Images\filePdf" + Path.GetExtension(requestedSyllabus.FileName), FileMode.Append);
                    //System.IO.FileStream stream = new FileStream(@"C:\Users\מיטל\Desktop\לימודים\שנה ג\סדנה\FinalProject\FinalProject\WebServer\Images\filePdf" + Path.GetExtension(requestedSyllabus.FileName), FileMode.Append);
                    System.IO.BinaryWriter writer = new BinaryWriter(stream);
                    writer.Write(requestedSyllabus.File, 0, requestedSyllabus.File.Length);
                    writer.Close();
                    file.isPic = false;
                    file.str = @"../Images/filePdf.pdf";
                    file.ext = Path.GetExtension(requestedSyllabus.FileName).ToLower();
                }
                else if (requestedSyllabus.ext.ToLower() == ".doc" || requestedSyllabus.ext.ToLower() == ".docx")
                {
                    System.IO.FileStream stream = new FileStream(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory.ToString()) + @"\Images\fileWord" + Path.GetExtension(requestedSyllabus.FileName), FileMode.Create);
                 //   System.IO.FileStream stream = new FileStream(@"C:\Users\מיטל\Desktop\לימודים\שנה ג\סדנה\FinalProject\FinalProject\WebServer\Images\fileWord" + Path.GetExtension(requestedSyllabus.FileName), FileMode.Create);
                    System.IO.BinaryWriter writer = new BinaryWriter(stream);
                    writer.Write(requestedSyllabus.File, 0, requestedSyllabus.File.Length);
                    writer.Close();
                    file.isPic = false;
                    file.str = @"../Images/fileWord" + Path.GetExtension(requestedSyllabus.FileName).ToLower();
                    file.ext = Path.GetExtension(requestedSyllabus.FileName).ToLower();
                }
                else
                {
                    if (requestedSyllabus.isPic == false)
                    {
                        val = Encoding.UTF8.GetString(requestedSyllabus.File);
                        file.isPic = false;
                        file.str = val;
                        file.ext = Path.GetExtension(requestedSyllabus.FileName).ToLower();
                    }
                    else
                    {
                        File.WriteAllBytes(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory.ToString())+ @"\Images\filePic" + Path.GetExtension(requestedSyllabus.FileName), requestedSyllabus.File);
                       // File.WriteAllBytes(@"C:\Users\מיטל\Desktop\לימודים\שנה ג\סדנה\FinalProject\FinalProject\WebServer\Images\filePic" + Path.GetExtension(requestedSyllabus.FileName), requestedSyllabus.File);
                        file.isPic = true;
                        file.str = @"../Images/filePic.jpg";
                        file.ext = Path.GetExtension(requestedSyllabus.FileName).ToLower();
                    }
                }
                return file;
            }
        }

        public class RequestedFile
        {
            public bool isPic { get; set; }
            public string str { get; set; }
            public string ext { get; set; }
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