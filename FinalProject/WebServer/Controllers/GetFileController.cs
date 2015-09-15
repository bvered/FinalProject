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

                foreach (var courseSemester in course.CourseInSemesters)
                {
                    if (courseSemester.uploadedSyllabus != null)
                    {
                        Guid syllabusId = courseSemester.uploadedSyllabus.Id;
                        string semester = courseSemester.uploadedSyllabus.Semster.ToString();
                        semester = getSpecificSemester(semester);
                        int year = courseSemester.uploadedSyllabus.Year;
                        string fileName = courseSemester.uploadedSyllabus.FileName;
                        string extenstion = courseSemester.uploadedSyllabus.ext;
                        result.Add(new ResultSyllabus(syllabusId, semester, year, fileName, true, extenstion, "סילבוס"));
                    }

                    if (courseSemester.uploadedGrades != null)
                    {
                        Guid gradesId = courseSemester.uploadedGrades.Id;
                        string semester = courseSemester.uploadedGrades.Semster.ToString();
                        semester = getSpecificSemester(semester);
                        int year = courseSemester.uploadedGrades.Year;
                        string fileName = courseSemester.uploadedGrades.FileName;
                        string extenstion = courseSemester.uploadedGrades.ext;
                        result.Add(new ResultSyllabus(gradesId, semester, year, fileName, false, extenstion, "התפלגות ציונים"));
                    }
                }

                return result;
            }
        }

        private string getSpecificSemester(string semester)
        {
            string specificSemester = null;

            switch (semester)
            {
                case "A":
                    specificSemester = "א";
                    break;
                case "B":
                    specificSemester = "ב";
                    break;
                case "Summer":
                    specificSemester = "קיץ";
                    break;
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
                var requestedFile = session.Load<UplodedFile>(new Guid(id));
                RequestedFile file = new RequestedFile();

                switch (requestedFile.ext.ToLower())
                {
                    case ".pdf":
                        {
                            var s = Path.GetExtension(requestedFile.FileName);
                            if (s != null)
                            {
                                FileStream stream = new FileStream(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Images\filePdf" + s.ToLower(), FileMode.Append);
                                BinaryWriter writer = new BinaryWriter(stream);
                                writer.Write(requestedFile.File, 0, requestedFile.File.Length);
                                writer.Close();
                            }
                            file.isPic = false;
                            file.str = @"../Images/filePdf.pdf";
                            var extension = Path.GetExtension(requestedFile.FileName);
                            if (extension != null)
                                file.ext = extension.ToLower();
                        }
                        break;
                    case ".doc":
                    case ".docx":
                        {
                            var extension1 = Path.GetExtension(requestedFile.FileName);
                            if (extension1 != null)
                            {
                                FileStream stream = new FileStream(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Images\fileWord" + extension1.ToLower(), FileMode.Create);
                                BinaryWriter writer = new BinaryWriter(stream);
                                writer.Write(requestedFile.File, 0, requestedFile.File.Length);
                                writer.Close();
                            }
                            file.isPic = false;
                            var extension = Path.GetExtension(requestedFile.FileName);
                            if (extension != null)
                                file.str = @"../Images/fileWord" + extension.ToLower();
                            var s = Path.GetExtension(requestedFile.FileName);
                            if (s != null)
                                file.ext = s.ToLower();
                        }
                        break;
                    default:
                        if (requestedFile.isPic == false)
                        {
                            var extension1 = Path.GetExtension(requestedFile.FileName);
                            if (extension1 != null)
                                File.WriteAllBytes(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Images\fileTxt" + extension1.ToLower(), requestedFile.File);
                            file.isPic = false;
                            var extension = Path.GetExtension(requestedFile.FileName);
                            if (extension != null)
                                file.str = @"../Images/fileTxt" + extension.ToLower();
                            var s = Path.GetExtension(requestedFile.FileName);
                            if (s != null)
                                file.ext = s.ToLower();
                        }
                        else
                        {
                            var extension1 = Path.GetExtension(requestedFile.FileName);
                            if (extension1 != null)
                                File.WriteAllBytes(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Images\filePic" + extension1.ToLower(), requestedFile.File);
                            file.isPic = true;
                            var extension = Path.GetExtension(requestedFile.FileName);
                            if (extension != null)
                                file.str = @"../Images/filePic" + extension.ToLower();
                            var s = Path.GetExtension(requestedFile.FileName);
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
            public string Type { get; set; }

            public ResultSyllabus(Guid id, string semester, int year, string fileName, bool isSyllabus, string extention, string type)
            {
                Id = id;
                Semester = semester;
                Year = year;
                FileName = fileName;
                IsSyllabus = isSyllabus;
                Extention = extention;
                Type = type;
            }
        }
    }
}