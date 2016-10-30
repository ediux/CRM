using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{   
	public  partial class ApplicationUserRepository : EFRepository<ApplicationUser>, IApplicationUserRepository
	{

	}

	public  partial interface IApplicationUserRepository : IRepositoryBase<ApplicationUser>
	{

	}
}