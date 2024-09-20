using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactAppFinalMiniProjDemo.Models;
using FluentNHibernate.Mapping;

namespace ContactAppFinalMiniProjDemo.Mappings
{
    public class UserMap: ClassMap<User>
    {
        public UserMap()
        { 
            Table("Users");
            Id(u => u.Id).GeneratedBy.GuidComb();
            Map(u => u.UserName);
            Map(u => u.Password);
            Map(u => u.FName);
            Map(u => u.LName);
            Map(u => u.Email);
            Map(u => u.IsActive);
            Map(u => u.IsAdmin);
            HasOne(r => r.Role).Cascade.All().PropertyRef(r => r.User).Constrained();
            HasMany(c => c.Contacts).Inverse().Cascade.All();
        }
    }
}