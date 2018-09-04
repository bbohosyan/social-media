using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ChangeMessageParameters
    {
        //String message, [FromBody]String newMessage, [FromBody]UserAccount userAccount
        public String Message { get; set; }
        public String NewMessage { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}