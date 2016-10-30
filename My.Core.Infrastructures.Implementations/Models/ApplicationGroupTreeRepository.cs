using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{   
	public  partial class ApplicationGroupTreeRepository : EFRepository<ApplicationGroupTree>, IApplicationGroupTreeRepository
	{

	}

	public  partial interface IApplicationGroupTreeRepository : IRepositoryBase<ApplicationGroupTree>
	{

	}
}