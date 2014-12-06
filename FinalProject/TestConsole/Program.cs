using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var sessionFactory = NHibernateConfig.CreateSessionFactory();
        }
    }
}
