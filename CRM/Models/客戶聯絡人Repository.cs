using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CRM.Models
{
    public partial class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
    {
        public override IQueryable<客戶聯絡人> All()
        {
            return ObjectSet.Where(w => w.是否已刪除 == false).Include(客 => 客.客戶資料).OrderBy(o => o.客戶Id);
        }

        public override void Delete(客戶聯絡人 entity)
        {
            if (entity.是否已刪除 == false)
            {
                entity.是否已刪除 = true;
                this.Update(entity);
            }
        }

        public IEnumerable<客戶聯絡人> Filiter(string searchFor)
        {
            int idsearch = 0;

            if (int.TryParse(searchFor, out idsearch) == false)
                idsearch = 0;

            return Where(w =>
                w.Id == idsearch ||
                w.Email.Contains(searchFor) ||
                w.手機.Contains(searchFor) ||
                w.姓名.Contains(searchFor) ||
                w.電話.Contains(searchFor))
                .OrderBy(o => o.客戶Id);
        }

        public IEnumerable<客戶聯絡人> FiliterByJobTitleOnly(string searchFor)
        {
            return Where(w => w.職稱.Contains(searchFor))
                .OrderBy(o => o.客戶Id);
        }
    }

    public partial interface I客戶聯絡人Repository : IRepositoryBase<客戶聯絡人>
    {
        IEnumerable<客戶聯絡人> Filiter(string searchFor);
        IEnumerable<客戶聯絡人> FiliterByJobTitleOnly(string searchFor);
    }
}