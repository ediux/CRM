using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{   
	public  partial class UserOperationLogRepository : EFRepository<UserOperationLog>, IUserOperationLogRepository
	{

	}

	public  partial interface IUserOperationLogRepository : IRepositoryBase<UserOperationLog>
	{

	}
}