using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CRM.Models
{   
	public  partial class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        public override IQueryable<客戶聯絡人> All()
        {
            return ObjectSet.Where(w => w.是否已刪除 == false).Include(客 => 客.客戶資料).OrderBy(o => o.客戶Id);
        }

        
    }

	public  partial interface I客戶聯絡人Repository : IRepositoryBase<客戶聯絡人>
	{

	}
}