using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        [Route("getAllUsers")]
        public String Index()
        {
            using (OurDBContext db = new OurDBContext())
            {
                StringBuilder result = new StringBuilder();
                foreach(UserAccount userAccount in db.userAccount.ToList())
                {
                    result.Append(userAccount.Username + userAccount.Description + ", ");
                }
                return result.ToString();
            }
        }

        [HttpPost]
        [Route("register")]
        public UserAccount Register(UserAccount account)
        {
            if (ModelState.IsValid)
            {
                using (OurDBContext db = new OurDBContext())
                {
                    db.userAccount.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.Username + " successfully registered"; ;
            }
            return account;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public ActionResult Login(String username, String password)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.Username == username && u.Password == password);
                if(usr != null)
                {
                    Session["UserID"] = usr.UserID.ToString();
                    Session["Username"] = usr.Username.ToString();
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("", "Username ot Password is wrong.");
                }
                return null;
            }
        }

        public ActionResult LoggedIn()
        {
            if(Session["UserId"] != null)
            {
                return null;
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [Route("logout")]
        public String Logout(String username, String password)
        {
            Session["UserId"] = null;
            return "Logout";
        }

        [Route("changePassword")]
        public String changePassword(String username, String password, String newPassword)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.UserID.ToString().Equals(Session["UserId"].ToString()));
                usr.Password = newPassword;
                return "changed password";
            }
        }

        [Route("changeDescription")]
        public String changeDescription(String newDescription)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.UserID.ToString().Equals(Session["UserId"].ToString()));
                usr.Description = newDescription;
                return "changed description";
            }
        }

        [Route("subscribe")]
        public String subscribe(String username)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.UserID.ToString().Equals(Session["UserId"].ToString()));
                var subscription = db.userAccount.Single(u => u.Username == username);
                if (!subscription.blockedUsers.Contains(usr))
                {
                    subscription.subscribers.Add(usr);
                    usr.subscriptions.Add(subscription);
                }
            }
            return "subscribed to " + username;
        }

        [Route("unsubscribe")]
        public String unsubscribe(String username)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.UserID.ToString().Equals(Session["UserId"].ToString()));
                var subscription = db.userAccount.Single(u => u.Username == username);
                    subscription.subscribers.Remove(usr);
                    usr.subscriptions.Remove(subscription);
            }
            return "subscribed to " + username;
        }

        [Route("publishMessage")]
        public String publish(String message)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.UserID.ToString().Equals(Session["UserId"].ToString()));
                usr.messages.Add(message);
            }
            return "message is published";
        }

        [Route("getSubscribers")]
        public String getSubscribers()
        {
            using (OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.UserID.ToString().Equals(Session["UserId"].ToString()));
                StringBuilder result = new StringBuilder();
                foreach (UserAccount userAccount in usr.subscribers)
                {
                    foreach (String message in userAccount.messages)
                    {
                        result.Append(message + ", ");
                    };
                }
                return result.ToString();
            }
        }

        [Route("sendMessage")]
        public String sendMessage(String message, String username)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.UserID.ToString().Equals(Session["UserId"].ToString()));
                var receiver = db.userAccount.Single(u => u.Username.Equals(username));
                usr.sentMessages.Add(message, receiver);
                receiver.personalMessages.Add(message, usr);
            }
            return "message " + message + " is sent";
        }

        [Route("blockUser")]
        public String blockUser(String username)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.UserID.ToString().Equals(Session["UserId"].ToString()));
                var userToBlock = db.userAccount.Single(u => u.Username.Equals(username));
                usr.blockedUsers.Add(userToBlock);
            }
            return "username " + username + " is blocked";
        }

        [Route("changeMessage")]
        public String changeMessage(String message, String newMessage, UserAccount userAccount)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.UserID.ToString().Equals(Session["UserId"].ToString()));
                usr.sentMessages.Remove(message);
                usr.sentMessages.Add(newMessage, userAccount);
                userAccount.personalMessages.Add(newMessage, usr);
                userAccount.personalMessages.Remove(message);
            }
            return "message " + message + " to " + userAccount.Username + " was changed";
        }

        [Route("deleteMessage")]
        public String deleteMessage(String message, UserAccount userAccount)
        {
            using (OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.UserID.ToString().Equals(Session["UserId"].ToString()));
                usr.sentMessages.Remove(message);
                userAccount.personalMessages.Remove(message);
            }
            return "message " + message + " to " + userAccount.Username + " was deleted";
        }
    }
}