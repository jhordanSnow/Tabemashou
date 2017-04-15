﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tabemashou_User.Models;

namespace Tabemashou_User.Controllers
{
    public class ChecksController : Controller
    {
        private TabemashouEntities db = new TabemashouEntities();
        private readonly CommonController _common = new CommonController();

        public ActionResult Invalid()
        {
            return View();
        }

        // GET: Checks
        public ActionResult Index()
        {
            return View(db.Check.ToList());
        }

        // GET: Checks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Check check = db.Check.Find(id);
            if (check == null)
            {
                return HttpNotFound();
            }
            return View(check);
        }

        // GET: Checks/Create
        public ActionResult Create()
        {
            return RedirectToAction("Invalid", "Checks");
        }

        // POST: Checks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Code")] userCheck model)
        {

            Table requestTable = db.Table.Find(model.Code);
            if (requestTable != null)
            {
                model.CheckTable = requestTable;
                model.CheckLocal = requestTable.Local;
                model.CheckRestaurant = requestTable.Local.Restaurant;
                model.DistrictCompeteName = _common.DistrictCompleteName(requestTable.Local.IdDistrict);
                model.UserDishes = "";
                return View(model);
            }
            return RedirectToAction("Invalid","Checks");
        }


        [HttpPost]
        public ActionResult Register(userCheck model)
        {
            if (model.UserDishes != null)
            {
                using (var dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        int IdCheck = Convert.ToInt32(db.PR_NewCheck(model.CheckLocal.IdLocal).ToArray()[0]);
                        var lista = model.UserDishes.Split(',').ToList();
                        foreach (var dishTmp in lista)
                        {
                            if (dishTmp != "")
                            {
                                db.PR_CreateDetailCheck(Convert.ToInt32(dishTmp), IdCheck);
                            }
                        }
                        var identity = (System.Web.HttpContext.Current.User as MyIdentity.MyPrincipal).Identity as MyIdentity;
                        db.PR_CreatePaymentCheck(identity.User.IdCard, IdCheck);
                        db.SaveChanges();
                        dbTran.Commit();

                        return RedirectToAction("Index", "Checks");
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                    }
                }
            }
            return RedirectToAction("Create", "Checks", model);
        }

        // GET: Checks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Check check = db.Check.Find(id);
            if (check == null)
            {
                return HttpNotFound();
            }
            return View(check);
        }

        // POST: Checks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCheck,TotalPrice")] Check check)
        {
            if (ModelState.IsValid)
            {
                db.Entry(check).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(check);
        }

        // GET: Checks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Check check = db.Check.Find(id);
            if (check == null)
            {
                return HttpNotFound();
            }
            return View(check);
        }

        // POST: Checks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Check check = db.Check.Find(id);
            db.Check.Remove(check);
            db.SaveChanges();
            return RedirectToAction("Index");
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