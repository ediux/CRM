using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRM.Models;
using System.IO;

namespace CRM.Controllers
{
    public class CustomerDataManagementController : Controller
    {
        //private CRMEntities db = new CRMEntities();
        private I客戶資料Repository db;
        private I客戶分類對照表Repository db_class;
        private Ivw_CustomerSummaryRepository db_vw;
        public CustomerDataManagementController()
        {
            db = RepositoryHelper.Get客戶資料Repository();
            db_vw = RepositoryHelper.Getvw_CustomerSummaryRepository(db.UnitOfWork);
            db_class = RepositoryHelper.Get客戶分類對照表Repository(db.UnitOfWork);
        }

        // GET: CustomerDataManagement
        public ActionResult Index()
        {
            ViewBag.searchFor = new SelectList(db_class.All().ToList(), "客戶分類", "客戶分類");
            ViewBag.Filiter = "";
            return View(db.Where(w => w.是否已刪除 == false).ToList());
        }

        // GET: CustomerDataManagement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = db.Get(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: CustomerDataManagement/Create        
        public ActionResult Create()
        {
            
            ViewBag.客戶分類ID = new SelectList(db_class.All().ToList(), "Id", "客戶分類");
            return View(new 客戶資料());
        }

        // POST: CustomerDataManagement/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,是否已刪除,客戶分類ID")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                db.Add(客戶資料);
                db.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶分類ID = new SelectList(db_class.All().ToList(), "Id", "客戶分類");
            return View(客戶資料);
        }

        // GET: CustomerDataManagement/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = db.Get(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶分類ID = new SelectList(db_class.All().ToList(), "Id", "客戶分類");
            return View(客戶資料);
        }

        // POST: CustomerDataManagement/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,是否已刪除,客戶分類ID")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                db.UnitOfWork.Context.Entry(客戶資料).State = EntityState.Modified;
                db.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶分類ID = new SelectList(db_class.All().ToList(), "客戶分類ID", "客戶分類");
            return View(客戶資料);
        }

        // GET: CustomerDataManagement/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = db.Get(id);
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
                客戶資料 客戶資料 = db.Get(id);
                db.Delete(客戶資料);
                db.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult Summary()
        {            
            return View(db_vw.All().ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filiter(string searchFor)
        {
            ViewBag.searchFor = new SelectList(db_class.All().ToList(), "客戶分類", "客戶分類");
            ViewBag.Filiter = searchFor;
            return View("Index", db.Filiter(searchFor).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FiliterVW(string searchFor)
        {
            int idsearch = 0;

            if (int.TryParse(searchFor, out idsearch) == false)
                idsearch = 0;

            return View("Summary", db_vw.Where(w => (
                w.Id == idsearch ||
                w.客戶名稱.Contains(searchFor))).ToList());
        }

        public ActionResult Export(string searchFor)
        {

            //filewriter.Write()
            ViewBag.Filiter = searchFor;
            db.Export(searchFor,new string[] { "客戶名稱", "統一編號", "電話", "傳真", "地址", "Email", "客戶分類" });

            return File(Server.MapPath("~/CustomData.xlsx"), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","Export.xlsx");    //File(db.Export(searchFor), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
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
