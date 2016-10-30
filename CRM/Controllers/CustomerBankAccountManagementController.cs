using System;
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
    public class CustomerBankAccountManagementController : Controller
    {
        private CRMEntities db = new CRMEntities();

        // GET: CustomerBankAccountManagement
        public ActionResult Index(int? id, string returnUrl,string returnTitle)
        {
            if (id.HasValue)
            {
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.ReturnTitle = returnTitle;
                var 客戶銀行資訊byId = db.客戶銀行資訊.Include(客 => 客.客戶資料);
                return View(客戶銀行資訊byId.Where(w => w.是否已刪除 == false && w.客戶Id == id).OrderBy(o => o.Id).ToList());
            }

            var 客戶銀行資訊 = db.客戶銀行資訊.Include(客 => 客.客戶資料);
            return View(客戶銀行資訊.Where(w => w.是否已刪除 == false).OrderBy(o => o.Id).ToList());
        }

        // GET: CustomerBankAccountManagement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // GET: CustomerBankAccountManagement/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(db.客戶資料.Where(w => w.是否已刪除 == false).OrderBy(o => o.客戶名稱), "Id", "客戶名稱");
            return View();
        }

        // POST: CustomerBankAccountManagement/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼,是否已刪除")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                db.客戶銀行資訊.Add(客戶銀行資訊);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(db.客戶資料.Where(w => w.是否已刪除 == false).OrderBy(o => o.客戶名稱), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: CustomerBankAccountManagement/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料.Where(w => w.是否已刪除 == false).OrderBy(o => o.客戶名稱), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // POST: CustomerBankAccountManagement/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼,是否已刪除")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                db.Entry(客戶銀行資訊).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料.Where(w => w.是否已刪除 == false).OrderBy(o => o.客戶名稱), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: CustomerBankAccountManagement/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // POST: CustomerBankAccountManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);
            客戶銀行資訊.是否已刪除 = true;
            //db.客戶銀行資訊.Remove(客戶銀行資訊);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filiter(string searchFor)
        {
            int idsearch = 0;

            if (int.TryParse(searchFor, out idsearch) == false)
                idsearch = 0;

            return View("Index", db.客戶銀行資訊.Where(w => (
                w.Id == idsearch ||
                w.分行代碼 == idsearch ||
                w.帳戶名稱.Contains(searchFor) ||
                w.帳戶號碼.Contains(searchFor) ||
                w.銀行代碼 == idsearch ||
                w.銀行名稱.Contains(searchFor))
                && w.是否已刪除 == false).OrderBy(o => o.Id).ToList());
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
