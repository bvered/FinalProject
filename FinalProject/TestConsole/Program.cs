using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            session.Save(MTA);
            session.Save(BGU);

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
            using (OleDbConnection conn = CreateConnection( Environment.CurrentDirectory + @"\..\..\Images\db.xlsx", true))
            {
                string query = "SELECT  * FROM [" + "Report1$" + "]";
                OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                dtexcel.Locale = CultureInfo.InvariantCulture;
                daexcel.Fill(dtexcel);
            }

            int index = 0;

            //saves the teachers by name and mail
            var teachersSaved = new Dictionary<int, string>();

            var teachersForCourses = new Dictionary<int, Teacher>();

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

                    teachersForCourses.Add(teacherId, newTeacher);
              //      teachersForCourses[teachersSaved.Key] = newTeacher;

                    index++;
                    if (index % 50 == 0)
                    {
                        session.Clear();
                    }
                }
            }


            index = 0;
            //add the courses
            
            var courses = new Dictionary<string, List<courseInSemester>>();

            foreach (DataRow row in dtexcel.Rows)
            {
                string courseName = row["Description"].ToString();
                string facultyName = row["Faculty"].ToString();
                string nameAndFculty = courseName + "." + facultyName;

                string semester = row["Semester"].ToString();
                string teacherName = row["TeacherName"].ToString();

                if (courses.ContainsKey(nameAndFculty) == false) //if the course doesnt exist
                {
                    courseInSemester courseSemester = new courseInSemester();
                    int isMandatory = 0;
                    bool mandatory=false;
                    if (Int32.TryParse(row["Mandatory"].ToString(), out isMandatory))
                    {
                        mandatory = isMandatory != 0;
                    }
                    courseSemester.isMandatory = mandatory;
                    courseSemester.semester = semester;
                    courseSemester.teacherName = teacherName;
                    int teacherId;
                    if (Int32.TryParse(row["TeacherId"].ToString(), out teacherId))
                    {
                        courseSemester.id = teacherId;
                    }
                    else
                    {
                        courseSemester.id = 0;
                    }
                        
                    courses[nameAndFculty] = new List<courseInSemester>();
                    courses[nameAndFculty].Add(courseSemester);
                }
                else // the course exists
                {
                    bool found = false;
                    foreach (courseInSemester courseInSemester in courses[nameAndFculty]) // cross all over the teachers, to see if we need to add new teacher 
                    {
                        if (courseInSemester.teacherName == teacherName && courseInSemester.semester == semester)
                        {
                            found = true;
                        }
                    }

                    if (found == false) // if the current teacher is not in the course semester yet
                    {
                        courseInSemester courseSemester = new courseInSemester();
                        int isMandatory = 0; 
                        bool mandatory = false;
                        if (Int32.TryParse(row["Mandatory"].ToString(), out isMandatory))
                        {
                            if (isMandatory == 0)
                            {
                                mandatory = false;
                            }
                            else
                            {
                                mandatory = true;
                            }
                        }
                        courseSemester.isMandatory = mandatory;
                        courseSemester.semester = semester;
                        courseSemester.teacherName = teacherName;
                        int teacherId;
                        if (Int32.TryParse(row["TeacherId"].ToString(), out teacherId))
                        {
                            courseSemester.id = teacherId;
                        }
                        else
                        {
                            courseSemester.id = 0;
                        }
            
                        courses[nameAndFculty].Add(courseSemester);
                    }
                }
            }
            

            /*עבור קורס חסרים השדות הבאים:
                *  AcademicDegree = AcademicDegree.Bachelor,
            * IntendedYear = IntendedYear.First,*/
            
            foreach (KeyValuePair<string, List<courseInSemester>> course in courses)
            {
                Faculty faculty = new Faculty();
                string name = course.Key.Substring(0, course.Key.IndexOf("."));
                string courseFaculty = course.Key.Substring(course.Key.IndexOf(".") + 1, course.Key.Length - name.Length - 1);
                if (courseFaculty == "יעוץ ופותוח ארגוני"){
                    faculty = Faculty.ConsultingOrganizationalDevelopment;
                }
                else if (courseFaculty == "כלכלה וניהול"){
                    faculty = Faculty.ManagementEconomics;
                }
                else if (courseFaculty == "מדעי המחשב"){
                    faculty = Faculty.ComputerScience;
                }
                else if (courseFaculty == "מדעי ההתנהגות"){
                    faculty = Faculty.BehavioralSciences;
                }
                else if (courseFaculty == "מדעי הסיעוד"){
                    faculty = Faculty.Nursing;
                }
                else if (courseFaculty == "ממשל וחברה"){
                    faculty = Faculty.SocietyPolitics;
                }
                else if (courseFaculty == "מנהל עסקים"){
                    faculty = Faculty.BusinessAdministration;
                }
                else if (courseFaculty == "ניהול מערכות מידע"){
                    faculty = Faculty.InformationSystems;
                }
                else if (courseFaculty == "פסיכולוגיה"){
                    faculty = Faculty.Psychology;
                }
                else
                {////////צריך להוסיף משהו כללי לפקולטות שלא נמצאות ברשימה כמו למשל יחידה ללימודי אנגלית
                    faculty = Faculty.ComputerScience;
                }/////////////

                var newCourse = new Course(0, name, faculty);

                newCourse.University = MTA;
                newCourse.IsMandatory = course.Value.Any(x => x.isMandatory);

                foreach (courseInSemester courseIn in course.Value) //אקבל את הרשימה של הקורסים בסמסטר לפי המחלקה החדשה שהגדרתי
                {
                    
                    CourseInSemester semesterCourse = new CourseInSemester();
                    if (teachersForCourses.ContainsKey(courseIn.id) == false)
                    {
                        semesterCourse.Teacher = null;
                    }
                    else
                    {
                        semesterCourse.Teacher = teachersForCourses[courseIn.id];
                    }
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

                session.Flush();

                index++;
                if (index % 50 == 0)
                {
                    session.Clear();
                }
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
            public int id { get; set; }

        }
    }
}