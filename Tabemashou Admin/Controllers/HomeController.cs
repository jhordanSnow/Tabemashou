using System.Web.Mvc;
using Tabemashou_Admin.Filter;

namespace Tabemashou_Admin.Controllers
{
    public class HomeController : Controller
    {
        [UserAuth]
        public ActionResult Index()
        {
            return View();
        }
        
    }
}