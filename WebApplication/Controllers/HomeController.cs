using System.Web.Mvc;
using WebApplication.Filter;

namespace WebApplication.Controllers
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