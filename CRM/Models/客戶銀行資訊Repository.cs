using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Models
{
    public partial class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
    {
        public override IQueryable<客戶銀行資訊> All()
        {
            return base.All().Where(w => w.是否已刪除 == false);
        }
        public override void Delete(客戶銀行資訊 entity)
        {
            if (entity.是否已刪除 == false)
            {
                entity.是否已刪除 = true;
                this.Update(entity);
            }
        }

        public IEnumerable<客戶銀行資訊> Filiter(string searchFor)
        {
            int idsearch = 0;

            if (int.TryParse(searchFor, out idsearch) == false)
                idsearch = 0;

            return All().Where(w => 
                w.Id == idsearch ||
                w.分行代碼 == idsearch ||
                w.帳戶名稱.Contains(searchFor) ||
                w.帳戶號碼.Contains(searchFor) ||
                w.銀行代碼 == idsearch ||
                w.銀行名稱.Contains(searchFor))
                .OrderBy(o => o.Id);
        }
    }

    public partial interface I客戶銀行資訊Repository : IRepositoryBase<客戶銀行資訊>
    {
        IEnumerable<客戶銀行資訊> Filiter(string searchFor);
    }
}