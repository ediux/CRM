using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{
    public partial class UserOperationLogRepository : IUserOperationLogRepository
    {
        public override void Add(UserOperationLog entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                var logdefined = UserOperationCodeDefineRepository.Get(entity.OpreationCode);

                if (logdefined == null)
                {
                    UserOperationCodeDefineRepository.Add(new UserOperationCodeDefine()
                    {
                        Description = "自動產生",
                        MessageResourceKey = "",
                        OpreationCode = entity.OpreationCode
                    });
                }
                base.Add(entity);                
                UnitOfWork.Commit();                
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IUserOperationCodeDefineRepository _userOperationCodeDefinedRepository;
        public IUserOperationCodeDefineRepository UserOperationCodeDefineRepository
        {
            get
            {
                if (_userOperationCodeDefinedRepository == null)
                    _userOperationCodeDefinedRepository = RepositoryHelper.GetUserOperationCodeDefineRepository();

                return _userOperationCodeDefinedRepository;
            }

            set
            {
                _userOperationCodeDefinedRepository = value;
            }
        }
    }

    public partial interface IUserOperationLogRepository
    {
        IUserOperationCodeDefineRepository UserOperationCodeDefineRepository { get; set; }
    }
}
