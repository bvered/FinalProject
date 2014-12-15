using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using Server;
using Server.Models;

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            User newUser = createObjects();

            var sessionFactory = NHibernateConfig.CreateSessionFactory();
            ISession session = sessionFactory.OpenSession();
            session.Save(newUser);
            session.Flush();

            Guid universityId = Guid.NewGuid();

            getBestTeachers(session);

            University universityWithId = session.Get<University>(universityId);
            IList<User> allUsers = session.QueryOver<User>().List();
            IList<string> allUserFullNames = session.QueryOver<User>().Select(x => x.FullName).List<string>();

            getReportedComments(session);

            session.Close();
        }

        private static User createObjects()
        {
            User newUser = new User
            {
                LoginEmail = "vered631gmailcom",
                BirthDate = new DateTime(1991, 07, 03),
                FullName = "veredb",
                PasswordHash = "123456",
            };

            University newUniversity = new University("Tel-aviv_yafo");
            Faculty newFaculty = new Faculty("Computer science", newUniversity);
            Course newCourse = new Course(newUniversity, 123456, "Math", newFaculty);

            createTeacher(newCourse, newUniversity);
            return newUser;
        }

        private static void createTeacher(Course newCourse, University newUniversity)
        {
            Teacher newTeacher = new Teacher("Romina");
            newTeacher.addCourse(newCourse);
            newTeacher.addUniversity(newUniversity);
        }

        private void addTeacherCritiries()
        {
            List<TeacherCriteria> teacherCritias = new List<TeacherCriteria>
            {
                new TeacherCriteria("Student- teacher relationship"),
                new TeacherCriteria("Teaching ability"),
                new TeacherCriteria("Teachers knowlegde level"),
                new TeacherCriteria("The teacher Encouregment for self learning"),
                new TeacherCriteria("The teacher interest level")
            };
        }

        private void addCourseCritiries()
        {
            List<CourseCriteria> courseCritias = new List<CourseCriteria>
            {
                new CourseCriteria("Material ease"),
                new CourseCriteria("Time investment for home-work"),
                new CourseCriteria("Number of home-work submissions"),
                new CourseCriteria("Time investment for test learning"),
                new CourseCriteria("Course usability"),
                new CourseCriteria("Course grades average"),
                new CourseCriteria("Does the attendance is mandatory"),
                new CourseCriteria("Does the test has open material/reference Pages")
            };
        }

        private static void getReportedComments(ISession session)
        {
            IList<CourseComment> courseCommentWithMoreThen5Reports = session.QueryOver<CourseComment>()
                                                                            .Where(x => x.Reports > 5).List();
        }

        private static void getBestTeachers(ISession session)
        {
            IList<Teacher> bestTeachers = session.QueryOver<Teacher>().OrderBy(x => x.Score).Asc.Take(10).List();
        }

        private static void getAllUniversities(ISession session)
        {
            IList<University> allUniversities = session.QueryOver<University>().List();
        }

        private static void getAllCoursesPerUniversity(ISession session, University university)
        {
            IList<Course> allCourses = session.QueryOver<Course>().Where(x => x.University == university).List();
        }

        private static void getAllTeacherPerUniversity(ISession session, University university)
        {
            IList<Teacher> allTeachers = session.QueryOver<Teacher>().Where(x => x.Universities.Contains(university)).List();
        }

        private static void getAllTeacherPerCourse(ISession session, Course course)
        {
            IList<Teacher> allTeachers = session.QueryOver<Teacher>().Where(x => x.Courses.Contains(course)).List();
        }



    }
}