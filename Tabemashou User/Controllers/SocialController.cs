    using System;
using System.Collections.Generic;
    using System.Data.Entity.Core.Objects;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Web;
using System.Web.Mvc;
    using Tabemashou_User.Models;

namespace Tabemashou_User.Controllers
{
    public class SocialController : Controller
    {
        private TabemashouEntities db = new TabemashouEntities();

        public ActionResult Followers()
        {
            var idCard = ((System.Web.HttpContext.Current.User as MyIdentity.MyPrincipal).Identity as MyIdentity).User.IdCard;
            var model = new FollowersViewModel();
            List<PR_GetFollowers_Result> customers = db.PR_GetFollowers(idCard).ToList();
            if (customers.Count == 0)
            {
                TempData["NoFollowers"] = "You do not have any subscriber yet :(";
            }
            List<ProfileModel> profileModels = new List<ProfileModel>();
            foreach (var account in customers)
            {
                var profile = new ProfileModel();
                profile.IdCard = account.IdCard;
                profile.Username = account.Username;
                profile.FirstName = account.FirstName;
                profile.LastName = account.LastName;
                profile.LastName = account.LastName;

                ObjectParameter followersQty = new ObjectParameter("Qty", typeof(int));
                db.PR_GetFollowersCount(account.IdCard, followersQty);
                profile.Followers = (int)followersQty.Value;
                profile.Reviews = db.Customer.Find(account.IdCard).Review.Count;
                profileModels.Add(profile);
            }
            model.Customers = profileModels;
            return View(model);
        }

        public ActionResult Following()
        {
            var idCard = ((System.Web.HttpContext.Current.User as MyIdentity.MyPrincipal).Identity as MyIdentity).User.IdCard;
            var model = new FollowersViewModel();
            List<PR_GetFriends_Result> customers = db.PR_GetFriends(idCard).ToList();
            List<ProfileModel> profileModels = new List<ProfileModel>();
            foreach (var account in customers)
            {
                var profile = new ProfileModel();
                profile.IdCard = account.IdCard;
                profile.Username = account.Username;
                profile.FirstName = account.FirstName;
                profile.LastName = account.LastName;
                profile.LastName = account.LastName;

                ObjectParameter followersQty = new ObjectParameter("Qty", typeof(int));
                db.PR_GetFollowersCount(account.IdCard, followersQty);
                profile.Followers = (int)followersQty.Value;
                profile.Reviews = db.Customer.Find(account.IdCard).Review.Count;
                profileModels.Add(profile);
            }
            model.Customers = profileModels;
            return View(model);
        }

        public ActionResult Profile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            Customer customer = db.Customer.Find(id);
            if (user == null && !IsACustomer(id))
            {
                return HttpNotFound();
            }
            var idCard = ((System.Web.HttpContext.Current.User as MyIdentity.MyPrincipal).Identity as MyIdentity).User.IdCard;
            if (user.IdCard == idCard)
            {
                return RedirectToAction("UserProfile", "Account");
            }

            ProfileModel model = new ProfileModel();
            model.IdCard = user.IdCard;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Birth = user.BirthDate.ToString("dd MMMM yyyy");
            model.Nationality = db.Country.Find(user.Nationality).Name;
            model.Username = user.Username;
            model.MiddleName = user.MiddleName;
            model.SecondLastName = user.SecondLastName;
            model.Gender = (user.Gender == "M") ? "Male" : "Female";
            ObjectParameter followersQty = new ObjectParameter("Qty", typeof(int));
            db.PR_GetFollowersCount(id, followersQty);
            model.Followers = (int)followersQty.Value;

            ObjectParameter followingQty = new ObjectParameter("Qty", typeof(int));
            db.PR_GetFriendsCount(id, followingQty);
            model.Following = (int) followingQty.Value;
            model.Reviews = customer.Review.Count;

            model.Timeline = customer.Review.OrderByDescending( r => r.Date).ToList();
            return View(model);
        }

        [HttpPost]
        public JsonResult Follow(int? id)
        {
            if (id == null)
            {
                Json(false);
            }
            User friendUser = db.User.Find(id);
            Customer friendCustomer = db.Customer.Find(id);
            if (friendUser == null && !IsACustomer(id))
            {
                return Json(new JsonResponse { Success = false, Message = "Not a valid user." });
            }
            var identity = ((System.Web.HttpContext.Current.User as MyIdentity.MyPrincipal).Identity as MyIdentity);
            Customer loggedCustomer = db.Customer.Find(identity.User.IdCard);
            loggedCustomer.Customer1.Add(friendCustomer);
            db.SaveChanges();
            return Json(true);
        }

        public JsonResult Unfollow(int? id)
        {
            if (id == null)
            {
                return Json(new JsonResponse { Success = false});
            }
            User friendUser = db.User.Find(id);
            Customer friendCustomer = db.Customer.Find(id);
            if (friendUser == null && !IsACustomer(id))
            {
                return Json(new JsonResponse { Success = false, Message = "Not a valid user."});
            }
            var identity = ((System.Web.HttpContext.Current.User as MyIdentity.MyPrincipal).Identity as MyIdentity);
            Customer loggedCustomer = db.Customer.Find(identity.User.IdCard);
            loggedCustomer.Customer1.Remove(friendCustomer);
            db.SaveChanges();
            return Json(new JsonResponse { Success = true });

        }

        public FileContentResult ShowProfilePicture(int id)
        {
            Customer customer = db.Customer.Find(id);
            var imagedata = customer?.Photo;
            if (imagedata != null && imagedata.Length > 0) return File(imagedata, "image/jpg");
            string path = Server.MapPath("~/AdminLTE/dist/img/user.svg");
            byte[] array;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                array = new byte[fs.Length];
                fs.Read(array, 0, (int)fs.Length);
            }
            return File(array, "image/jpg");
        }

        public bool IsACustomer(int? id)
        {
            return (db.Customer.Find(id) != null);
        }

        public JsonResult FindCustomerByUsername(string name)
        {

            User user = db.User.FirstOrDefault(m => m.Username == name);
            if (user != null && IsACustomer((int)user.IdCard))
            {
                return Json(user.IdCard);
            }
            throw new Exception(null);
        }

        public bool IsFollowing(int id)
        {
            var idCard = ((System.Web.HttpContext.Current.User as MyIdentity.MyPrincipal).Identity as MyIdentity).User.IdCard;
            return db.PR_GetFriends(idCard).Any(n => n.IdCard == id);
        }

        public List<PR_GetFriends_Result> GetMyFollowers()
        {
            var idCard = ((System.Web.HttpContext.Current.User as MyIdentity.MyPrincipal).Identity as MyIdentity).User.IdCard;
            return db.PR_GetFriends(idCard).ToList();
        }

    }
}