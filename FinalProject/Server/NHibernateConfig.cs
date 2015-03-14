using System;
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
using Server.Models;
using System.IO;

namespace Server
{
    public static class NHibernateConfig
    {
        private const string _dbFile = "ProjectDB.db";
        public static bool _override = false;

        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(_dbFile)                )
                .Mappings(m=> m.AutoMappings.Add(CreateMappings()))
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
            if (_override || !File.Exists(_dbFile))
            {
                var se = new SchemaExport(config);

                se.Create(false, true);
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

        private static void DropCreateSchema(Configuration cfg)
        {
            new SchemaExport(cfg).Create(true, true);
        }

        internal class PersistentConvention : IIdConvention
        {
            public bool Accept(IIdentityInstance id)
            {
                return true;
            }

            public void Apply(IIdentityInstance instance)
            {
                instance.GeneratedBy.Guid();
            }
        }
    }
}