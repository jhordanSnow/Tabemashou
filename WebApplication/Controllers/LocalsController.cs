using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var locals = (from loc in db.Local
                where loc.IdRestaurant == id
                select loc);

            var dishes = (from loc in db.Dish
                where loc.IdRestaurant == id
                select loc);

            LocalsViewModels model = new LocalsViewModels();
            model.restaurant = db.Restaurant.Find(id);
            model.locals = locals;
            model.menu = new DishesViewModels();
            model.menu.dishes = dishes;
            model.menu.restaurant = model.restaurant;
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
            model.idRestaurant = (int) id;
            model.menu = GetMenuLocal(id);
            model.local = new Local();
            return View(model);
        }

        public List<DishLocal> GetMenuLocal(int? idRestaurant)
        {
            var dishes = (from loc in db.Dish
                          where loc.IdRestaurant == idRestaurant
                          select loc);
            List<DishLocal> result = new List<DishLocal>();
            foreach (Dish tmpDish in dishes)
            {
                DishLocal tmpDishesPerLocal = new DishLocal();
                tmpDishesPerLocal.dish = tmpDish;
                tmpDishesPerLocal.idDish = tmpDish.IdDish;
                tmpDishesPerLocal.state = true;
                result.Add(tmpDishesPerLocal);
            }

            return result;
        }

        public byte[] FileUpload(HttpPostedFileBase file)
        {

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

        // POST: Locals/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(localRegister dataModel)
        {
            if (ModelState.IsValid)
            {
                using (var dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        int idLocal = Convert.ToInt32(db.PR_CreateLocal(dataModel.idRestaurant, dataModel.local.IdDistrict,
                            dataModel.local.Detail, dataModel.local.Latitude, dataModel.local.Longitude).ToList()[0]);

                        if (dataModel.menu != null) {
                            foreach (DishLocal localDish in dataModel.menu)
                            {
                                DishesPerLocal tmpDishesPerLocal = new DishesPerLocal();
                                tmpDishesPerLocal.IdLocal = idLocal;
                                tmpDishesPerLocal.IdDish = localDish.idDish;
                                tmpDishesPerLocal.State = localDish.state;
                                db.DishesPerLocal.Add(tmpDishesPerLocal);
                            }
                        }
                        Local tmpLocal = db.Local.Find(idLocal);
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            HttpPostedFileBase tmpFile = Request.Files[i];
                            if (tmpFile != null)
                            {
                                byte[] dbImage = FileUpload(tmpFile);
                                Photo tmpPhoto = new Photo();
                                tmpPhoto.Photo1 = dbImage;
                                db.Photo.Add(tmpPhoto);

                                tmpLocal.Photo.Add(tmpPhoto);
                                tmpPhoto.Local.Add(tmpLocal);
                            }
                        }

                        db.SaveChanges();
                        dbTran.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                        return RedirectToAction("Create", "Locals", new { id = dataModel.idRestaurant });
                    }
                }
                return RedirectToAction("Index", "Locals", new { id = dataModel.idRestaurant });
            }
            return RedirectToAction("Create", "Locals", new { id = dataModel.idRestaurant });
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
        public ActionResult DeleteConfirmed(int id)
        {

            Local local = db.Local.Find(id);
            int restId = local.Restaurant.IdRestaurant;
            db.PR_DeleteLocal(id);
            db.SaveChanges();
            return RedirectToAction("Index","Locals", new {id = restId });
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
