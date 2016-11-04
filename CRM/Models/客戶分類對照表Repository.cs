using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Models
{
    public partial class 客戶分類對照表Repository : EFRepository<客戶分類對照表>, I客戶分類對照表Repository
    {
        public override IQueryable<客戶分類對照表> All()
        {
            return base.All().Where(w => w.Void == false);
        }
        public void ChangeClassify(客戶分類對照表 source, 客戶分類對照表 target)
        {
            var sourcecustoms = source.客戶資料.ToList();
            source.客戶資料.Clear();

            foreach (var custom in sourcecustoms)
            {
                target.客戶資料.Add(custom);
            }
        }

        public void ChangeClassifyByCustom(int customId, 客戶分類對照表 source, 客戶分類對照表 target)
        {
            try
            {
                var custom = source.客戶資料.Where(w => w.Id == customId).Single();

                source.客戶資料.Remove(custom);
                target.客戶資料.Add(custom);
            }
            catch
            {
                throw;
            }
        }

        public override void Delete(客戶分類對照表 entity)
        {
            if (entity.Void == false)
            {
                if (entity.客戶資料.Any())
                {
                    throw new InvalidOperationException("還有其他客戶資料使用此分類，在刪除前請先將這些資料改為未分類或其他分類後。再進行刪除作業!");
                }
                entity.Void = true;
                this.Update(entity);
            }
        }

        public IEnumerable<客戶分類對照表> Filiter(string searchFor)
        {
            int idsearch = 0;

            if (int.TryParse(searchFor, out idsearch) == false)
                idsearch = 0;

            return All().Where(w => (
                 w.Id == idsearch ||
                 w.客戶分類.Contains(searchFor))
                 ).OrderBy(o => o.Id);
        }
    }

    public partial interface I客戶分類對照表Repository : IRepositoryBase<客戶分類對照表>
    {
        void ChangeClassify(客戶分類對照表 source, 客戶分類對照表 target);

        void ChangeClassifyByCustom(int customId, 客戶分類對照表 source, 客戶分類對照表 target);

        IEnumerable<客戶分類對照表> Filiter(string searchFor);
    }
}