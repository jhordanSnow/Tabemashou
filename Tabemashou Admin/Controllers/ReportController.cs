using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tabemashou_Admin.Models;

namespace Tabemashou_Admin.Controllers
{
    public class ReportController : Controller
    {
        private TabemashouEntities db = new TabemashouEntities();

        // GET: Report
        public ActionResult Local()
        {
            var identity = (System.Web.HttpContext.Current.User as MyIdentity.MyPrincipal).Identity as MyIdentity;
            ReportLocal model = new ReportLocal();
            model.Restaurants = new SelectList(db.PR_RestaurantInfo(identity.User.IdCard).ToList(), "IdType", "Name");

            return View(model);
        }
    }
}