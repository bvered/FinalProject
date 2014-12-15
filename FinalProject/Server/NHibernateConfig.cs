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

namespace Server
{
    public static class NHibernateConfig
    {
        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(c => c.FromConnectionStringWithKey("ConnectionString")))
                .Mappings(m => m.AutoMappings.Add(CreateMappings()))
                .ExposeConfiguration(DropCreateSchema)
                .BuildSessionFactory();
        }

        private static AutoPersistenceModel CreateMappings()
        {
            return AutoMap.Assembly(Assembly.GetCallingAssembly())
                .Where(t => typeof (IPersistent).IsAssignableFrom(t))
                .Conventions.Setup(c =>
                {
                    c.Add(DefaultCascade.SaveUpdate());
                    c.Add(DefaultLazy.Never());
                    c.Add<PersistentConvention>();
                })
                .UseOverridesFromAssembly((Assembly.GetCallingAssembly()));
        }

        private static void DropCreateSchema(Configuration cfg)
        {
            new SchemaExport(cfg).Create(true, true);
        }

        internal class PersistentConvention : IIdConvention
        {
            public void Apply(IIdentityInstance instance)
            {
                instance.GeneratedBy.Guid();
            }
        }
    }
}