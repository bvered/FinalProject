using System;
using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Server.Models;

namespace Server
{
    public static class NHibernateConfig
    {
      //  private const string DatabaseFile = "FinalProjectDB.accdb";

        public static ISessionFactory CreateSessionFactory()
        {

          return Fluently.Configure()
   .Database(MsSqlConfiguration.MsSql2012.ConnectionString(c => c.FromConnectionStringWithKey("ConnectionString")))
      .Mappings(AddMappings)
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();

            /*
            return Fluently.Configure()
                .Database(JetDriverConfiguration.Standard.ConnectionString(SetAccessDbFile))
                .Mappings(AddMappings)
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory(); */
        }

  /*      private static void SetAccessDbFile(JetDriverConnectionStringBuilder c)
        {
            c.DatabaseFile(DatabaseFile);
        } */

        private static void AddMappings(MappingConfiguration m)
        {
            m.AutoMappings.Add(CreateMappings);
        }

        private static AutoPersistenceModel CreateMappings()
        {
            return
                new AutoPersistenceModel().AddMappingsFromAssemblyOf<Course>()
                    .Where(CheckIsIPersistent)
                    .Conventions.Add<PersistentConvention>();
        }

        private static bool CheckIsIPersistent(Type t)
        {
            return typeof (IPersistent).IsAssignableFrom(t);
        }

        private static void BuildSchema(Configuration config)
        {
            new SchemaExport(config).Create(true, true);
            //new SchemaExport(config).Create(true, false);
        }
    }

    internal class PersistentConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.GeneratedBy.Guid();
        }
    }
}