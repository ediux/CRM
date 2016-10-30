using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{
    public partial class ApplicationUserRoleRepository
    {
        private IApplicationUserRepository _applicationUserRepository;
        public IApplicationUserRepository ApplicationUserRepository
        {
            get
            {
                if (_applicationUserRepository == null)
                {
                    _applicationUserRepository = RepositoryHelper.GetApplicationUserRepository();
                    _applicationUserRepository.UnitOfWork = UnitOfWork;
                }

                return _applicationUserRepository;
            }

            set
            {
                _applicationUserRepository = value;
            }
        }

        private IApplicationRoleRepository _applicationRoleRepository;
        public IApplicationRoleRepository ApplicationRoleRepository
        {
            get
            {
                if (_applicationRoleRepository == null)
                {
                    _applicationRoleRepository = RepositoryHelper.GetApplicationRoleRepository();
                    _applicationRoleRepository.UnitOfWork = UnitOfWork;
                }

                return _applicationRoleRepository;
            }

            set
            {
                _applicationRoleRepository = value;
            }
        }



        public IEnumerable<ApplicationUserRole> BatchCreateUserRole(IEnumerable<ApplicationUserRole> entities)
        {
            try
            {
                var result = ((DbSet<ApplicationUserRole>)ObjectSet).AddRange(entities);
                return result;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw;
            }
        }

        public bool IsInRole(int MemberId, string roleName)
        {
            try
            {
                var role = ApplicationRoleRepository.FindByName(roleName);
                var chk = Get(MemberId, role.Id);
                return (chk != null);
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        public void RemoveUserRoleRange(IEnumerable<ApplicationUserRole> entities)
        {
            ((DbSet<ApplicationUserRole>)ObjectSet).RemoveRange(entities);
        }

        public IList<ApplicationRole> FindByUser(int MemberId)
        {
            try
            {
                var result = from q in ApplicationUserRepository.Get(MemberId).ApplicationUserRole
                             where q.Void == false
                             select q.ApplicationRole;

                return result.ToList();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }
    }

    public partial interface IApplicationUserRoleRepository
    {
        IApplicationUserRepository ApplicationUserRepository { get; set; }

        IApplicationRoleRepository ApplicationRoleRepository { get; set; }


        IEnumerable<ApplicationUserRole> BatchCreateUserRole(IEnumerable<ApplicationUserRole> entities);

        void RemoveUserRoleRange(IEnumerable<ApplicationUserRole> entities);

        /// <summary>
        /// Ises the in role.
        /// </summary>
        /// <returns>The in role.</returns>
        /// <param name="MemberId">Member identifier.</param>
        /// <param name="roleName">Role name.</param>
        bool IsInRole(int MemberId, string roleName);

        /// <summary>
        /// Finds the by user.
        /// </summary>
        /// <returns>The by user.</returns>
        /// <param name="MemberId">Member identifier.</param>
        IList<ApplicationRole> FindByUser(int MemberId);
    }
}
