using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tabemashou_Admin.Models;

namespace Tabemashou_Admin.Controllers
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
        public List<DishLocal> GetMenuLocalEdit(int? idLocal)
        {
            var dishes = (from loc in db.DishesPerLocal
                          where loc.IdLocal == idLocal
                          select loc);
            List<DishLocal> result = new List<DishLocal>();
            foreach (DishesPerLocal tmpDish in dishes)
            {
                DishLocal tmpDishesPerLocal = new DishLocal();
                tmpDishesPerLocal.dish = tmpDish.Dish;
                tmpDishesPerLocal.idDish = tmpDish.IdDish;
                tmpDishesPerLocal.state = tmpDish.State;
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

        // GET: Locals/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.IdDistrict = new SelectList(db.District, "IdDistrict", "Name");
            ViewBag.IdRestaurant = new SelectList(db.Restaurant, "IdRestaurant", "Name");

            localRegister model = new localRegister();
            model.restaurant = db.Restaurant.Find(id);
            model.idRestaurant = (int)id;
            model.menu = GetMenuLocal(id);
            model.local = new Local();
            return View(model);
        }

        // POST: Locals/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(localRegister dataModel)
        {
            if (!ModelState.IsValid || dataModel.local.IdDistrict == 0)
            {
                if (dataModel.local.IdDistrict == 0) TempData["Error"] = "Please select a District.";
                ViewBag.IdDistrict = new SelectList(db.District, "IdDistrict", "Name");
                ViewBag.IdRestaurant = new SelectList(db.Restaurant, "IdRestaurant", "Name");

                localRegister model = new localRegister();
                model.restaurant = db.Restaurant.Find(dataModel.idRestaurant);
                model.idRestaurant = dataModel.idRestaurant;
                model.menu = GetMenuLocal(dataModel.idRestaurant);
                model.uploadFilesNames = String.Empty;
                model.local = new Local();
                return View(model);
            }
            

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

                    for (int i = 1; i <= dataModel.cantMesas; i++)
                    {
                        Table tmpTable = new Table();
                        tmpTable.IdLocal = tmpLocal.IdLocal;
                        tmpTable.DistinctiveName = i.ToString();
                        db.Table.Add(tmpTable);
                    }

                    if (dataModel.uploadFilesNames != null)
                    {
                        string[] uploadFiles = dataModel.uploadFilesNames.Split(',');
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            HttpPostedFileBase tmpFile = Request.Files[i];
                            if (tmpFile != null && uploadFiles.Any(name => name == tmpFile.FileName) && uploadFiles.Length > 0)
                            {
                                byte[] dbImage = FileUpload(tmpFile);
                                Photo tmpPhoto = new Photo();
                                tmpPhoto.Photo1 = dbImage;
                                db.Photo.Add(tmpPhoto);

                                tmpLocal.Photo.Add(tmpPhoto);
                                tmpPhoto.Local.Add(tmpLocal);
                                uploadFiles = uploadFiles.Where(name => name != tmpFile.FileName).ToArray();
                            }
                        }
                    }

                    db.SaveChanges();
                    dbTran.Commit();
                    TempData["Success"] = "Local created successfully";
                    return RedirectToAction("Index", "Locals", new { id = dataModel.idRestaurant });
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    TempData["Error"] = ex.ToString();
                    return RedirectToAction("Create", "Locals", new { id = dataModel.idRestaurant });
                }
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

            ViewBag.IdDistrict = new SelectList(db.District, "IdDistrict", "Name");
            ViewBag.IdRestaurant = new SelectList(db.Restaurant, "IdRestaurant", "Name");

            localRegister model = new localRegister
            {
                restaurant = db.Restaurant.Find(local.IdRestaurant),
                cantMesas = db.Table.Count(cantTable => cantTable.IdLocal == local.IdLocal),
                idRestaurant = local.IdRestaurant,
                menu = GetMenuLocalEdit(id),
                local = local,
                photos = db.Photo.ToList().Where(m=>m.Local.Any(n => n.IdLocal == local.IdLocal)),
                uploadFilesNames =  "",
                deletedFilesIds = ""
            };
            return View(model);
        }

        // POST: Locals/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(localRegister dataModel)
        {
            if (!ModelState.IsValid || dataModel.local.IdDistrict == 0)
            {
                if (dataModel.local.IdDistrict == 0) TempData["Error"] = "Please select a District.";
                ViewBag.IdDistrict = new SelectList(db.District, "IdDistrict", "Name");
                ViewBag.IdRestaurant = new SelectList(db.Restaurant, "IdRestaurant", "Name");
                localRegister model = new localRegister
                {
                    restaurant = db.Restaurant.Find(dataModel.idRestaurant),
                    cantMesas = db.Table.Count(cantTable => cantTable.IdLocal == dataModel.local.IdLocal),
                    idRestaurant = dataModel.idRestaurant,
                    menu = GetMenuLocalEdit(dataModel.idRestaurant),
                    local = dataModel.local,
                    photos = db.Photo.ToList().Where(m => m.Local.Any(n => n.IdLocal == dataModel.local.IdLocal)),
                    uploadFilesNames = "",
                    deletedFilesIds = ""
                };
                return View(model);
            }

            using (var dbTran = db.Database.BeginTransaction())
            {
                try
                {
                    Local local = db.Local.Find(dataModel.local.IdLocal);
                    local.Longitude = dataModel.local.Longitude;
                    local.Latitude = dataModel.local.Latitude;
                    local.IdDistrict = dataModel.local.IdDistrict;
                    local.Detail = dataModel.local.Detail;
                    db.Entry(local).State = EntityState.Modified;

                    if (dataModel.menu != null)
                    {
                        foreach (DishLocal localDish in dataModel.menu)
                        {
                            DishesPerLocal tmpDishesPerLocal = db.DishesPerLocal.Find(local.IdLocal, localDish.idDish);
                            tmpDishesPerLocal.State = localDish.state;
                            db.Entry(tmpDishesPerLocal).State = EntityState.Modified;
                        }
                    }

                    int cantMesas = db.Table.Count(cantTable => cantTable.IdLocal == local.IdLocal);
                    if (dataModel.cantMesas > cantMesas)
                    {
                        for (int i = cantMesas+1; i <= dataModel.cantMesas; i++)
                        {
                            Table tmpTable = new Table();
                            tmpTable.IdLocal = local.IdLocal;
                            tmpTable.DistinctiveName = i.ToString();
                            db.Table.Add(tmpTable);
                        }
                    }
                    else if (dataModel.cantMesas < cantMesas)
                    {
                        var tables = db.Table.Where(cantTable => cantTable.IdLocal == local.IdLocal).ToArray();
                        for (int i = cantMesas-1; i > dataModel.cantMesas; i--)
                        {
                            db.Table.Remove(tables[i]);
                        }
                    }
                        
                    if (dataModel.deletedFilesIds != null) { 
                        foreach (var idPhotoDelete in dataModel.deletedFilesIds.Split(','))
                        {
                            db.PR_DeleteLocalPhoto(local.IdLocal, int.Parse(idPhotoDelete));
                        }
                    }

                    if (dataModel.uploadFilesNames != null) { 
                        string[] uploadFiles = dataModel.uploadFilesNames.Split(',');
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            HttpPostedFileBase tmpFile = Request.Files[i];
                            if (tmpFile != null && uploadFiles.Any(name => name == tmpFile.FileName) && uploadFiles.Length > 0)
                            {
                                byte[] dbImage = FileUpload(tmpFile);
                                Photo tmpPhoto = new Photo();
                                tmpPhoto.Photo1 = dbImage;
                                db.Photo.Add(tmpPhoto);

                                local.Photo.Add(tmpPhoto);
                                tmpPhoto.Local.Add(local);
                                uploadFiles = uploadFiles.Where(name => name != tmpFile.FileName).ToArray();
                            }
                        }
                    }

                    db.SaveChanges();
                    dbTran.Commit();
                    TempData["Success"] = "Local edited successfully.";
                    return RedirectToAction("Index", "Locals", new { id = dataModel.idRestaurant });
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    TempData["Error"] = ex.ToString();
                    return RedirectToAction("Edit", "Locals", new { id = dataModel.local.IdLocal });
                }
            }
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

        public FileContentResult ShowPhotoById(int id)
        {
            Photo restaurant = db.Photo.Find(id);
            var imagedata = restaurant.Photo1;
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

        public FileContentResult ShowLocalPhoto(int id)
        {
            var photos = db.Photo.ToList().Where(m => m.Local.Any(n => n.IdLocal == id));
            if (photos.Any() && photos.FirstOrDefault().Photo1 != null && photos.FirstOrDefault().Photo1.Length > 0) return File(photos.FirstOrDefault().Photo1, "image/jpg");
            string path = Server.MapPath("~/Images/RestaurantLogos/default.png");
            byte[] array;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                array = new byte[fs.Length];
                fs.Read(array, 0, (int)fs.Length);
            }
            return File(array, "image/jpg");
        }
    }
}