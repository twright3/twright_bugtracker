using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using twright_bugtracker.Helpers;
using twright_bugtracker.Models;
using twright_bugtracker.ViewModels;

namespace twright_bugtracker.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: User
        public ActionResult Index()
        {
            return View();

        }

        [Authorize]
        public ActionResult EditProfile()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var myUserViewModel = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfilePic = user.ProfilePic
            };


            return View(myUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(UserViewModel user, HttpPostedFileBase avatar)
        {
            var newUser = db.Users.Find(user.Id);
            if (user.Avatar != null)
            {
                if (AvatarUploadValidator.IsWebFriendlyImage(user.Avatar))
                {
                    var fileName = Path.GetFileName(user.Avatar.FileName).Replace(' ', '_');
                    user.Avatar.SaveAs(Path.Combine(Server.MapPath("~/Avatar/"), fileName));
                    newUser.ProfilePic = "/Avatar/" + fileName;
                }
            }

            var userId = User.Identity.GetUserId();
            newUser.FirstName = user.FirstName;
            newUser.LastName = user.LastName;
            newUser.Email = user.Email;
            newUser.UserName = user.Email;
            user.ProfilePic = user.ProfilePic;


            db.SaveChanges();
            return RedirectToAction("Dashboard", "Home");
        }
    }
}