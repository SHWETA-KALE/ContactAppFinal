using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactAppFinalMiniProjDemo.Models;
using FluentNHibernate.Mapping;

namespace ContactAppFinalMiniProjDemo.Mappings
{
    public class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            Table("Roles");
            Id(r => r.Id).GeneratedBy.GuidComb();
            Map(r => r.RoleName);
            //one to one
            References(r => r.User)
                .Column("UserId")
                .Unique()
                .Cascade.None();
        }
    }
}