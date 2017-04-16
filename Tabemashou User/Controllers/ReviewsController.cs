using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        // GET: Reviews
        public ActionResult Index()
        {
            var review = db.Review.Include(r => r.Customer).Include(r => r.Local);
            return View(review.ToList());
        }

        // GET: Reviews/Details/5
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
        public ActionResult Create()
        {
            ViewBag.IdCustomer = new SelectList(db.Customer, "IdCard", "IdCard");
            ViewBag.IdLocal = new SelectList(db.Local, "IdLocal", "Latitude");
            return View();
        }

        // POST: Reviews/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdReview,Date,Description,Price,Quality,IdCustomer,IdLocal")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Review.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCustomer = new SelectList(db.Customer, "IdCard", "IdCard", review.IdCustomer);
            ViewBag.IdLocal = new SelectList(db.Local, "IdLocal", "Latitude", review.IdLocal);
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
