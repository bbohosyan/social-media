using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AccountController : ApiController
    {
        [HttpGet]
        [Route("getAllUsers")]
        public List<UserAccount> Index()
        {
            using (OurDBContext db = new OurDBContext())
            {
                return db.userAccount.ToList();
            }
        }

        [HttpPost]
        [Route("register")]
        public UserAccount Register([FromBody]UserAccount account)
        {
            if (ModelState.IsValid)
            {
                using (OurDBContext db = new OurDBContext())
                {
                    db.userAccount.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
            }
            return account;
        }

        [HttpPost]
        [Route("login")]
        public UserAccount Login([FromBody]LoginParameters loginParameters)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.Username == loginParameters.username && u.Password == loginParameters.password);
                if(usr != null)
                {
                    if(db.session.ToList().Count == 0)
                    {
                        db.session.Add(new Session());
                        db.SaveChanges();
                    }
                    db.session.ToList()[0].UserID = usr.UserID;
                    db.session.ToList()[0].Username = usr.Username;
                    return usr;
                }
                else
                {
                    ModelState.AddModelError("", "Username ot Password is wrong.");
                }
                return null;
            }
        }

        [HttpPost]
        [Route("logout")]
        public UserAccount Logout()
        {
            using (OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.UserID == db.session.ToList()[0].UserID);
                db.session.ToList()[0].UserID = -1;
                return usr;
            }
        }

        [HttpPut]
        [Route("changePassword")]
        public String changePassword([FromBody]String newPassword)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var session = HttpContext.Current.Session;
                var usr = db.userAccount.Single(u => u.UserID == db.session.ToList()[0].UserID);
                usr.Password = newPassword;
                return newPassword;
            }
        }

        [HttpPut]
        [Route("changeDescription")]
        public String changeDescription([FromBody]String newDescription)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var session = HttpContext.Current.Session;
                var usr = db.userAccount.Single(u => u.UserID == db.session.ToList()[0].UserID);
                usr.Description = newDescription;
                return newDescription;
            }
        }

        [HttpPost]
        [Route("subscribe")]
        public UserAccount subscribe([FromBody]String username)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var session = HttpContext.Current.Session;
                var usr = db.userAccount.Single(u => u.UserID == db.session.ToList()[0].UserID);
                var subscription = db.userAccount.Single(u => u.Username == username);
                if (!subscription.blockedUsers.Contains(usr))
                {
                    subscription.subscribers.Add(usr);
                    usr.subscriptions.Add(subscription);
                }
                return subscription;
            }
        }

        [HttpDelete]
        [Route("unsubscribe")]
        public UserAccount unsubscribe([FromBody]String username)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.UserID == db.session.ToList()[0].UserID);
                var subscription = db.userAccount.Single(u => u.Username == username);
                    subscription.subscribers.Remove(usr);
                    usr.subscriptions.Remove(subscription);
                return subscription;
            }
        }

        [HttpPost]
        [Route("publishMessage")]
        public String publish([FromBody]String message)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.UserID == db.session.ToList()[0].UserID);
                usr.messages.Add(message);
            }
            return message;
        }

        [HttpGet]
        [Route("getSubscribers")]
        public List<UserAccount> getSubscribers()
        {
            using (OurDBContext db = new OurDBContext())
            {
                var session = HttpContext.Current.Session;
                var usr = db.userAccount.Single(u => u.UserID == db.session.ToList()[0].UserID);
                return usr.subscribers;
            }
        }

        [HttpPost]
        [Route("sendMessage")]
        public String sendMessage([FromBody]SendMessageParameters sendMessageParameters)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.UserID == db.session.ToList()[0].UserID);
                var receiver = db.userAccount.Single(u => u.Username.Equals(sendMessageParameters.Username));
                usr.sentMessages.Add(sendMessageParameters.Message, receiver);
                receiver.personalMessages.Add(sendMessageParameters.Message, usr);
            }
            return sendMessageParameters.Message;
        }

        [HttpPost]
        [Route("blockUser")]
        public UserAccount blockUser([FromBody]String username)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.UserID == db.session.ToList()[0].UserID);
                var userToBlock = db.userAccount.Single(u => u.Username.Equals(username));
                usr.blockedUsers.Add(userToBlock);
                return userToBlock;
            }
        }

        [HttpPut]
        [Route("changeMessage")]
        public String changeMessage([FromBody]ChangeMessageParameters changeMessageParameters)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.UserID == db.session.ToList()[0].UserID);
                usr.sentMessages.Remove(changeMessageParameters.Message);
                usr.sentMessages.Add(changeMessageParameters.NewMessage, changeMessageParameters.UserAccount);
                changeMessageParameters.UserAccount.personalMessages.Add(changeMessageParameters.NewMessage, usr);
                changeMessageParameters.UserAccount.personalMessages.Remove(changeMessageParameters.Message);
            }
            return changeMessageParameters.Message;
        }

        [HttpDelete]
        [Route("deleteMessage")]
        public String deleteMessage([FromBody]String message, [FromBody]UserAccount userAccount)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.UserID == db.session.ToList()[0].UserID);
                usr.sentMessages.Remove(message);
                userAccount.personalMessages.Remove(message);
            }
            return message;
        }
    }
}