using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{
    public partial class UserOperationCodeDefineRepository : IUserOperationCodeDefineRepository
    {
        public UserOperationCodeDefine FindByCode(int code)
        {
            return Get(code);            
        }
    }

    public partial interface IUserOperationCodeDefineRepository : IRepositoryBase<UserOperationCodeDefine>
    {
        UserOperationCodeDefine FindByCode(int code);
    }
}
