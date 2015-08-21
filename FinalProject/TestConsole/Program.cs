using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
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

            var romina = new Teacher(13899828, "רומינה זיגדון", 232, "05x-xxxxxxx", "ROMINAZI@MTA.AC.IL", MTA, Faculty.ComputerScience);
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
            var teachersSaved = new Dictionary<int, string>();
            foreach (DataRow row in dtexcel.Rows)
            {
                var teacherIdString = row["TeacherId"].ToString();
                int teacherId;
                if (!Int32.TryParse(teacherIdString, out teacherId))
                {
                    continue;
                }
                if (teachersSaved.ContainsKey(teacherId) == false)
                {
                    string teacherName = row["TeacherName"].ToString();
                    if (teacherName == string.Empty)
                    {
                        continue;
                    }
                    teachersSaved.Add(teacherId, teacherName);
                    string teacherMail = row["Mail"].ToString();
                    Faculty teacherFaculty = FacultyMethod.FacultyFromString(row["Faculty"].ToString());
                    var newTeacher = new Teacher(teacherId, teacherName, 0, "בקרוב", teacherMail, MTA, teacherFaculty);
                    session.Save(newTeacher);

                    session.Flush();
                }
            }




            //add the courses
            /*
            var courses = new Dictionary<string, List<courseInSemester>>();

            foreach (DataRow row in dtexcel.Rows)
            {
                string courseName = row["שם הקורס"].ToString();
                string facultyName = row["פקולטה"].ToString();
                string nameAndFculty = courseName + "." + facultyName;

                string semester = row["סמסטר"].ToString();
                string teacherName = row["שם מרצה"].ToString();

                if (courses.ContainsKey(nameAndFculty) == false)
                {
                    courseInSemester courseSemester = new courseInSemester();
                    courseSemester.isMandatory = false;
                    courseSemester.semester = semester;
                    courseSemester.teacherName = teacherName;
                    courses[nameAndFculty] = new List<courseInSemester>();

                    courses[nameAndFculty].Add(courseSemester);

                }
                else
                {
                    bool found = false;
                    foreach (courseInSemester courseInSemester in courses[nameAndFculty])
                    {
                        if (courseInSemester.teacherName == teacherName && courseInSemester.semester == semester)
                        {
                            found = true;
                        }
                    }

                    if (found == false)
                    {
                        courseInSemester courseSemester = new courseInSemester();

                        ///// אין את המידע הזה בקובץ EXCEL
                        courseSemester.isMandatory = false;//////////////

                        courseSemester.semester = semester;
                        courseSemester.teacherName = teacherName;

                        courses[nameAndFculty].Add(courseSemester);
                    }
                }
            }
            */

            /*עבור קורס חסרים השדות הבאים:
                *  AcademicDegree = AcademicDegree.Bachelor,
            * IntendedYear = IntendedYear.First,*/
            /*
            foreach (KeyValuePair<string, List<courseInSemester>> course in courses)
            {
                Faculty faculty = new Faculty();
                string name = course.Key.Substring(0, course.Key.IndexOf("."));
                string courseFaculty = course.Key.Substring(course.Key.IndexOf(".") + 1, course.Key.Length - name.Length - 1);
                if (courseFaculty == "יעוץ ופותוח ארגוני")
                {
                    faculty = Faculty.ConsultingOrganizationalDevelopment;
                }
                else if (courseFaculty == "כלכלה וניהול")
                {
                    faculty = Faculty.ManagementEconomics;
                }
                else if (courseFaculty == "מדעי המחשב")
                {
                    faculty = Faculty.ComputerScience;
                }
                else if (courseFaculty == "מדעי ההתנהגות")
                {
                    faculty = Faculty.BehavioralSciences;
                }
                else if (courseFaculty == "מדעי הסיעוד")
                {
                    faculty = Faculty.Nursing;
                }
                else if (courseFaculty == "ממשל וחברה")
                {
                    faculty = Faculty.SocietyPolitics;
                }
                else if (courseFaculty == "מנהל עסקים")
                {
                    faculty = Faculty.BusinessAdministration;
                }
                else if (courseFaculty == "ניהול מערכות מידע")
                {
                    faculty = Faculty.InformationSystems;
                }
                else if (courseFaculty == "פסיכולוגיה")
                {
                    faculty = Faculty.Psychology;
                }
                else
                {////////צריך להוסיף משהו כללי לפקולטות שלא נמצאות ברשימה כמו למשל יחידה ללימודי אנגלית
                    faculty = Faculty.ComputerScience;
                }/////////////

                var newCourse = new Course(0, name, faculty);
                newCourse.University = MTA;

                foreach (courseInSemester courseIn in course.Value) //אקבל את הרשימה של הקורסים בסמסטר לפי המחלקה החדשה שהגדרתי
                {
                    CourseInSemester semesterCourse = new CourseInSemester();
                    semesterCourse.Teacher = teachersForCourses[courseIn.teacherName];
                    semesterCourse.Year = 2016;
                    semesterCourse.Course = newCourse;
                    if (courseIn.semester == "1")
                    {
                        semesterCourse.Semester = Semester.A;
                    }
                    else if (courseIn.semester == "2")
                    {
                        semesterCourse.Semester = Semester.B;
                    }
                    else if (courseIn.semester == "3")
                    {
                        semesterCourse.Semester = Semester.Summer;
                    }
                    newCourse.CourseInSemesters.Add(semesterCourse);
                }

                session.Save(newCourse);
            }
            */
            //     Console.WriteLine("meitali");







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

        public class courseInSemester
        {
            public string teacherName { get; set; }
            public string semester { get; set; }
            public bool isMandatory { get; set; }

        }
    }
}