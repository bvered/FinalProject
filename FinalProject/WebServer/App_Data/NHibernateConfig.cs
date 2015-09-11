using System.IO;
using System.Reflection;
using System.Web.Hosting;
using System.Web.UI.WebControls;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Instances;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using WebServer.App_Data.Models;

namespace WebServer.App_Data
{
    public static class NHibernateConfig
    {
        private static string GetDBFilePath()
        {
            return HostingEnvironment.MapPath("~/App_Data/ProjectDB.db");
        }

        public static ISessionFactory CreateSessionFactory(bool create = false, string filePath = null)
        {
            filePath = filePath ?? GetDBFilePath();

            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(filePath))
                .Mappings(m => m.AutoMappings.Add(CreateMappings()))
                .ExposeConfiguration(x => BuildSchema(x, filePath, create))
                .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config, string filePath, bool create = false)
        {
            if (create)
            {
                new SchemaExport(config).Create(false, true);
            }
            else if (File.Exists(filePath))
            {
                new SchemaUpdate(config).Execute(true, true);                
            }
        }

        private static AutoPersistenceModel CreateMappings()
        {
            return AutoMap.Assembly(Assembly.GetCallingAssembly())
                .Where(t => typeof(IPersistent).IsAssignableFrom(t))
                .Conventions.Setup(c =>
                {
                    c.Add(DefaultCascade.SaveUpdate());
                    c.Add(DefaultLazy.Never());
                    c.Add(new PersistentConvention());
                })
                .UseOverridesFromAssembly((Assembly.GetCallingAssembly()));
        }

        internal class PersistentConvention : IIdConvention
        {
            public void Apply(IIdentityInstance instance)
            {
                instance.GeneratedBy.Guid();
            }

            public bool Accept(IIdentityInstance id)
            {
                return true;
            }
        }
    }
}