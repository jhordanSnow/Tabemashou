using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tabemashou_User.Filter;

namespace Tabemashou_User.Controllers
{
    [UserAuth]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}