﻿using System.Web;
using System.Web.Mvc;

namespace CRM
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute(), 1);
            filters.Add(new PerformanceAnalysisFiliterAttribute(), 0);
        }
    }
}
