using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{   
	public  partial class ApplicationUserGroupRepository : EFRepository<ApplicationUserGroup>, IApplicationUserGroupRepository
	{

	}

	public  partial interface IApplicationUserGroupRepository : IRepositoryBase<ApplicationUserGroup>
	{

	}
}