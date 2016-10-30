using System;

namespace My.Core.Infrastructures.Implementations.Models
{
    public partial class UserOperationLogRepository : IUserOperationLogRepository
    {
        public override UserOperationLog Add(UserOperationLog entity)
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
                entity = base.Add(entity);
                UnitOfWork.Commit();
                return entity;
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
