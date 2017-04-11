﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class RestaurantsController : Controller
    {
        private TabemashouEntities db = new TabemashouEntities();
        // GET: Restaurants
        public ActionResult Index()
        {
            var identity = (System.Web.HttpContext.Current.User as MyIdentity.MyPrincipal).Identity as MyIdentity;
            User loggedUser = db.User.Find(identity.User.IdCard);

            RestaurantsViewModel model = new RestaurantsViewModel();
            model.restaurant = db.PR_RestaurantInfo(identity.User.IdCard).ToList();
            return View(model);
        }

        // GET: Restaurants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurant.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);
        }

        // GET: Restaurants/Create
        public ActionResult Create()
        {
            ViewBag.Type = new SelectList(db.Type, "IdType", "Name");
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(RegisterRestaurantModel model, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                Restaurant restaurant = model.restaurant;
                if(model.restTypesId != null)
                {
                    foreach (var typeId in model.restTypesId)
                    {
                        Models.Type restType = db.Type.Find(typeId);
                        restType.Restaurant.Add(restaurant);
                        restaurant.Type.Add(restType);
                    }
                }
                if (image != null)
                {
                    byte[] dbImage = FileUpload(image);
                    restaurant.Logo = dbImage;

                }
                var identity = (System.Web.HttpContext.Current.User as MyIdentity.MyPrincipal).Identity as MyIdentity;
                restaurant.IdAdmin = identity.User.IdCard;
                db.Restaurant.Add(restaurant);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Type = new SelectList(db.Type, "IdType", "Name");
            return View(model);
        }

        public byte[] FileUpload(HttpPostedFileBase file)
        {
            string pic = System.IO.Path.GetFileName(file.FileName);
            string path = System.IO.Path.Combine(
                                    Server.MapPath("~/Images/RestaurantLogos"), pic);
            // file is uploaded
            file.SaveAs(path);

            // save the image path path to the database or you can send image
            // directly to database
            // in-case if you want to store byte[] ie. for DB
            byte[] array;
            using (MemoryStream ms = new MemoryStream())
            {
                file.InputStream.CopyTo(ms);
                array = ms.GetBuffer();
            }

            return array;

        }

        public FileContentResult Show(int id)
        {
            Restaurant restaurant = db.Restaurant.Find(id);
            var imagedata =  restaurant.Logo;
            if (imagedata != null) return File(imagedata, "image/jpg");
            string path = Server.MapPath("~/Images/RestaurantLogos/default.png");
            byte[] array;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                array = new byte[fs.Length];
                fs.Read(array, 0, (int)fs.Length);
            }
            return File(array, "image/jpg");
        }





        // GET: Restaurants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurant.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            Session["RestId"] = id;
            RegisterRestaurantModel model = new RegisterRestaurantModel { restaurant = restaurant };
            model.selectedItems = new MultiSelectList(db.Type, "IdType", "Name", restaurant.Type.Select(t => t.IdType));
            return View(model);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "IdRestaurant,Name,Logo,IdAdmin")] Restaurant restaurant, HttpPostedFileBase image)
        {
            Restaurant dbRest = db.Restaurant.Find(Session["RestId"]);
            if (ModelState.IsValid)
            {
                if (image != null && image.ContentLength > 0)
                {
                    byte[] dbImage = FileUpload(image);
                    dbRest.Logo = dbImage;
                }
                dbRest.Name = restaurant.Name;
                db.Entry(dbRest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdAdmin = new SelectList(db.Administrator, "IdCard", "IdCard", restaurant.IdAdmin);
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurant.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            db.PR_DeleteRestaurant(id);
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
