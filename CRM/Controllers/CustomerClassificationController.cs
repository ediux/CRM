using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRM.Models;

namespace CRM.Controllers
{
    public class CustomerClassificationController : Controller
    {
        private 客戶分類對照表Repository db = RepositoryHelper.Get客戶分類對照表Repository();

        // GET: CustomerClassification
        public async Task<ActionResult> Index()
        {
            return View(await db.All().ToListAsync());
        }

        // GET: CustomerClassification/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶分類對照表 客戶分類對照表 = await db.GetAsync(id);
            if (客戶分類對照表 == null)
            {
                return HttpNotFound();
            }
            return View(客戶分類對照表);
        }

        // GET: CustomerClassification/Create
        public ActionResult Create()
        {
            return View(new 客戶分類對照表() { Id=1 });
        }

        // POST: CustomerClassification/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,客戶分類")] 客戶分類對照表 客戶分類對照表)
        {
            if (ModelState.IsValid)
            {
                db.Add(客戶分類對照表);
                await db.UnitOfWork.CommitAsync();
                return RedirectToAction("Index");
            }

            return View(客戶分類對照表);
        }

        // GET: CustomerClassification/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶分類對照表 客戶分類對照表 = await db.GetAsync(id);
            if (客戶分類對照表 == null)
            {
                return HttpNotFound();
            }
            return View(客戶分類對照表);
        }

        // POST: CustomerClassification/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,客戶分類")] 客戶分類對照表 客戶分類對照表)
        {
            if (ModelState.IsValid)
            {
                db.UnitOfWork.Context.Entry(客戶分類對照表).State = EntityState.Modified;
                await db.UnitOfWork.CommitAsync();
                return RedirectToAction("Index");
            }
            return View(客戶分類對照表);
        }

        // GET: CustomerClassification/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶分類對照表 客戶分類對照表 = await db.GetAsync(id);
            if (客戶分類對照表 == null)
            {
                return HttpNotFound();
            }
            return View(客戶分類對照表);
        }

        // POST: CustomerClassification/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            客戶分類對照表 客戶分類對照表 = await db.GetAsync(id);
            db.Delete(客戶分類對照表);
            await db.UnitOfWork.CommitAsync();
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
