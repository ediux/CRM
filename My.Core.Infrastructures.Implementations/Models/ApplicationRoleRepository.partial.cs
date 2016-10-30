using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{
    public partial class ApplicationRoleRepository
    {

        public ApplicationRole FindById(int RoleId)
        {
            try
            {
                return Get(RoleId);
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
                var role = (from q in ObjectSet
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

   
    }

    public partial interface IApplicationRoleRepository
    {

        /// <summary>
        /// 以指定ID尋找
        /// </summary>
        /// <returns>The p find by identifier.</returns>
        /// <param name="RoleId">Role identifier.</param>
        ApplicationRole FindById(int RoleId);

        /// <summary>
        /// 以角色名稱尋找
        /// </summary>
        /// <returns>The p find by name.</returns>
        /// <param name="roleName">Role name.</param>
        ApplicationRole FindByName(string roleName);


    }
}
