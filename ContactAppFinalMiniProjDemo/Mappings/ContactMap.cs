using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactAppFinalMiniProjDemo.Models;
using FluentNHibernate.Mapping;

namespace ContactAppFinalMiniProjDemo.Mappings
{
    public class ContactMap : ClassMap<Contact>
    {
        public ContactMap()
        {
            Table("Contacts");
            Id(c => c.Id).GeneratedBy.GuidComb();
            Map(c => c.FName);
            Map(c => c.LName);
            Map(c => c.IsActive);
            References(c => c.User).Column("UserId").Cascade.None().Nullable();
            HasMany(cd => cd.ContactDetails).Inverse().Cascade.All();

        }
    }
}