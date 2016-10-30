using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{   
	public  partial class ApplicationUserClaimRepository : EFRepository<ApplicationUserClaim>, IApplicationUserClaimRepository
	{

	}

	public  partial interface IApplicationUserClaimRepository : IRepositoryBase<ApplicationUserClaim>
	{

	}
}