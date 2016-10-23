using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using My.Core.Infrastructures.DAL;
using My.Core.Infrastructures.Logs;
using My.Core.Infrastructures.Implementations.Models;
using System.Data.Entity;

namespace My.Core.Infrastructures.Implementations
{
    public class ApplicationRoleRepository : RepositoryBase<ApplicationRole>, IApplicationRoleRepository<ApplicationRole, ApplicationUserRole>
    {
        private IApplicationUserRepository<ApplicationUser> accountrepo;

        public ApplicationRoleRepository(IUnitofWork unitofwork)
            : base(unitofwork)
        {

            accountrepo = _unitofwork.GetRepository<ApplicationUserRepository, ApplicationUser>();
            userRoleTable = _unitofwork.GetEntity<ApplicationUserRole>() as DbSet<ApplicationUserRole>;
        }

        public override ApplicationRole Update(ApplicationRole entity)
        {
            try
            {
                entity.LastUpdateTime = DateTime.Now.ToUniversalTime();
                entity.LastUpdateUserId = GetCurrentLoginedUserId();
                return base.Update(entity);
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }

        }

        public ApplicationRole FindById(int RoleId)
        {
            try
            {
                return Find(RoleId);
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }

        }

        public ApplicationRole FindByName(string roleName)
        {
            try
            {
                var role = (from q in FindAll()
                            where q.Name.Equals(roleName, StringComparison.InvariantCultureIgnoreCase)
                               && q.Void == false
                            select q).SingleOrDefault();

                return role;

            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        public IList<ApplicationRole> FindByUser(int MemberId)
        {
            try
            {
                var result = from q in accountrepo.Find(MemberId).ApplicationUserRole
                             where q.Void == false
                             select q.ApplicationRole;

                return ToList(result.AsQueryable());
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        public void AddUserToRole(int RoleId, int MemberId)
        {
            try
            {
                var user = accountrepo.Find(MemberId);
                user.ApplicationUserRole.Add(new ApplicationUserRole() { RoleId = RoleId, UserId = MemberId });
                accountrepo.Update(user);
                SaveChanges();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        public void RemoveUserFromRole(int RoleId, int MemberId)
        {
            try
            {
                var user = (from q in accountrepo.Find(MemberId).ApplicationUserRole
                            where q.UserId == MemberId && q.RoleId == RoleId
                            select q).Single();

                user.Void = true;


            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        public bool IsInRole(int MemberId, string roleName)
        {
            try
            {
                var user = accountrepo.Find(MemberId);

                var chk = from q in user.ApplicationUserRole
                          where q.ApplicationRole.Name.Equals(roleName, StringComparison.InvariantCultureIgnoreCase)
                          select q;

                return chk.Any();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        private DbSet<ApplicationUserRole> userRoleTable;

        public ApplicationUserRole CreateUserRole(ApplicationUserRole entity)
        {
            try
            {
                var result = userRoleTable.Add(entity);
                SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }


        public IEnumerable<ApplicationUserRole> BatchCreateUserRole(IEnumerable<ApplicationUserRole> entities)
        {
            try
            {
                var result = userRoleTable.AddRange(entities);
                SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        public void RemoveUserRole(ApplicationUserRole entity)
        {
            try
            {
                var result = userRoleTable.Remove(entity);
                SaveChanges();
               
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        public void RemoveUserRoleRange(IEnumerable<ApplicationUserRole> entities)
        {
            try
            {
                var result = userRoleTable.RemoveRange(entities);
                SaveChanges();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }


        public ApplicationUserRole Find(int userId, ApplicationRole entity)
        {
            try
            {
                var user = accountrepo.Find(userId);

                var chk = from q in user.ApplicationUserRole
                          where q.RoleId == entity.Id && q.Void == false
                          select q;

                return chk.Single();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }
    }
}

