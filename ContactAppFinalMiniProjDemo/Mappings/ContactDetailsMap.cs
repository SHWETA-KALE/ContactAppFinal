using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactAppFinalMiniProjDemo.Models;
using FluentNHibernate.Mapping;

namespace ContactAppFinalMiniProjDemo.Mappings
{
    public class ContactDetailsMap : ClassMap<ContactDetails>
    {
        public ContactDetailsMap()
        {
            Table("ContactDetails");
            Id(cd => cd.Id).GeneratedBy.GuidComb();
            Map(cd => cd.PhoneNumber);
            Map(cd => cd.Email);
            References(cd => cd.Contact).Column("ContactId").Cascade.None().Nullable();
        }
    }
}