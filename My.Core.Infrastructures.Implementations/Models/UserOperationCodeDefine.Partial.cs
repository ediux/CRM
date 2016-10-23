namespace My.Core.Infrastructures.Implementations.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(UserOperationCodeDefineMetaData))]
    public partial class UserOperationCodeDefine
    {
    }
    
    public partial class UserOperationCodeDefineMetaData
    {
        [Required]
        [Display(Name = "OpreationCode", ResourceType = typeof(ReslangMUI.MUI))]
        public int OpreationCode { get; set; }
        
        [StringLength(512, ErrorMessage="欄位長度不得大於 512 個字元")]
        [Required]
        [Display(Name = "Description", ResourceType = typeof(ReslangMUI.MUI))]
        public string Description { get; set; }
        
        [StringLength(256, ErrorMessage="欄位長度不得大於 256 個字元")]
        [Display(Name = "MessageResourceKey", ResourceType = typeof(ReslangMUI.MUI))]
        public string MessageResourceKey { get; set; }
    
        public virtual ICollection<UserOperationLog> UserOperationLog { get; set; }
    }
}
