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

            Teacher.getBestTeachers(session);

            University universityWithId = session.Get<University>(universityId);
            IList<User> allUsers = session.QueryOver<User>().List();
            IList<string> allUserFullNames = session.QueryOver<User>().Select(x => x.FullName).List<string>();

            Comment.getReportedComments(session);

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
            List<TeacherCriteria> teacherCritias = new List<TeacherCriteria>();
            teacherCritias.Add(new TeacherCriteria("Student- teacher relationship"));
            teacherCritias.Add(new TeacherCriteria("Teaching ability"));
            teacherCritias.Add(new TeacherCriteria("Teachers knowlegde level"));
            teacherCritias.Add(new TeacherCriteria("The teacher Encouregment for self learning"));
            teacherCritias.Add(new TeacherCriteria("The teacher interest level"));
        }

        private void addCourseCritiries()
        {
            List<CourseCriteria> courseCritias = new List<CourseCriteria>();
            courseCritias.Add(new CourseCriteria("Material ease"));
            courseCritias.Add(new CourseCriteria("Time investment for home-work"));
            courseCritias.Add(new CourseCriteria("Number of home-work submissions"));
            courseCritias.Add(new CourseCriteria("Time invesment for test learning"));
            courseCritias.Add(new CourseCriteria("Course usability"));
            courseCritias.Add(new CourseCriteria("Course grades average"));
            courseCritias.Add(new CourseCriteria("Does the attendance is mandatory"));
            courseCritias.Add(new CourseCriteria("Does the test has open material/reference Pages"));
        }
    }
}