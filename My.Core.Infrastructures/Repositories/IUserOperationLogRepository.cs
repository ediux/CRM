using System;
using My.Core.Infrastructures.DAL;

namespace My.Core.Infrastructures
{
    public interface IUserOperationLogRepository<IUserOperationLog> : IRepositoryBase<IUserOperationLog> where IUserOperationLog :class
	{
	}
}

