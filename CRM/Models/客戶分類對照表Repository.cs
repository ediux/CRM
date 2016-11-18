using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Web;

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
                    //if (fields[i] == "客戶分類")
                    //{
                    //    var propinfoc = rowtype.GetProperty("客戶分類對照表");

                    //    if (propinfoc == null)
                    //        continue;

                    //    object value = propinfoc.GetValue(exportdatasource[rowi]);
                    //    if (value == null)
                    //        continue;

                    //    var navproptype = value.GetType();
                    //    if (navproptype == null)
                    //        continue;

                    //    var navprop = navproptype.GetProperty(fields[i]);
                    //    if (navprop == null)
                    //        continue;

                    //    row.CreateCell(i).SetCellValue((string)navprop.GetValue(value));
                    //    continue;
                    //}

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

        Stream Export(string filiter, string[] outputfields);
    }
}