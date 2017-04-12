 using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
 using System.Diagnostics;
 using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class LocalsController : Controller
    {
        private TabemashouEntities db = new TabemashouEntities();

        // GET: Locals
        public ActionResult Index(int? id)
        {
            var locals = (from loc in db.Local
                where loc.IdRestaurant == id
                select loc);

            var dishes = (from loc in db.Dish
                where loc.IdRestaurant == id
                select loc);

            LocalsViewModels model = new LocalsViewModels();
            model.restaurant = db.Restaurant.Find(id);
            model.locals = locals;
            model.menu = dishes;
            return View(model);
        }

        // GET: Locals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Local local = db.Local.Find(id);
            if (local == null)
            {
                return HttpNotFound();
            }
            return View(local);
        }

        // GET: Locals/Create
        public ActionResult Create(int? id)
        {
            ViewBag.IdDistrict = new SelectList(db.District, "IdDistrict", "Name");
            ViewBag.IdRestaurant = new SelectList(db.Restaurant, "IdRestaurant", "Name");

            localRegister model = new localRegister();
            model.restaurant = db.Restaurant.Find(id);
            model.menu = (from loc in db.Dish
                          where loc.IdRestaurant == id
                          select loc);
            model.local = new Local();
            return View(model);
        }

        // POST: Locals/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdLocal,Latitude,Longitude,IdDistrict,Detail,IdRestaurant")] Local local)
        {
            if (ModelState.IsValid)
            {
                db.Local.Add(local);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdDistrict = new SelectList(db.District, "IdDistrict", "Name", local.IdDistrict);
            ViewBag.IdRestaurant = new SelectList(db.Restaurant, "IdRestaurant", "Name", local.IdRestaurant);
            return View(local);
        }

        // GET: Locals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Local local = db.Local.Find(id);
            if (local == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdDistrict = new SelectList(db.District, "IdDistrict", "Name", local.IdDistrict);
            ViewBag.IdRestaurant = new SelectList(db.Restaurant, "IdRestaurant", "Name", local.IdRestaurant);
            return View(local);
        }

        // POST: Locals/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdLocal,Latitude,Longitude,IdDistrict,Detail,IdRestaurant")] Local local)
        {
            if (ModelState.IsValid)
            {
                db.Entry(local).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdDistrict = new SelectList(db.District, "IdDistrict", "Name", local.IdDistrict);
            ViewBag.IdRestaurant = new SelectList(db.Restaurant, "IdRestaurant", "Name", local.IdRestaurant);
            return View(local);
        }

        // GET: Locals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Local local = db.Local.Find(id);
            if (local == null)
            {
                return HttpNotFound();
            }
            return View(local);
        }

        // POST: Locals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Local local = db.Local.Find(id);
            db.Local.Remove(local);
            db.SaveChanges();
            return RedirectToAction("Index");
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
