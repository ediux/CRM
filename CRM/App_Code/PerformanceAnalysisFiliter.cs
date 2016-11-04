using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class PerformanceAnalysisFiliterAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.StartRunActionTime = DateTime.Now;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            DateTime endTime = DateTime.Now;
            filterContext.Controller.ViewBag.EndRunActionTime = endTime;
            if (filterContext.Controller.ViewBag.StartRunActionTime != null)
            {
                TimeSpan durtionTime = endTime - ((DateTime)filterContext.Controller.ViewBag.StartRunActionTime);                
                filterContext.Controller.ViewBag.SpendTimeForAction = durtionTime.TotalSeconds.ToString();
                System.Diagnostics.Debug.WriteLine("執行動作[{0}]共花費{1}秒。",filterContext.ActionDescriptor.ActionName, durtionTime);
                
            }

            filterContext.Controller.ViewBag.ActionName = filterContext.ActionDescriptor.ActionName;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.StartRunActionResultTime = DateTime.Now;
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            DateTime endTime = DateTime.Now;
            filterContext.Controller.ViewBag.EndRunActionResultTime = endTime;
            if (filterContext.Controller.ViewBag.StartRunActionResultTime != null)
            {
                TimeSpan durtionTime = endTime - ((DateTime)filterContext.Controller.ViewBag.StartRunActionResultTime);
                filterContext.Controller.ViewBag.SpendTimeForActionResult = durtionTime.TotalSeconds.ToString();
                System.Diagnostics.Debug.WriteLine("執行動作結果[{0}]共花費{1}秒。", filterContext.Controller.ViewBag.ActionName as string, durtionTime);
            }
        }
    }
}