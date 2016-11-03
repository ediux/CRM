namespace CRM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(客戶分類對照表MetaData))]
    public partial class 客戶分類對照表
    {
    }
    
    public partial class 客戶分類對照表MetaData
    {
        [Required]
        public int Id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 客戶分類 { get; set; }
    
        public virtual ICollection<客戶資料> 客戶資料 { get; set; }
    }
}
