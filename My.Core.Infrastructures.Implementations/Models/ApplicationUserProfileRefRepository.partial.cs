using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{
    public partial class ApplicationUserProfileRefRepository
    {
        private IApplicationUserProfileRepository _userprofileRepository;
        public IApplicationUserProfileRepository UserProfileRepository { get {
            if (_userprofileRepository == null)
            {
                _userprofileRepository = RepositoryHelper.GetApplicationUserProfileRepository();
                _userprofileRepository.UnitOfWork = UnitOfWork;
            }

            return _userprofileRepository;
        } set { } }
    }

    public partial interface IApplicationUserProfileRefRepository
    {
        IApplicationUserProfileRepository UserProfileRepository { get; set; }
    }
}
