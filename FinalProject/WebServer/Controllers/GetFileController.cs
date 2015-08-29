using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using WebServer.App_Data;
using WebServer.App_Data.Models;

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
                session.QueryOver<UplodedFile>().List();
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
                        string extenstion = CourseSemester.uploadedSyllabus.ext;
                        result.Add(new ResultSyllabus(syllabusId, semester, year, fileName, true, extenstion));
                    }
                    if (CourseSemester.uploadedGrades != null)
                    {
                        Guid syllabusId = CourseSemester.uploadedGrades.Id;
                        string semester = CourseSemester.uploadedGrades.Semster.ToString();
                        semester = getSpecificSemester(semester);
                        int year = CourseSemester.uploadedGrades.Year;
                        string fileName = CourseSemester.uploadedGrades.FileName;
                        string extenstion = CourseSemester.uploadedGrades.ext;
                        result.Add(new ResultSyllabus(syllabusId, semester, year, fileName, false, extenstion));
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
                session.QueryOver<UplodedFile>().List();
                var requestedSyllabus = session.Load<UplodedFile>(new Guid(id));
                RequestedFile file = new RequestedFile();

                switch (requestedSyllabus.ext.ToLower())
                {
                    case ".pdf":
                        {
                            var s = Path.GetExtension(requestedSyllabus.FileName);
                            if (s != null)
                            {
                                FileStream stream = new FileStream(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Images\filePdf" + s.ToLower(), FileMode.Append);
                                BinaryWriter writer = new BinaryWriter(stream);
                                writer.Write(requestedSyllabus.File, 0, requestedSyllabus.File.Length);
                                writer.Close();
                            }
                            file.isPic = false;
                            file.str = @"../Images/filePdf.pdf";
                            var extension = Path.GetExtension(requestedSyllabus.FileName);
                            if (extension != null)
                                file.ext = extension.ToLower();
                        }
                        break;
                    case ".doc":
                    case ".docx":
                        {
                            var extension1 = Path.GetExtension(requestedSyllabus.FileName);
                            if (extension1 != null)
                            {
                                FileStream stream = new FileStream(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Images\fileWord" + extension1.ToLower(), FileMode.Create);
                                BinaryWriter writer = new BinaryWriter(stream);
                                writer.Write(requestedSyllabus.File, 0, requestedSyllabus.File.Length);
                                writer.Close();
                            }
                            file.isPic = false;
                            var extension = Path.GetExtension(requestedSyllabus.FileName);
                            if (extension != null)
                                file.str = @"../Images/fileWord" + extension.ToLower();
                            var s = Path.GetExtension(requestedSyllabus.FileName);
                            if (s != null)
                                file.ext = s.ToLower();
                        }
                        break;
                    default:
                        if (requestedSyllabus.isPic == false)
                        {
                            var extension1 = Path.GetExtension(requestedSyllabus.FileName);
                            if (extension1 != null)
                                File.WriteAllBytes(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Images\fileTxt" + extension1.ToLower(), requestedSyllabus.File);
                            file.isPic = false;
                            var extension = Path.GetExtension(requestedSyllabus.FileName);
                            if (extension != null)
                                file.str = @"../Images/fileTxt" + extension.ToLower();
                            var s = Path.GetExtension(requestedSyllabus.FileName);
                            if (s != null)
                                file.ext = s.ToLower();
                        }
                        else
                        {
                            var extension1 = Path.GetExtension(requestedSyllabus.FileName);
                            if (extension1 != null)
                                File.WriteAllBytes(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Images\filePic" + extension1.ToLower(), requestedSyllabus.File);
                            file.isPic = true;
                            var extension = Path.GetExtension(requestedSyllabus.FileName);
                            if (extension != null)
                                file.str = @"../Images/filePic" + extension.ToLower();
                            var s = Path.GetExtension(requestedSyllabus.FileName);
                            if (s != null)
                                file.ext = s.ToLower();
                        }
                        break;
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