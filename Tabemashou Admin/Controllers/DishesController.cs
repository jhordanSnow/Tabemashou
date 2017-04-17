using System;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tabemashou_Admin.Models;
using Type = Tabemashou_Admin.Models.Type;

namespace Tabemashou_Admin.Controllers
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

        // GET: Dishes1/Create
        public ActionResult Create(int? id)
        {
            DishRegister model = new DishRegister();
            model.dish = new Dish();
            model.restaurant = db.Restaurant.Find(id);
            db.PR_DeleteUnusedTypes();
            return View(model);
        }


        // POST: Dishes1/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(DishRegister model)
        {
            if (ModelState.IsValid)
            {
                Dish dish = model.dish;
                if (model.restTypesId != null)
                {
                    foreach (var typeId in model.restTypesId)
                    {
                        Type restType = db.Type.Find(typeId);
                        restType.Dish.Add(dish);
                        dish.Type.Add(restType);
                    }
                }
                dish.IdRestaurant = model.idRestaurant;
                db.Dish.Add(dish);

                if (model.uploadFilesNames != null)
                {
                    string[] uploadFiles = model.uploadFilesNames.Split(',');
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase tmpFile = Request.Files[i];
                        if (tmpFile != null && uploadFiles.Any(name => name == tmpFile.FileName) && uploadFiles.Length > 0)
                        {
                            byte[] dbImage = FileUpload(tmpFile);
                            Photo tmpPhoto = new Photo();
                            tmpPhoto.Photo1 = dbImage;
                            db.Photo.Add(tmpPhoto);

                            dish.Photo.Add(tmpPhoto);
                            tmpPhoto.Dish.Add(dish);
                            uploadFiles = uploadFiles.Where(name => name != tmpFile.FileName).ToArray();
                        }
                    }
                }
                var locals = (from loc in db.Local
                    where loc.IdRestaurant == model.idRestaurant
                    select loc);
                foreach (var local in locals)
                {
                    DishesPerLocal dpl = new DishesPerLocal();
                    dpl.IdDish = model.dish.IdDish;
                    dpl.IdLocal = local.IdLocal;
                    dpl.State = true;
                    db.DishesPerLocal.Add(dpl);
                }
                db.SaveChanges();
                return RedirectToAction("Index", "Locals", new { id = dish.IdRestaurant });
            }
            model.restaurant = db.Restaurant.Find(model.idRestaurant);
            return View(model);
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

            DishRegister model = new DishRegister
            {
                dish = dish,
                photos = db.Photo.ToList().Where(m => m.Dish.Any(n => n.IdDish == dish.IdDish)),
                restaurant = db.Restaurant.Find(dish.IdRestaurant),
                deletedFilesIds = "",
                uploadFilesNames = "",
                idRestaurant = dish.IdRestaurant,
                selectedItems = new MultiSelectList(db.Type, "IdType", "Name", dish.Type.Select(t => t.IdType))
        };
            db.PR_DeleteUnusedTypes();
            return View(model);
        }

        // POST: Dishes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(DishRegister model)
        {
            if (!ModelState.IsValid)
            {
                model.selectedItems = new MultiSelectList(db.Type, "IdType", "Name", model.dish.Type.Select(t => t.IdType));
                model.restaurant = db.Restaurant.Find(model.idRestaurant);
                model.photos = db.Photo.ToList().Where(m => m.Dish.Any(n => n.IdDish == model.dish.IdDish));
                model.deletedFilesIds = "";
                model.uploadFilesNames = "";
                return View(model);
            }
            if (model.restTypesId != null && model.restTypesId.Any())
            {
                String typeList = "";
                foreach (var typeId in model.restTypesId)
                {
                    typeList += typeId + ",";
                }
                db.PR_UpdateDishTypes(model.dish.IdDish, typeList.Remove(typeList.Length - 1));
            }
            else
            {
                db.PR_DeleteDishTypes(model.dish.IdDish);
            }

            Dish dish = db.Dish.Find(model.dish.IdDish);
            dish.Name = model.dish.Name;
            dish.Description = model.dish.Description;
            dish.Price = model.dish.Price;
            db.Entry(dish).State = EntityState.Modified;

            if (model.deletedFilesIds != null)
            {
                foreach (var idPhotoDelete in model.deletedFilesIds.Split(','))
                {
                    db.PR_DeleteDishPhoto(dish.IdDish, int.Parse(idPhotoDelete));
                }
            }

            if (model.uploadFilesNames != null)
            {
                string[] uploadFiles = model.uploadFilesNames.Split(',');
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase tmpFile = Request.Files[i];
                    if (tmpFile != null && uploadFiles.Any(name => name == tmpFile.FileName) &&
                        uploadFiles.Length > 0)
                    {
                        byte[] dbImage = FileUpload(tmpFile);
                        Photo tmpPhoto = new Photo();
                        tmpPhoto.Photo1 = dbImage;
                        db.Photo.Add(tmpPhoto);

                        dish.Photo.Add(tmpPhoto);
                        tmpPhoto.Dish.Add(dish);
                        uploadFiles = uploadFiles.Where(name => name != tmpFile.FileName).ToArray();
                    }
                }
            }

            db.SaveChanges();
            TempData["Success"] = dish.Name + " edited successfully.";
            return RedirectToAction("Index", "Locals", new { id = dish.IdRestaurant });
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
        public ActionResult DeleteConfirmed(int id)
        {
            int RestId;
            string dishName;
            using (TabemashouEntities dc = new TabemashouEntities())
            {
                Dish dish = dc.Dish.Find(id);
                dishName = dish.Name;
                RestId = dish.IdRestaurant;
            }
            TempData["Success"] = dishName + " deleted successfully.";
            db.PR_DeleteDish(id);
            db.SaveChanges();
            return RedirectToAction("Index","Locals",new {id = RestId});
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
