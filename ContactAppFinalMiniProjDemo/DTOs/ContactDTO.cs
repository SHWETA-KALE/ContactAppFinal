using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactAppFinalMiniProjDemo.DTOs
{
    public class ContactDTO
    {
        public Guid Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public bool IsActive { get; set; }
    }
}