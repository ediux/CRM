using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Models
{
    public partial class vw_CustomerSummaryRepository : EFRepository<vw_CustomerSummary>, Ivw_CustomerSummaryRepository
    {
        public IEnumerable<vw_CustomerSummary> Filiter(string searchFor)
        {
            throw new NotImplementedException();
        }
    }

    public  partial interface Ivw_CustomerSummaryRepository : IRepositoryBase<vw_CustomerSummary>
	{
        IEnumerable<vw_CustomerSummary> Filiter(string searchFor);
    }
}