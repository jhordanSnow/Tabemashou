using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Microsoft.Owin.Security;
using Tabemashou_Admin.Models;

namespace Tabemashou_Admin.Controllers
{
    public class AccountController : Controller
    {
        private TabemashouEntities db = new TabemashouEntities();

        IAuthenticationManager Authentication
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        // GET: Account
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                bool isValidUser = Membership.ValidateUser(model.Username, model.Password);
                if (isValidUser)
                {
                    User user = null;
                    using (TabemashouEntities dc = new TabemashouEntities())
                    {
                        dc.Configuration.ProxyCreationEnabled = false;
                        user = dc.User.FirstOrDefault(a => a.Username.Equals(model.Username));
                    }

                    if (user != null)
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        string data = js.Serialize(user);
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.Username, DateTime.Now, DateTime.Now.AddMinutes(30),model.RememberMe, data);
                        string encToken = FormsAuthentication.Encrypt(ticket);
                        HttpCookie authoCookies = new HttpCookie(FormsAuthentication.FormsCookieName, encToken);
                        Response.Cookies.Add(authoCookies);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        [AllowAnonymous]
        public ActionResult UserProfile()
        {

            var identity = (System.Web.HttpContext.Current.User as MyIdentity.MyPrincipal).Identity as MyIdentity;
            User loggedUser = db.User.Find(identity.User.IdCard);
            ProfileEditViewModel model = new ProfileEditViewModel();

            model.profileData = new ProfileViewModel();

            model.profileData.IdCard = loggedUser.IdCard;
            model.profileData.FirstName = loggedUser.FirstName;
            model.profileData.LastName = loggedUser.LastName;
            model.profileData.BirthDate = loggedUser.BirthDate;
            model.profileData.Nationality = loggedUser.Nationality;
            model.profileData.Username = loggedUser.Username;
            model.profileData.MiddleName = loggedUser.MiddleName;
            model.profileData.SecondLastName = loggedUser.SecondLastName;
            model.profileData.Gender = loggedUser.Gender;

            ViewBag.Nationality = new SelectList(db.Country, "IdCountry", "Name", loggedUser.Nationality);
            return View(model);
        }

        [HttpPost]
        public ActionResult UserProfile(ProfileEditViewModel model)
        {
            bool error = false;
            var identity = (System.Web.HttpContext.Current.User as MyIdentity.MyPrincipal).Identity as MyIdentity;
            User loggedUser = db.User.FirstOrDefault(a => a.IdCard.Equals(model.profileData.IdCard));
            error = (model.profileData.IdCard != identity.User.IdCard) ? !ValidateIdCard(model.profileData.IdCard) : error;
            error = (model.profileData.Username != identity.User.Username && !error) ? !ValidateUserName(model.profileData.Username) : error;

            if (!error && ModelState.IsValid)
            {
                loggedUser.Username = model.profileData.Username;
                loggedUser.FirstName = model.profileData.FirstName;
                loggedUser.MiddleName = model.profileData.MiddleName;
                loggedUser.LastName= model.profileData.LastName;
                loggedUser.SecondLastName = model.profileData.SecondLastName;
                loggedUser.IdCard = model.profileData.IdCard;
                loggedUser.BirthDate= model.profileData.BirthDate;
                loggedUser.Gender= model.profileData.Gender;
                loggedUser.Nationality = model.profileData.Nationality;
                db.Entry(loggedUser).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Profile correctly updated.";
                return RedirectToAction("UserProfile", "Account");
            }
            ViewBag.Nationality = new SelectList(db.Country, "IdCountry", "Name", loggedUser.Nationality);
            return View(model);
        }

        public ActionResult ChangePassword(ProfileEditViewModel model)
        {
            bool error = false;
            var identity = (System.Web.HttpContext.Current.User as MyIdentity.MyPrincipal).Identity as MyIdentity;
            User loggedUser = db.User.FirstOrDefault(a => a.IdCard.Equals(model.profileData.IdCard));

            if (ModelState.IsValid && hashPassword(model.changePass.OldPassword) != loggedUser.Password)
            {
                TempData["Error"] = "Current Password don't match";
                error = true;
            }

            if (ModelState.IsValid && !error)
            {
                loggedUser.Password = hashPassword(model.changePass.NewPassword);
                db.Entry(loggedUser).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Profile correctly updated.";
                return RedirectToAction("UserProfile", "Account");
            }

            model.profileData = new ProfileViewModel();

            model.profileData.IdCard = loggedUser.IdCard;
            model.profileData.FirstName = loggedUser.FirstName;
            model.profileData.LastName = loggedUser.LastName;
            model.profileData.BirthDate = loggedUser.BirthDate;
            model.profileData.Nationality = loggedUser.Nationality;
            model.profileData.Username = loggedUser.Username;
            model.profileData.MiddleName = loggedUser.MiddleName;
            model.profileData.SecondLastName = loggedUser.SecondLastName;
            model.profileData.Gender = loggedUser.Gender;

            ViewBag.Nationality = new SelectList(db.Country, "IdCountry", "Name", loggedUser.Nationality);
            return View("UserProfile", model);
        }
    
      
        public bool UsernameInDb(string username)
        {
            var checkUsername = (from userCheck in db.User where userCheck.Username == username select "count(*)");
            return (checkUsername.Any());
        }

        public bool ValidateUserName(string username)
        {
            if (UsernameInDb(username))
            {
                TempData["Error"] = "The Username '" + username + "' is already taken.";
                ModelState.AddModelError("", "The Username '" + username + "' is already taken.");
                return false;
            }
            return true;
        }


        public bool ValidateIdCard(decimal idcard)
        {
            var checkId = (from userCheckId in db.User where userCheckId.IdCard == idcard select "count(*)");
            if (checkId.Any())
            {
                TempData["Error"] = "There is another user with Identification Card: " + idcard + ".";
                ModelState.AddModelError("", "There is another user with Identification Card: " + idcard + ".");
                return false;
            }
            return true;
        }

        [AllowAnonymous]
        public ActionResult Register(string returnUrl)
        {
            ViewBag.Nationality = new SelectList(db.Country, "IdCountry", "Name");
            return View(new RegisterViewModel());
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid && ValidateUserName(model.Username) && ValidateIdCard(model.IdCard))
            {

                int query = db.PR_CreateAdministrator(model.IdCard, model.Username, hashPassword(model.Password), model.Gender, model.BirthDate,
                                          model.Nationality, model.FirstName, model.MiddleName, model.LastName, model.SecondLastName);
                TempData["Success"] = "Success.";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Nationality = new SelectList(db.Country, "IdCountry", "Name", model.Nationality);
            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [Authorize]
        public ActionResult LogOff()
        {
            Authentication.SignOut();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }


        //MD5 Hash
        public string hashPassword(string inputPassword)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            Byte[] hashedBytes = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(inputPassword));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                sBuilder.Append(hashedBytes[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}