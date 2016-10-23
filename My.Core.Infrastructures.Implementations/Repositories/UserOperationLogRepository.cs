using My.Core.Infrastructures.DAL;
using My.Core.Infrastructures.Implementations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations
{
    public class UserOperationLogRepository : RepositoryBase<UserOperationLog>, IUserOperationLogRepository<UserOperationLog>
    {
        private IUserOperationCodeDefineRepository<UserOperationCodeDefine> logdefinerepo;

        public UserOperationLogRepository(IUnitofWork unitofwork)
            : base(unitofwork)
        {
            logdefinerepo = _unitofwork.GetRepository<UserOperationCodeDefineRepository, UserOperationCodeDefine>() as IUserOperationCodeDefineRepository<UserOperationCodeDefine>;
        }

        public override UserOperationLog Create(UserOperationLog entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                var logdefined = logdefinerepo.Find(entity.OpreationCode);

                if (logdefined == null)
                {
                    logdefined = logdefinerepo.Create(new UserOperationCodeDefine()
                       {
                           Description = "自動產生",
                           MessageResourceKey = "",
                           OpreationCode = entity.OpreationCode
                       });
                    logdefinerepo.SaveChanges();
                }

                var logdata= base.Create(entity);
                SaveChanges();
                return logdata;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
