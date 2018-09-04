using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Session
    {
        [Key]
        public int UserID { get; set; }
        public String Username { get; set; }
    }
}