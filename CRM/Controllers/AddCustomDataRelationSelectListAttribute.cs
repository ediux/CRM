using CRM.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class AddCustomDataRelationSelectListAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var db = RepositoryHelper.Get客戶資料Repository();
            filterContext.Controller.ViewBag.客戶Id = new SelectList(db.Where(w => w.是否已刪除 == false).OrderBy(o => o.客戶名稱).ToList(), "Id", "客戶名稱");
        }
    }
}