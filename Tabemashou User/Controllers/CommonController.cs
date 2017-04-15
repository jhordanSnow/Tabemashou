using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tabemashou_User.Models;

namespace Tabemashou_User.Controllers
{
    public class CommonController : Controller
    {
        private TabemashouEntities db = new TabemashouEntities();
        // GET: Common
        public ActionResult ShowRestLogo(int? id)
        {
            var imagedata = db.Restaurant.Find(id).Logo;
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

        public ActionResult ShowPhoto(int? id)
        {
            var imagedata = db.Photo.Find(id).Photo1;
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

        public string DistrictCompleteName(int? id)
        {
            District district = db.District.Find(id);
            return district.Canton.Province.Name + " - " + district.Canton.Name + " - " + district.Name;
        }
    }
}