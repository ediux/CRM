using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{   
	public  partial class ApplicationUserLoginRepository : EFRepository<ApplicationUserLogin>, IApplicationUserLoginRepository
	{

	}

	public  partial interface IApplicationUserLoginRepository : IRepositoryBase<ApplicationUserLogin>
	{

	}
}