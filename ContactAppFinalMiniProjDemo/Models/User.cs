using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactAppFinalMiniProjDemo.Models
{
    public class User
    {
        public virtual Guid Id { get; set; }

        [Required] 
        public virtual string UserName { get; set; }

        [Required]
        public virtual string Password { get; set; }

        [Required]
        public virtual string FName { get; set; }

        [Required]
        public virtual string LName { get; set; }

        [Required]
        public virtual string Email { get; set; }

        public virtual bool IsAdmin { get; set; }

        public virtual bool IsActive { get; set; }

        //nav property
        public virtual IList<Contact> Contacts { get; set; } = new List<Contact>();

        //nav 
        public virtual Role Role { get; set; } = new Role();
    }
}