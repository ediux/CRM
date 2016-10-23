namespace My.Core.Infrastructures.Implementations.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(ApplicationUserLoginMetaData))]
    public partial class ApplicationUserLogin
    {
    }
    
    public partial class ApplicationUserLoginMetaData
    {
        [Required]
        [Display(Name = "UserName", ResourceType = typeof(ReslangMUI.MUI))]
        [UIHint("UserIDMappingDisplay")]
        public int UserId { get; set; }
        
        [StringLength(512, ErrorMessage="欄位長度不得大於 512 個字元")]
        [Required]
        [Display(Name = "LoginProvider", ResourceType = typeof(ReslangMUI.MUI))]
        public string LoginProvider { get; set; }
        
        [StringLength(512, ErrorMessage="欄位長度不得大於 512 個字元")]
        [Required]
        [Display(Name = "ProviderKey", ResourceType = typeof(ReslangMUI.MUI))]
        public string ProviderKey { get; set; }
    
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
