using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tabemashou_User.Models;

namespace Tabemashou_User.Controllers
{
    public class ReviewsController : Controller
    {
        private TabemashouEntities db = new TabemashouEntities();
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Review.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Check check = db.Check.Find(id);
            if (check == null)
            {
                return HttpNotFound();
            }
            Review review = new Review();
            review.IdCheck = (int) id;
            return View(review);
        }
        [HttpPost]
        public ActionResult Create(Review review)
        {
            if (ModelState.IsValid)
            {
                var idCustomer = (((MyIdentity.MyPrincipal)System.Web.HttpContext.Current.User).Identity as MyIdentity).User.IdCard;
                review.IdCustomer = idCustomer;
                review.Date = DateTime.Now;
                db.Review.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index", "Checks");
            }
            return View(review);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
