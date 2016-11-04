using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Web;

namespace CRM.Models
{
    public partial class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
    {
        private I客戶分類對照表Repository dependcyCustomClassRepo;
        private I客戶聯絡人Repository depencyCustomContractRepo;
        private I客戶銀行資訊Repository depencyBankRepo;

        public 客戶資料Repository()
        {
            dependcyCustomClassRepo = RepositoryHelper.Get客戶分類對照表Repository(UnitOfWork);
            depencyCustomContractRepo = RepositoryHelper.Get客戶聯絡人Repository(UnitOfWork);
            depencyBankRepo = RepositoryHelper.Get客戶銀行資訊Repository(UnitOfWork);
        }

        public override IQueryable<客戶資料> All()
        {
            return base.All()
                .Where(w => w.是否已刪除 == false)
                .Include(i => i.客戶分類對照表)
                .Include(i => i.客戶聯絡人)
                .Include(i => i.客戶銀行資訊);
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
                this.Update(existdata);
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
                foreach (var contact in entity.客戶聯絡人)
                {
                    depencyCustomContractRepo.Delete(contact);
                }
            }

            if (entity.客戶銀行資訊.Any())
            {
                foreach (var Bank in entity.客戶銀行資訊.Where(w => w.是否已刪除 == false))
                {
                    depencyBankRepo.Delete(Bank);
                }
            }
            if (entity.是否已刪除 == false)
            {
                entity.是否已刪除 = true;
                this.Update(entity);
            }
        }

        public IEnumerable<客戶資料> Filiter(string searchFor)
        {
            int idsearch = 0;

            if (int.TryParse(searchFor, out idsearch) == false)
                idsearch = 0;

            return All().Where(w =>
                w.客戶分類對照表.客戶分類 == searchFor ||
                w.Id == idsearch ||
                w.Email.Contains(searchFor) ||
                w.客戶名稱.Contains(searchFor) ||
                w.地址.Contains(searchFor) ||
                w.客戶名稱.Contains(searchFor) ||
                w.統一編號.Contains(searchFor) ||
                w.傳真.Contains(searchFor) ||
                w.電話.Contains(searchFor));
        }

        public Stream Export(string filiter)
        {
            var exportdatasource = Filiter(filiter).ToList();

            //IWorkbook wb = new HSSFWorkbook();
            //ISheet ws = wb.CreateSheet("客戶資料");

            //建立Excel 2007檔案
            IWorkbook wb = new XSSFWorkbook();
            ISheet ws = wb.CreateSheet("客戶資料");

            Type currtype = typeof(客戶資料);

            var props = currtype.GetProperties();

            ws.CreateRow(0);//第一行為欄位名稱
            Dictionary<int, string> fields = new Dictionary<int, string>();

            int coli = 0;
            //掃描欄位名稱
            foreach (var prop in props)
            {
                ws.GetRow(0).CreateCell(coli).SetCellValue(prop.Name);
                fields.Add(coli, prop.Name);
                coli++;
            }

            int row = 1;
            foreach(var rows in exportdatasource)
            {
                ws.CreateRow(row);
                for(int i = 0; i < fields.Keys.Count; i++)
                {
                    Type rowtype = rows.GetType();
                    var propinfo = rowtype.GetProperty(fields[i]);
                    if (propinfo != null)
                    {
                        object val = propinfo.GetValue(rows);
                        if (val != null)
                        {
                            ws.GetRow(row).CreateCell(i).SetCellValue(val.ToString());
                        }
                        
                    }                    
                }
                row++;                          
            }

            FileStream file = new FileStream(Path.Combine(HttpRuntime.AppDomainAppPath, "CustomData.xlsx"), FileMode.Create);
            wb.Write(file);
            return file;
        }
    }

    public partial interface I客戶資料Repository : IRepositoryBase<客戶資料>
    {
        IEnumerable<客戶資料> Filiter(string searchFor);

        System.IO.Stream Export( string filiter);
    }
}