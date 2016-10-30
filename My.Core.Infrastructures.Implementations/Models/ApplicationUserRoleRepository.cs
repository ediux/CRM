using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{
    public partial class ApplicationUserRoleRepository : EFRepository<ApplicationUserRole>, IApplicationUserRoleRepository
    {
        
    }

    public  partial interface IApplicationUserRoleRepository : IRepositoryBase<ApplicationUserRole>
	{

	}
}