using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tabemashou_User.Filter;
using Tabemashou_User.Models;

namespace Tabemashou_User.Controllers
{
    [UserAuth]
    public class HomeController : Controller
    {
        private TabemashouEntities db = new TabemashouEntities();

        public ActionResult Index()
        {
            var identity = (System.Web.HttpContext.Current.User as MyIdentity.MyPrincipal).Identity as MyIdentity;
            Customer customer = db.Customer.Find(identity.User.IdCard);
            List<Review> modelReview = new List<Review>();
            foreach (var account in customer.Customer1)
            {
                modelReview.AddRange(account.Review);
                
            }
            
            return View(modelReview.OrderByDescending(review => review.Date).ToList());
        }

        public string TimeAgo(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("about {0} {1} ago",
                    years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("about {0} {1} ago",
                    months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format("about {0} {1} ago",
                    span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format("about {0} {1} ago",
                    span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format("about {0} {1} ago",
                    span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            if (span.Seconds > 5)
                return String.Format("about {0} seconds ago", span.Seconds);
            if (span.Seconds <= 5)
                return "just now";
            return string.Empty;
        }
    }
}