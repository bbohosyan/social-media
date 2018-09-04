using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class UserAccount
    {
        [Key]
        public int UserID { get; set; }

        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Description { get; set; }

        public string Email { get; set; }

        public List<UserAccount> subscriptions { get; set; }

        public List<UserAccount> subscribers { get; set; }

        public List<String> messages { get; set; }

        public Dictionary<String, UserAccount> personalMessages { get; set; }

        public Dictionary<String, UserAccount> sentMessages { get; set; }

        public List<UserAccount> blockedUsers { get; set; }
    }
}