using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using NHibernate;
using ContactAppFinalMiniProjDemo.Mappings;
using NHibernate.Tool.hbm2ddl;

namespace ContactAppFinalMiniProjDemo.Data
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory = null;

        public static ISession CreateSession()
        {
            if (_sessionFactory == null)
            {
                _sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ContactAppMiniProjDemoDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;"))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
                .BuildSessionFactory();
            }
            return _sessionFactory.OpenSession();
        }
    }
}