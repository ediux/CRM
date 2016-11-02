using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Models
{   
	public  partial class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{

	}

	public  partial interface I客戶資料Repository : IRepositoryBase<客戶資料>
	{

	}
}