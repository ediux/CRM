using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{   
	public  partial class ApplicationGroupRepository : EFRepository<ApplicationGroup>, IApplicationGroupRepository
	{

	}

	public  partial interface IApplicationGroupRepository : IRepositoryBase<ApplicationGroup>
	{

	}
}