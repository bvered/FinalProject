using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using NHibernate;
using WebServer.App_Data;
using WebServer.App_Data.Models;
using WebServer.App_Data.Models.Enums;
<<<<<<< HEAD
using System.Data;
using System.Data.OleDb;
using System.Globalization;
=======
using System.Drawing;
>>>>>>> origin/master

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sessionFactory = NHibernateConfig.CreateSessionFactory(true);
            var session = sessionFactory.OpenSession();

            CreateInitData(session);

            session.Flush();
            session.Close();

            Console.WriteLine("Finished Creating DB!");
            Console.ReadKey();
        }

        private static void CreateInitData(ISession session)
        {
            var MTA = new University
            {
                Acronyms = "MTA",
                Name = "המכללה האקדמית תל אביב יפו",
                SiteAddress = "www.mta.ac.il",
                FileExtention = "jpeg"
            };

            string currentDir = Environment.CurrentDirectory;
            Image img = Image.FromFile(currentDir + @"\..\..\Images\site_background.jpeg");
            byte[] arr;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                arr = ms.ToArray();
            }
            MTA.BackgroundImage = arr;

            var BGU = new University
            {
                Acronyms = "BGU",
                Name = "אוניברסטית בן גוריון",
                SiteAddress = "in.bgu.ac.il/Pages/default.aspx",
                FileExtention = "jpg"
            };

            img = Image.FromFile(currentDir + @"\..\..\Images\BGU.jpg");
            byte[] arr2;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                arr2 = ms.ToArray();
            }
            BGU.BackgroundImage = arr2;

            const Faculty computersFaculty = Faculty.ComputerScience;
            const Faculty socialityFaculty = Faculty.SocietyPolitics;

            var romina = new Teacher("רומינה זיגדון", 232, "05x-xxxxxxx", "ROMINAZI@MTA.AC.IL", MTA);
            TeacherComment rominaComment = new TeacherComment
            {
                CommentText = "ממש עוזרת ללמוד",
                DateTime = DateTime.Now,
            };
            foreach (var teacherCritiria in TeacherComment.GetTeacherCommentCriterias())
            {
                rominaComment.CriteriaRatings.Add(new TeacherCriteriaRating(teacherCritiria, 5));
            }
            rominaComment.AddVote(new Vote(true));
            romina.AddTeacherCommnet(rominaComment);


            DataTable dtexcel = new DataTable("Report$".TrimEnd('$'));
            using (OleDbConnection conn = CreateConnection(@"C:\Users\מיטל\Desktop\לימודים\שנה ג\סדנה\FinalProject\FinalProject\db.xlsx", true))
            {
                string query = "SELECT  * FROM [" + "Report1$" + "]";
                OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                dtexcel.Locale = CultureInfo.InvariantCulture;
                daexcel.Fill(dtexcel);
            }

            //saves the teachers by name and mail
            var teachers = new Dictionary<string, string>();

            foreach (DataRow row in dtexcel.Rows)
            {
               // Console.WriteLine(row["שם מרצה"]);
                string teacherName = row["שם מרצה"].ToString();
                if (teachers.ContainsKey(teacherName) ==  false)
                {
                    string mail = row["מייל"].ToString();
                    teachers.Add(teacherName,mail);
                  //  teachers[teacherName]
                }
            }


            foreach (KeyValuePair<string, string> teacher in teachers)
            {
                var newTeacher = new Teacher(teacher.Key, 0, "05x-xxxxxxx", teacher.Value, MTA);
                session.Save(newTeacher);
            }
           /* var teachers = new[]
            {
                new Teacher {Name = "אמיר קירש"},
                new Teacher {Name = "יוסי בצלאל"},
                new Teacher {Name = "צבי מלמד"},
                new Teacher {Name = "כרמי"},
                new Teacher {Name ="הדר בינסקי"},
                new Teacher {Name ="בוריס לוין"},
                new Teacher {Name ="אלכס קומן"},
            };*/

           /* foreach (var teacher in teachers)
            {
                session.Save(teacher);
            }
            */
            

            var logic = new Course
            {
                University = MTA,
                Name = "לוגיקה",
                AcademicDegree = AcademicDegree.Bachelor,
                Faculty = computersFaculty,
                IntendedYear = IntendedYear.First,
                IsMandatory = true,
            };

            var courses = new[]
            {
                logic,
                new Course
                {
                    University = MTA,
                    Name = "אלגוריתמים",
                    AcademicDegree = AcademicDegree.Bachelor,
                    Faculty = computersFaculty,
                    IntendedYear = IntendedYear.Second,
                    IsMandatory = true
                },
                new Course
                {
                    University = MTA,
                    Name = "תורת הגרפים",
                    AcademicDegree = AcademicDegree.Bachelor,
                    Faculty = computersFaculty,
                    IntendedYear = IntendedYear.Any,
                    IsMandatory = false
                },
                new Course
                {
                    University = MTA,
                    Name = "סיבוכיות ",
                    AcademicDegree = AcademicDegree.Master,
                    Faculty = computersFaculty,
                    IntendedYear = IntendedYear.First,
                    IsMandatory = true
                },
                 new Course
                {
                    University = MTA,
                    Name = "ביולוגיה",
                    AcademicDegree = AcademicDegree.Master,
                    Faculty = socialityFaculty,
                    IntendedYear = IntendedYear.First,
                    IsMandatory = true
                },
                new Course
                {
                    University = MTA,
                    Name = "מדעים",
                    AcademicDegree = AcademicDegree.Master,
                    Faculty = socialityFaculty,
                    IntendedYear = IntendedYear.Third,
                    IsMandatory = true
                },
            };

            var courseComment = new CourseComment
            {
                CommentText = "קורס ממש מעניין",
                DateTime = DateTime.Now,
                Votes = { new Vote(true) }
            };

            foreach (var courseCriteria in CourseComment.GetCourseCommentCriterias())
            {
                courseComment.CriteriaRatings.Add(new CourseCriteriaRating(courseCriteria, 5));
            }

            logic.CourseInSemesters.Add(new CourseInSemester
            {
                Semester = Semester.A,
                Teacher = romina,
                Course = logic,
                Year = 2012,
            });

            logic.AddCourseCommnet(logic.CourseInSemesters[0], courseComment);

            foreach (var course in courses)
            {
                session.Save(course);
            }
        }

        private static OleDbConnection CreateConnection(string filePath, bool hasHeaders)
        {
            string HDR = hasHeaders ? "Yes" : "No";
            string strConn;
            if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx" || filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsm")
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"" + filePath + "\";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
            else
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"" + filePath + "\";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            return conn;
        }
    }
}