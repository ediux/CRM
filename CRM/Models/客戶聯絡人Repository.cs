using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Models
{   
	public  partial class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{

	}

	public  partial interface I客戶聯絡人Repository : IRepositoryBase<客戶聯絡人>
	{

	}
}