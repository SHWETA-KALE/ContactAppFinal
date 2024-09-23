using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactAppFinalMiniProjDemo.Models
{
    public class ContactDetails
    {
        public virtual Guid Id { get; set; }
        public virtual long PhoneNumber { get; set; }

        public virtual string Email { get; set; }
        public virtual Contact Contact { get; set; }
    }
}