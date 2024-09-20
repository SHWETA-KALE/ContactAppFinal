using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactAppFinalMiniProjDemo.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
    }
}