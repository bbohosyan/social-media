using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class OurDBContext : DbContext
    {
        public DbSet<UserAccount> userAccount { get; set; }
        public DbSet<Session> session { get; set; }
    }
}