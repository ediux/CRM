using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Models
{   
	public  partial class 客戶分類對照表Repository : EFRepository<客戶分類對照表>, I客戶分類對照表Repository
	{        
    }

	public  partial interface I客戶分類對照表Repository : IRepositoryBase<客戶分類對照表>
	{

	}
}