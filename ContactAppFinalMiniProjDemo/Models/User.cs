using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactAppFinalMiniProjDemo.Models
{
    public class User
    {
        public virtual Guid Id { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Password { get; set; }

        public virtual string FName { get; set; }

        public virtual string LName { get; set; }

        public virtual string Email { get; set; }

        public virtual bool IsAdmin { get; set; }

        public virtual bool IsActive { get; set; }

        //nav property
        public virtual IList<Contact> Contacts { get; set; } = new List<Contact>();

        //nav 
        public virtual Role Role { get; set; } = new Role();
    }
}