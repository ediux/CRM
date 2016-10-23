using My.Core.Infrastructures.DAL;
using My.Core.Infrastructures.Implementations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations
{
    public class UserOperationCodeDefineRepository : RepositoryBase<Models.UserOperationCodeDefine>,IUserOperationCodeDefineRepository<Models.UserOperationCodeDefine>
    {
        

        public UserOperationCodeDefineRepository(IUnitofWork unitofwork)
            : base(unitofwork)
        {
           
        }

        public Models.UserOperationCodeDefine FindByCode(int code)
        {
            return Find(code);
        }
    }
}
