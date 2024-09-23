using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactAppFinalMiniProjDemo.Models
{
    public class Contact
    {
        public virtual Guid Id { get; set; }

        public virtual string FName { get; set; }
        
        public virtual string LName { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual User User { get; set; }
        public virtual IList<ContactDetails> ContactDetails { get; set; } = new List<ContactDetails>();
    }
}