using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class DishesController : Controller
    {
        private TabemashouEntities db = new TabemashouEntities();

        // GET: Dishes
        public ActionResult Index()
        {
            var dish = db.Dish.Include(d => d.Restaurant);
            return View(dish.ToList());
        }

        // GET: Dishes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dish = db.Dish.Find(id);
            if (dish == null)
            {
                return HttpNotFound();
            }
            return View(dish);
        }

        // GET: Dishes/Create
        public ActionResult Create()
        {
            ViewBag.IdRestaurant = new SelectList(db.Restaurant, "IdRestaurant", "Name");
            return View();
        }

        // POST: Dishes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdDish,Description,Name,IdRestaurant")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                db.Dish.Add(dish);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdRestaurant = new SelectList(db.Restaurant, "IdRestaurant", "Name", dish.IdRestaurant);
            return View(dish);
        }

        // GET: Dishes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dish = db.Dish.Find(id);
            if (dish == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdRestaurant = new SelectList(db.Restaurant, "IdRestaurant", "Name", dish.IdRestaurant);
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdDish,Description,Name,IdRestaurant")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dish).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdRestaurant = new SelectList(db.Restaurant, "IdRestaurant", "Name", dish.IdRestaurant);
            return View(dish);
        }

        // GET: Dishes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dish = db.Dish.Find(id);
            if (dish == null)
            {
                return HttpNotFound();
            }
            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dish dish = db.Dish.Find(id);
            db.Dish.Remove(dish);
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
