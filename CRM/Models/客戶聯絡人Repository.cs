using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using System.IO;
using NPOI.XSSF.UserModel;
using System.Web;
using NPOI.SS.UserModel;

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

        public Stream Export(string filiter, string[] outputfields)
        {
            var exportdatasource = Filiter(filiter).ToList();

            //IWorkbook wb = new HSSFWorkbook();
            //ISheet ws = wb.CreateSheet("客戶資料");

            //建立Excel 2007檔案
            IWorkbook wb = new XSSFWorkbook();
            ISheet ws = wb.CreateSheet("客戶資料");

            Type currtype = typeof(客戶資料);

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

        Stream Export(string filiter, string[] outputfields);
    }
}