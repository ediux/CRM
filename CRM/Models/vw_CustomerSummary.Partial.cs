namespace CRM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(vw_CustomerSummaryMetaData))]
    public partial class vw_CustomerSummary
    {
    }
    
    public partial class vw_CustomerSummaryMetaData
    {
        [Required]
        public int Id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 客戶名稱 { get; set; }
        public Nullable<int> 客戶聯絡人數量 { get; set; }
        public Nullable<int> 客戶銀行帳戶數量 { get; set; }
    }
}
