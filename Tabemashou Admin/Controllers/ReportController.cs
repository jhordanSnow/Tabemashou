using System;
using System.Activities.Expressions;
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
            model.Restaurants = new SelectList(db.PR_RestaurantInfo(identity.User.IdCard).ToList(), "IdRestaurant", "Name");
            model.DateStart = DateTime.Now.AddMonths(-1).ToString("yyyy-M-d");
            model.DateEnd = DateTime.Now.ToString("yyyy-M-d");
            model.Result = new List<ReportLocalResult>();

            return View(model);
        }

        [HttpPost]
        public ActionResult Local(ReportLocal model)
        {
            var identity = (System.Web.HttpContext.Current.User as MyIdentity.MyPrincipal).Identity as MyIdentity;
            List<ReportLocalResult> resultArray = new List<ReportLocalResult>();

            foreach (var tmp in db.PR_LocalReport(model.RestaurantId, DateTime.Parse(model.DateStart), DateTime.Parse(model.DateEnd).AddDays(1)))
            {
                ReportLocalResult resultTmp = new ReportLocalResult
                {
                    LocalResult = db.Local.Find(tmp.IdLocal),
                    SalesResult = (int) tmp.Sales
                };
                resultArray.Add(resultTmp);
            }

            var newModel = new ReportLocal
            {
                Restaurants = new SelectList(db.PR_RestaurantInfo(identity.User.IdCard).ToList(), "IdRestaurant", "Name"),
                RestaurantId = model.RestaurantId,
                DateStart = model.DateStart,
                DateEnd = model.DateEnd,
                Result = resultArray
            };


            return View(newModel);
        } 
        
        // GET: Report
        public ActionResult DaySales()
        {
            var identity = (System.Web.HttpContext.Current.User as MyIdentity.MyPrincipal).Identity as MyIdentity;
            ReportLocal model = new ReportLocal();
            model.Restaurants = new SelectList(db.PR_RestaurantInfo(identity.User.IdCard).ToList(), "IdRestaurant", "Name");
            model.DateStart = DateTime.Now.AddMonths(-1).ToString("yyyy-M-d");
            model.DateEnd = DateTime.Now.ToString("yyyy-M-d");
            model.Top = 5;
            model.Result = new List<ReportLocalResult>();

            return View(model);
        }

        [HttpPost]
        public ActionResult DaySales(ReportLocal model)
        {
            var identity = (System.Web.HttpContext.Current.User as MyIdentity.MyPrincipal).Identity as MyIdentity;
            List<ReportLocalResult> resultArray = new List<ReportLocalResult>();

            foreach (var tmp in db.PR_BestSalesDaysReport(model.RestaurantId, DateTime.Parse(model.DateStart), DateTime.Parse(model.DateEnd).AddDays(1), model.Top))
            {
                ReportLocalResult resultTmp = new ReportLocalResult
                {
                    LocalResult = db.Local.Find(tmp.LocalId),
                    DayResult = (DateTime) tmp.Day,
                    TotalResult = (decimal) tmp.Total
                };
                resultArray.Add(resultTmp);
            }

            var newModel = new ReportLocal
            {
                Restaurants = new SelectList(db.PR_RestaurantInfo(identity.User.IdCard).ToList(), "IdRestaurant", "Name"),
                RestaurantId = model.RestaurantId,
                DateStart = model.DateStart,
                DateEnd = model.DateEnd,
                Top = model.Top,
                Result = resultArray
            };


            return View(newModel);
        }
    }
}