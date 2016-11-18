using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRM.Models.ViewModel
{
    public class CRMIndexViewModel<T> : PagedList.PagedList<T>, PagedList.IPagedList<T>,IEnumerable<T> where T : class 
    {
        public CRMIndexViewModel(IQueryable<T> source,int pageIndex,int pageSize):base(source,pageIndex,pageSize)
        {
            dataSource = source.FirstOrDefault();
        }

        [Display(Name= "Search", ResourceType = typeof(ReslangMUI.Languages))]
        public string searchFor { get; set; }

        [Display(Name ="搜尋欄位")]
        public int FiliterByColumnIndex { get; set; }
        
        public string ReturnUrl { get; set; }

        [Display(Name ="排序方式")]
        public bool OrderByType { get; set; }

        private T dataSource;
        public T FirstRow { get { return dataSource; } }

        public string ReturnTitle { get; set; }
    }
}