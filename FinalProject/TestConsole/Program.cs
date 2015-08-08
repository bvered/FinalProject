using System;
using System.Collections.Generic;
using NHibernate;
using WebServer.App_Data;
using WebServer.App_Data.Models;
using WebServer.App_Data.Models.Enums;

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sessionFactory = NHibernateConfig.CreateSessionFactory(true);
            ISession session = sessionFactory.OpenSession();


            CreateInitData(session);

            getBestTeachers(session);

            session.Flush();
            session.Close();

            Console.WriteLine("Finished Creating DB!");
            Console.ReadKey();
        }

        private static void CreateInitData(ISession session)
        {
            const Faculty computersFaculty = Faculty.ComputerScience;
            const Faculty socialityFaculty = Faculty.SocietyPolitics;

            var romina = new Teacher("רומינה זיגדון", 232, "05x-xxxxxxx", "xxx@xxx.xxx");
            TeacherComment rominaComment = new TeacherComment
            {
                CommentText = "ממש עוזרת ללמוד",
                DateTime = DateTime.Now,
            };
            foreach (var teacherCritiria in TeacherComment.GetTeacherCommentCriterias()) {
                rominaComment.CriteriaRatings.Add(new TeacherCriteriaRating(teacherCritiria, 5));
            }
            rominaComment.AddVote(new Vote(true));
            romina.addTeacherCommnet(rominaComment);
            
            var teachers = new[]
            {
                romina,
                new Teacher {Name = "אמיר קירש"},
                new Teacher {Name = "יוסי בצלאל"},
                new Teacher {Name = "צבי מלמד"},
                new Teacher {Name = "כרמי"},
                new Teacher {Name ="הדר בינסקי"},
                new Teacher {Name ="בוריס לוין"},
                new Teacher {Name ="אלכס קומן"},
            };

            foreach (var teacher in teachers)
            {
                session.Save(teacher);
            }


            var logic = new Course
            {
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
                    Name = "אלגוריתמים",
                    AcademicDegree = AcademicDegree.Bachelor,
                    Faculty = computersFaculty,
                    IntendedYear = IntendedYear.Second,
                    IsMandatory = true
                },
                new Course
                {
                    Name = "תורת הגרפים",
                    AcademicDegree = AcademicDegree.Bachelor,
                    Faculty = computersFaculty,
                    IntendedYear = IntendedYear.Any,
                    IsMandatory = false
                },
                new Course
                {
                    Name = "סיבוכיות ",
                    AcademicDegree = AcademicDegree.Master,
                    Faculty = computersFaculty,
                    IntendedYear = IntendedYear.First,
                    IsMandatory = true
                },
                 new Course
                {
                    Name = "ביולוגיה",
                    AcademicDegree = AcademicDegree.Master,
                    Faculty = socialityFaculty,
                    IntendedYear = IntendedYear.First,
                    IsMandatory = true
                },
                new Course
                {
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
                Votes = {new Vote(true)}
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
            logic.addCourseCommnet(logic.CourseInSemesters[0], courseComment);

            foreach (var course in courses)
            {
                session.Save(course);
            }
        }

        private static void getBestTeachers(ISession session)
        {
            IList<Teacher> bestTeachers = session.QueryOver<Teacher>().OrderBy(x => x.Score).Asc.Take(10).List();
        }

        private static void getAllTeacherPerCourse(ISession session, Course course)
        {
//TODO
            //    IList<Teacher> allTeachers = session.QueryOver<Teacher>().Where(x => x.Courses.Contains(course)).List();
        }
    }
}