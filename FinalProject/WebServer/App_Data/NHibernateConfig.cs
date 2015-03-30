using System.IO;
using System.Reflection;
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
        private static readonly string DBFile = Path.Combine(Path.GetTempPath(), "ProjectDB.db");

        public static ISessionFactory CreateSessionFactory(bool create = false)
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(DBFile))
                .Mappings(m => m.AutoMappings.Add(CreateMappings()))
                .ExposeConfiguration(x => BuildSchema(x, create))
                .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config, bool create = false)
        {
            if (create || !File.Exists(DBFile))
            {
                new SchemaExport(config).Create(false, true);
            }
        }

        private static AutoPersistenceModel CreateMappings()
        {
            return AutoMap.Assembly(Assembly.GetCallingAssembly())
                .Where(t => typeof (IPersistent).IsAssignableFrom(t))
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