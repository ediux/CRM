using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Models
{   
	public  partial class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{
        public override IQueryable<客戶銀行資訊> All()
        {
            return base.All().Where(w=>w.是否已刪除==false);
        }
    }

	public  partial interface I客戶銀行資訊Repository : IRepositoryBase<客戶銀行資訊>
	{

	}
}