using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{
    public partial class UserOperationCodeDefineRepository : EFRepository<UserOperationCodeDefine>, IUserOperationCodeDefineRepository
    {

    }

    public  partial interface IUserOperationCodeDefineRepository : IRepositoryBase<UserOperationCodeDefine>
	{

	}
}