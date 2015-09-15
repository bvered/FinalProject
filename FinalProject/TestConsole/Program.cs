using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NHibernate;
using WebServer.App_Data;
using WebServer.App_Data.Models;
using WebServer.App_Data.Models.Enums;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Globalization;

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            bool updateDb = args.Length == 1 && args[0].Contains("/update");

            var sessionFactory = NHibernateConfig.CreateSessionFactory(!updateDb,
                @"..\..\..\..\WebServer\App_Data\ProjectDB.db");

            if (!updateDb)
            {
                var session = sessionFactory.OpenSession();

                CreateInitData(session);

                session.Flush();
                session.Close();
            }

            Console.WriteLine("Finished Creating DB!");
            Console.ReadKey();
        }

        private static void CreateInitData(ISession session)
        {
            var mta = new University
            {
                Acronyms = "MTA",
                Name = "המכללה האקדמית תל אביב יפו",
                SiteAddress = "www.mta.ac.il",
                FileExtention = "jpeg"
            };

            Image img = Image.FromFile(@"Images\site_background.jpeg");
            byte[] arr;
            using (var ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                arr = ms.ToArray();
            }
            mta.BackgroundImage = arr;

            var bgu = new University
            {
                Acronyms = "BGU",
                Name = "אוניברסטית בן גוריון",
                SiteAddress = "in.bgu.ac.il/Pages/default.aspx",
                FileExtention = "jpg"
            };

            session.Save(mta);
            session.Save(bgu);

            img = Image.FromFile(@"Images\BGU.jpg");
            byte[] arr2;
            using (var ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                arr2 = ms.ToArray();
            }

            bgu.BackgroundImage = arr2;

            var dtexcel = new DataTable("Report$".TrimEnd('$'));
            const string query = "SELECT  * FROM [Report1$]";
            using (OleDbConnection conn = CreateConnection(@"Images\db.xlsx", true))
            {
                var daexcel = new OleDbDataAdapter(query, conn);
                dtexcel.Locale = CultureInfo.InvariantCulture;
                daexcel.Fill(dtexcel);
            }

            var teachersForCourses = CreateTeachers(dtexcel, session, mta);

            CreateCourses(dtexcel, session, teachersForCourses, mta);

        }

        private static void CreateCourses(DataTable dtexcel, ISession session, Dictionary<int, Teacher> teachersForCourses, University mta)
        {
            int index = 0;
            var courses = new Dictionary<Tuple<string, Faculty>, List<CourseInSemesterTemp>>();
            foreach (DataRow row in dtexcel.Rows)
            {
                string courseName = row["Description"].ToString();
                string facultyName = row["Faculty"].ToString();
                var nameAndFculty = Tuple.Create(courseName, FacultyMethod.FacultyFromString(facultyName));
                string semester = row["Semester"].ToString();
                string teacherName = row["TeacherName"].ToString();
                string intendedYear = row["Year"].ToString();

                if (courses.ContainsKey(nameAndFculty) == false)
                { // if the course doesnt exist
                    courses[nameAndFculty] = new List<CourseInSemesterTemp>();
                    CreateCourseInSemester(courses, row, intendedYear, semester, teacherName, nameAndFculty);
                }
                else
                { // if the course exists
                    bool found = false;
                    foreach (CourseInSemesterTemp courseInSemester in courses[nameAndFculty])
                    // cross all over the teachers, to see if we need to add new teacher to the course
                    {
                        if (courseInSemester.TeacherName == teacherName && courseInSemester.Semester == semester)
                        {
                            found = true;
                        }
                    }

                    if (found == false)
                    {
                        CreateCourseInSemester(courses, row, intendedYear, semester, teacherName, nameAndFculty);
                    }
                }
            }

            foreach (KeyValuePair<Tuple<string, Faculty>, List<CourseInSemesterTemp>> course in courses)
            {
                string name = course.Key.Item1;
                var faculty = course.Key.Item2;

                var newCourse = new Course(0, name, faculty)
                {
                    University = mta,
                    IsMandatory = course.Value.Any(x => x.IsMandatory)
                };

                bool wasAdded = false;
                foreach (CourseInSemesterTemp courseIn in course.Value)
                {
                    var semesterCourse = new CourseInSemester
                    {
                        Teacher =
                            teachersForCourses.ContainsKey(courseIn.Id) == false
                                ? null
                                : teachersForCourses[courseIn.Id],
                        Year = 2016,
                        Course = newCourse,
                        Semester = SemesterMethod.SemesterFromString(courseIn.Semester)
                    };


                    if (wasAdded == false)
                    {
                        newCourse.IntendedYear = IntendedYearMethod.IntendedYearFromInt(courseIn.IntendedYear);
                        newCourse.AcademicDegree = AcademicDegreeMethod.AcademicDegreeFromString(courseIn.AcademicDegree);
                        wasAdded = true;
                    }

                    newCourse.CourseInSemesters.Add(semesterCourse);
                }

                session.Save(newCourse);
                session.Flush();
                index++;
                if (index % 50 == 0)
                {
                    session.Clear();
                }
            }
        }

        private static Dictionary<int, Teacher> CreateTeachers(DataTable dtexcel, ISession session, University mta)
        {
            int index = 0;
            var teachersForCourses = new Dictionary<int, Teacher>();
            foreach (DataRow row in dtexcel.Rows)
            {
                var teacherIdString = row["TeacherId"].ToString();
                int teacherId;
                if (!Int32.TryParse(teacherIdString, out teacherId))
                {
                    continue;
                }

                var facultyName = row["Faculty"].ToString();

                if (teachersForCourses.ContainsKey(teacherId) == false)
                {
                    string teacherName = row["TeacherName"].ToString();
                    if (teacherName == string.Empty)
                    {
                        continue;
                    }

                    string teacherMail = row["Mail"].ToString();
                    Faculty teacherFaculty = FacultyMethod.FacultyFromString(facultyName);
                    var newTeacher = new Teacher(teacherId, teacherName, 0, "בקרוב", teacherMail, mta);
                    newTeacher.Faculties.Add(teacherFaculty);
                    teachersForCourses.Add(teacherId, newTeacher);
                }
                else
                {
                    var teacher = teachersForCourses[teacherId];
                    var faculty = FacultyMethod.FacultyFromString(facultyName);
                    if (!teacher.Faculties.Contains(faculty))
                    {
                        teacher.Faculties.Add(faculty);
                    }
                }
            }

            foreach (var teacher in teachersForCourses)
            {
                session.Save(teacher.Value);
                index++;
                if (index % 50 == 0)
                {
                    session.Flush();
                    session.Clear();

                }
            }

            return teachersForCourses;

        }

        private static OleDbConnection CreateConnection(string filePath, bool hasHeaders)
        {
            string hdr = hasHeaders ? "Yes" : "No";
            string strConn;
            if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx" ||
                filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsm")
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"" + filePath +
                          "\";Extended Properties=\"Excel 12.0;HDR=" + hdr + ";IMEX=0\"";
            else
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"" + filePath +
                          "\";Extended Properties=\"Excel 8.0;HDR=" + hdr + ";IMEX=0\"";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            return conn;
        }

        private static void CreateCourseInSemester(Dictionary<Tuple<string, Faculty>, List<CourseInSemesterTemp>> courses, DataRow row,
            string intendedYear, string semester, string teacherName, Tuple<string, Faculty> nameAndFculty)
        {
            CourseInSemesterTemp courseSemesterTemp = new CourseInSemesterTemp();
            int isMandatory;
            bool mandatory = false;

            if (Int32.TryParse(row["Mandatory"].ToString(), out isMandatory))
            {
                mandatory = isMandatory != 0;
            }

            courseSemesterTemp.IsMandatory = mandatory;
            courseSemesterTemp.Semester = semester;
            courseSemesterTemp.TeacherName = teacherName;
            int teacherId;
            courseSemesterTemp.Id = Int32.TryParse(row["TeacherId"].ToString(), out teacherId) ? teacherId : 0;

            int year;
            if (!Int32.TryParse(intendedYear, out year))
            {
                year = 1;
            }

            courseSemesterTemp.IntendedYear = year;
            string academicDegree = row["AcademicDegree"].ToString();
            courseSemesterTemp.AcademicDegree = academicDegree;
            courses[nameAndFculty].Add(courseSemesterTemp);
        }

        public class CourseInSemesterTemp
        {
            public string TeacherName { get; set; }
            public string Semester { get; set; }
            public bool IsMandatory { get; set; }
            public int Id { get; set; }
            public int IntendedYear { get; set; }
            public string AcademicDegree { get; set; }
        }
    }
}