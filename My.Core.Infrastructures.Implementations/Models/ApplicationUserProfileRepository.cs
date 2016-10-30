using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{   
	public  partial class ApplicationUserProfileRepository : EFRepository<ApplicationUserProfile>, IApplicationUserProfileRepository
	{


    }

	public  partial interface IApplicationUserProfileRepository : IRepositoryBase<ApplicationUserProfile>
	{

	}
}