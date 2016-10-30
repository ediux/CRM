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
    public class CustomerDataManagementController : Controller
    {
        private CRMEntities db = new CRMEntities();

        // GET: CustomerDataManagement
        public ActionResult Index()
        {
            return View(db.客戶資料.Where(w => w.是否已刪除 == false).ToList());
        }

        // GET: CustomerDataManagement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = db.客戶資料.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: CustomerDataManagement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerDataManagement/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,是否已刪除")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                var checkexists = db.客戶資料.Where(w => w.客戶名稱 == 客戶資料.客戶名稱
                    && w.是否已刪除 == true);
                if (checkexists.Any())
                {
                    var existdata = checkexists.Single();
                    existdata.是否已刪除 = false;
                    existdata.Email = 客戶資料.Email;
                    existdata.地址 = 客戶資料.地址;
                    existdata.客戶名稱 = 客戶資料.客戶名稱;
                    existdata.統一編號 = 客戶資料.統一編號;
                    existdata.傳真 = 客戶資料.傳真;
                    existdata.電話 = 客戶資料.電話;
                }
                else
                {
                    db.客戶資料.Add(客戶資料);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: CustomerDataManagement/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = db.客戶資料.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: CustomerDataManagement/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,是否已刪除")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                db.Entry(客戶資料).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: CustomerDataManagement/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = db.客戶資料.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: CustomerDataManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                客戶資料 客戶資料 = db.客戶資料.Find(id);

                if (客戶資料.客戶聯絡人.Any())
                {
                    foreach (var 聯絡人 in 客戶資料.客戶聯絡人.Where(w => w.是否已刪除 == false))
                    {
                        聯絡人.是否已刪除 = true;
                        db.Entry<客戶聯絡人>(聯絡人).State = EntityState.Modified;
                    }
                }
                if (客戶資料.客戶銀行資訊.Any())
                {
                    foreach (var 客戶銀行 in 客戶資料.客戶銀行資訊.Where(w => w.是否已刪除 == false))
                    {
                        客戶銀行.是否已刪除 = true;
                        db.Entry<客戶銀行資訊>(客戶銀行).State = EntityState.Modified;
                    }
                }

                客戶資料.是否已刪除 = true;
                db.Entry<客戶資料>(客戶資料).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult Summary()
        {
            return View(db.vw_CustomerSummary.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filiter(string searchFor)
        {
            int idsearch = 0;

            if (int.TryParse(searchFor, out idsearch) == false)
                idsearch = 0;

            return View("Index", db.客戶資料.Where(w => (
                w.Id == idsearch ||
                w.Email.Contains(searchFor) ||
                w.客戶名稱.Contains(searchFor) ||
                w.地址.Contains(searchFor) ||
                w.客戶名稱.Contains(searchFor) ||
                w.統一編號.Contains(searchFor) ||
                w.傳真.Contains(searchFor) ||
                w.電話.Contains(searchFor))
                && w.是否已刪除 == false).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FiliterVW(string searchFor)
        {
            int idsearch = 0;

            if (int.TryParse(searchFor, out idsearch) == false)
                idsearch = 0;

            return View("Summary", db.vw_CustomerSummary.Where(w => (
                w.Id == idsearch ||
                w.客戶名稱.Contains(searchFor) ||
                w.客戶名稱.Contains(searchFor))).ToList());
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
