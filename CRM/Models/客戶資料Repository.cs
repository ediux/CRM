using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CRM.Models
{
    public partial class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
    {
        public override IQueryable<客戶資料> All()
        {            
            return base.All()
                .Where(w=>w.是否已刪除==false)
                .Include(i=>i.客戶分類對照表)
                .Include(i=>i.客戶聯絡人)
                .Include(i=>i.客戶銀行資訊);
        }

        public override void Add(客戶資料 entity)
        {
            var checkexists = Where(w => w.客戶名稱 == entity.客戶名稱
                   && w.是否已刪除 == true);

            if (checkexists.Any())
            {
                var existdata = checkexists.Single();
                existdata.是否已刪除 = false;
                existdata.Email = entity.Email;
                existdata.地址 = entity.地址;
                existdata.客戶名稱 = entity.客戶名稱;
                existdata.統一編號 = entity.統一編號;
                existdata.傳真 = entity.傳真;
                existdata.電話 = entity.電話;
                UnitOfWork.Context.Entry(existdata);
            }
            else
            {
                base.Add(entity);
            }
        }

        public override void Delete(客戶資料 entity)
        {

            if (entity.客戶聯絡人.Any())
            {
                foreach (var 聯絡人 in entity.客戶聯絡人.Where(w => w.是否已刪除 == false))
                {
                    聯絡人.是否已刪除 = true;
                    UnitOfWork.Context.Entry(聯絡人).State = EntityState.Modified;
                }
            }
            if (entity.客戶銀行資訊.Any())
            {
                foreach (var 客戶銀行 in entity.客戶銀行資訊.Where(w => w.是否已刪除 == false))
                {
                    客戶銀行.是否已刪除 = true;
                    UnitOfWork.Context.Entry(客戶銀行).State = EntityState.Modified;
                }
            }

            entity.是否已刪除 = true;
            UnitOfWork.Context.Entry(entity).State = EntityState.Modified;
        }
    }

    public partial interface I客戶資料Repository : IRepositoryBase<客戶資料>
    {

    }
}