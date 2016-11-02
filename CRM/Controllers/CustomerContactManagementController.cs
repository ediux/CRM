﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRM.Models;

namespace CRM.Controllers
{
    public class CustomerContactManagementController : Controller
    {
        //private CRMEntities db = new CRMEntities();
        private I客戶聯絡人Repository db;

        public CustomerContactManagementController()
        {
            db = RepositoryHelper.Get客戶聯絡人Repository();
        }

        // GET: CustomerContactManagement
        public ActionResult Index(int? id,string returnUrl,string returnTitle)
        {
            if (id.HasValue)
            {
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.ReturnTitle = returnTitle;
                var 客戶聯絡人byId = db.Where(w => w.是否已刪除==false &&
                    w.客戶Id==id)
                    .Include(客 => 客.客戶資料)
                    .OrderBy(o => o.客戶Id);

                return View(客戶聯絡人byId.ToList());
            }
            var 客戶聯絡人 = db.Where(w=>w.是否已刪除==false).Include(客 => 客.客戶資料).OrderBy(o=>o.客戶Id);
            return View(客戶聯絡人.ToList());
        }

        // GET: CustomerContactManagement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = db.Get((id != null && id.HasValue) ? id.Value : -1);

            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: CustomerContactManagement/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(db.Where(w => w.是否已刪除 == false).OrderBy(o=>o.客戶資料.客戶名稱), "Id", "客戶名稱");
            return View();
        }

        // POST: CustomerContactManagement/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,是否已刪除")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                db.Add(客戶聯絡人);
                db.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(db.Where(w => w.是否已刪除 == false).OrderBy(o=>o.客戶資料.客戶名稱), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: CustomerContactManagement/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = db.Get(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(db.Where(w => w.是否已刪除 == false).OrderBy(o => o.客戶資料.客戶名稱), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: CustomerContactManagement/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,是否已刪除")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                db.UnitOfWork.Context.Entry(客戶聯絡人).State = EntityState.Modified;
                db.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(db.Where(w => w.是否已刪除 == false).OrderBy(o => o.客戶資料.客戶名稱), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: CustomerContactManagement/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = db.Get(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: CustomerContactManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶聯絡人 客戶聯絡人 = db.Get(id);
           // db.客戶聯絡人.Remove(客戶聯絡人);
            客戶聯絡人.是否已刪除 = true;
            db.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filiter(string searchFor)
        {
            int idsearch = 0;

            if (int.TryParse(searchFor, out idsearch) == false)
                idsearch = 0;

            return View("Index", db.Where(w => (
                w.Id == idsearch ||
                w.Email.Contains(searchFor) ||
                w.手機.Contains(searchFor) ||
                w.姓名.Contains(searchFor) ||
                w.電話.Contains(searchFor) ||
                w.職稱.Contains(searchFor))
                && w.是否已刪除 == false).OrderBy(o => o.客戶Id).ToList());
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
