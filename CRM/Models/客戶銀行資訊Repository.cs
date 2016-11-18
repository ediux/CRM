using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using NPOI.SS.UserModel;
using System.Web;
using NPOI.XSSF.UserModel;

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

        public Stream Export(string filiter, string[] outputfields)
        {
            var exportdatasource = Filiter(filiter).ToList();

            //IWorkbook wb = new HSSFWorkbook();
            //ISheet ws = wb.CreateSheet("客戶資料");

            //建立Excel 2007檔案
            IWorkbook wb = new XSSFWorkbook();
            ISheet ws = wb.CreateSheet("客戶資料");

            Type currtype = typeof(客戶銀行資訊);

            ws.CreateRow(0);//第一行為欄位名稱
            Dictionary<int, string> fields = new Dictionary<int, string>();

            //掃描欄位名稱
            for (int coli = 0; coli < outputfields.Length; coli++)
            {
                // ws.GetRow(0).CreateCell(coli).SetCellValue(prop.Name);
                if (outputfields[coli] == null)
                    continue;

                fields.Add(coli, outputfields[coli]);
                ws.GetRow(0).CreateCell(coli).SetCellValue(outputfields[coli]);

            }

            for (int rowi = 0; rowi < exportdatasource.Count; rowi++)
            {
                IRow row = ws.CreateRow(1 + rowi);
                Type rowtype = exportdatasource[rowi].GetType();

                for (int i = 0; i < fields.Keys.Count; i++)
                {
                    if (fields[i] == "客戶分類")
                    {
                        var propinfoc = rowtype.GetProperty("客戶分類對照表");

                        if (propinfoc == null)
                            continue;

                        object value = propinfoc.GetValue(exportdatasource[rowi]);
                        if (value == null)
                            continue;

                        var navproptype = value.GetType();
                        if (navproptype == null)
                            continue;

                        var navprop = navproptype.GetProperty(fields[i]);
                        if (navprop == null)
                            continue;

                        row.CreateCell(i).SetCellValue((string)navprop.GetValue(value));
                        continue;
                    }

                    var propinfo = rowtype.GetProperty(fields[i]);

                    if (propinfo == null)
                        continue;

                    if (propinfo.PropertyType == typeof(bool))
                    {
                        object value = propinfo.GetValue(exportdatasource[rowi]);
                        if (value == null)
                            continue;

                        row.CreateCell(i).SetCellValue((bool)value);
                        continue;
                    }
                    if (propinfo.PropertyType == typeof(DateTime))
                    {
                        object value = propinfo.GetValue(exportdatasource[rowi]);
                        if (value == null)
                            continue;

                        row.CreateCell(i).SetCellValue((DateTime)value);
                        continue;
                    }
                    if (propinfo.PropertyType == typeof(double) ||
                        propinfo.PropertyType == typeof(float) ||
                        propinfo.PropertyType == typeof(decimal))
                    {
                        object value = propinfo.GetValue(exportdatasource[rowi]);
                        if (value == null)
                            continue;

                        row.CreateCell(i).SetCellValue((double)value);
                        continue;
                    }

                    object value2 = propinfo.GetValue(exportdatasource[rowi]);
                    if (value2 == null)
                        continue;
                    row.CreateCell(i).SetCellValue((string)value2);


                }

            }

            FileStream file = new FileStream(Path.Combine(HttpRuntime.AppDomainAppPath, "CustomData.xlsx"), FileMode.Create);
            wb.Write(file);
            return file;
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

        Stream Export(string filiter, string[] outputfields);
    }
}