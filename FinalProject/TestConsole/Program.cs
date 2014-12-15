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
            User newUser = new User
            {
                LoginEmail = "vered631gmailcom",
                BirthDate = new DateTime(1991, 07, 03),
                FullName = "veredb",
                PasswordHash = "123456",
            };

            University newUniversity = new University("Tel-aviv_yafo");
            Faculty newFaculty = new Faculty("Computer", newUniversity);
            Course newCourse = new Course(newUniversity, 123456, "Math", newFaculty);

            createTeacher(newCourse, newUniversity);


            var sessionFactory = NHibernateConfig.CreateSessionFactory();
            Console.WriteLine(sessionFactory.ToString());

            ISession session = sessionFactory.OpenSession();

            session.Save(newUser);
           
            session.Flush();

            Guid universityId = Guid.NewGuid();

            University universityWithId = session.Get<University>(universityId);
            IList<User> allUsers = session.QueryOver<User>().List();
            IList<string> allUserFullNames = session.QueryOver<User>().Select(x => x.FullName).List<string>();
            IList<CourseComment> courseCommentWithMoreThen5Reports = session.QueryOver<CourseComment>()
                                                                            .Where(x => x.Reports > 5).List();

            session.Close();
        }

        private static void createTeacher(Course newCourse, University newUniversity)
        {
            Teacher newTeacher = new Teacher("Romina");
            newTeacher.addCourse(newCourse);
            newTeacher.addUniversity(newUniversity);
        }
    }
}