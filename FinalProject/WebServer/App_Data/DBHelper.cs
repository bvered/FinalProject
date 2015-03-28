using NHibernate;

namespace WebServer.App_Data
{
    public class DBHelper
    {
        private static ISessionFactory _sessionFactory;

        public static void Load()
        {
            _sessionFactory = NHibernateConfig.CreateSessionFactory();
        }

        public static ISession OpenSession()
        {
            return _sessionFactory.OpenSession();
        }
    }
}