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

        // GET: Dishes1/Create
        public ActionResult Create(int? id)
        {
            DishRegister model = new DishRegister();
            model.dish = new Dish();
            model.restaurant = db.Restaurant.Find(id);
            return View(model);
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

        // POST: Dishes1/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(DishRegister model)
        {
            if (ModelState.IsValid)
            {
                Dish dish = model.dish;
                dish.IdRestaurant = model.idRestaurant;
                db.Dish.Add(dish);

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

                db.SaveChanges();
                return RedirectToAction("Index", "Locals", new { id = dish.IdRestaurant });
            }

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
                restaurant = db.Restaurant.Find(dish.IdRestaurant)
            };
            return View(model);
        }

        // POST: Dishes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(DishRegister model)
        {
            if (ModelState.IsValid)
            {
                Dish dish = db.Dish.Find(model.dish.IdDish);
                dish.Name = model.dish.Name;
                dish.Description = model.dish.Description;
                db.Entry(dish).State = EntityState.Modified;

                foreach (var idPhotoDelete in model.deletedFilesIds.Split(','))
                {
                    db.PR_DeleteDishPhoto(dish.IdDish, int.Parse(idPhotoDelete));
                }

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

                db.SaveChanges();
                return RedirectToAction("Index", "Locals", new { id = dish.IdRestaurant });
            }
            return View(model);
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
            Dish dish = db.Dish.Find(id);
            db.Dish.Remove(dish);
            db.SaveChanges();
            return RedirectToAction("Index", "Locals", new { id = dish.IdRestaurant });
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
