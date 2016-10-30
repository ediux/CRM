using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{   
	public  partial class ApplicationUserProfileRefRepository : EFRepository<ApplicationUserProfileRef>, IApplicationUserProfileRefRepository
	{

	}

	public  partial interface IApplicationUserProfileRefRepository : IRepositoryBase<ApplicationUserProfileRef>
	{

	}
}