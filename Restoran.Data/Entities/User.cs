
using Restoran.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;


namespace Restoran.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public RoleType Role { get; set; }
        
               
    }
}
