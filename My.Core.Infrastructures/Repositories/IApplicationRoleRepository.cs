using System;
using System.Collections.Generic;
using My.Core.Infrastructures.DAL;

namespace My.Core.Infrastructures
{
    public interface IApplicationRoleRepository<IApplicationRole, TUserRole> : IRepositoryBase<IApplicationRole>
        where IApplicationRole : class
        where TUserRole:class
    {

        /// <summary>
        /// 以指定ID尋找
        /// </summary>
        /// <returns>The p find by identifier.</returns>
        /// <param name="RoleId">Role identifier.</param>
        IApplicationRole FindById(int RoleId);

        /// <summary>
        /// 以角色名稱尋找
        /// </summary>
        /// <returns>The p find by name.</returns>
        /// <param name="roleName">Role name.</param>
        IApplicationRole FindByName(string roleName);

        /// <summary>
        /// Finds the by user.
        /// </summary>
        /// <returns>The by user.</returns>
        /// <param name="MemberId">Member identifier.</param>
        IList<IApplicationRole> FindByUser(int MemberId);

        /// <summary>
        /// Adds the user to role.
        /// </summary>
        /// <returns>The user to role.</returns>
        /// <param name="RoleId">Role identifier.</param>
        /// <param name="MemberId">Member identifier.</param>
        void AddUserToRole(int RoleId, int MemberId);

        /// <summary>
        /// Removes the user from role.
        /// </summary>
        /// <returns>The user from role.</returns>
        /// <param name="RoleId">Role identifier.</param>
        /// <param name="MemberId">Member identifier.</param>
        void RemoveUserFromRole(int RoleId, int MemberId);

        /// <summary>
        /// Ises the in role.
        /// </summary>
        /// <returns>The in role.</returns>
        /// <param name="MemberId">Member identifier.</param>
        /// <param name="roleName">Role name.</param>
        bool IsInRole(int MemberId, string roleName);

        TUserRole Find(int userId, IApplicationRole entity);
        TUserRole CreateUserRole(TUserRole entity);
        IEnumerable<TUserRole> BatchCreateUserRole(IEnumerable<TUserRole> entities);
        void RemoveUserRole(TUserRole entity);
        void RemoveUserRoleRange(IEnumerable<TUserRole> entities);
    }
}

