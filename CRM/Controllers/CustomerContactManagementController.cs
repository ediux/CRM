using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRM.Models;
using PagedList;

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
        public ActionResult Index(int? id, string returnUrl, string returnTitle, int? pageIndex, int? pagesize)
        {
            if (id.HasValue)
            {
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.ReturnTitle = returnTitle;
                var 客戶聯絡人byId = db.All()
                    .Where(w => w.客戶資料.Id == id)
                    .OrderBy(o => o.Id);

                return View(客戶聯絡人byId.ToPagedList(pageIndex ?? 1, pagesize ?? 25));
            }
            var 客戶聯絡人 = db.All().OrderBy(o => o.Id).ToPagedList(pageIndex ?? 1, pagesize ?? 25);
            return View(客戶聯絡人);
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
        [AddCustomDataRelationSelectList]
        public ActionResult Create()
        {

            return View(new 客戶聯絡人());
        }

        // POST: CustomerContactManagement/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AddCustomDataRelationSelectList]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,是否已刪除")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                db.Add(客戶聯絡人);
                db.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶聯絡人);
        }

        // GET: CustomerContactManagement/Edit/5
        [AddCustomDataRelationSelectList]
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

            return View(客戶聯絡人);
        }

        // POST: CustomerContactManagement/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AddCustomDataRelationSelectList]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,是否已刪除")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                db.UnitOfWork.Context.Entry(客戶聯絡人).State = EntityState.Modified;
                db.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

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
            db.Delete(客戶聯絡人);
            db.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filiter(string searchFor, string jobTitle)
        {
            ViewBag.searchFor = searchFor;
            ViewBag.jobTitle = jobTitle;
            var result = db.Filiter(searchFor);
            result = db.FiliterByJobTitleOnly(jobTitle);

            return View("Index", result.ToList());
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
