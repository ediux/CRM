namespace My.Core.Infrastructures.Implementations.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(ApplicationUserProfileRefMetaData))]
    public partial class ApplicationUserProfileRef
    {
    }
    
    public partial class ApplicationUserProfileRefMetaData
    {
        [Required]
        [Display(Name = "UserName", ResourceType = typeof(ReslangMUI.MUI))]
        [UIHint("UserIDMappingDisplay")]
        public int UserId { get; set; }
        [Required]
        [Display(Name = "ProfileId", ResourceType = typeof(ReslangMUI.MUI))]
        public int ProfileId { get; set; }
        [Required]
        [UIHint("VoidDisplay")]
        [Display(Name = "Void", ResourceType = typeof(ReslangMUI.MUI))]
        public bool Void { get; set; }
        [Required]
        [Display(Name = "CreateTime", ResourceType = typeof(ReslangMUI.MUI))]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        [UIHint("UTCLocalTimeDisplay")]
        public System.DateTime CreateTime { get; set; }
        [Required]
        [Display(Name = "LastUpdateTime", ResourceType = typeof(ReslangMUI.MUI))]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        [UIHint("UTCLocalTimeDisplay")]
        public System.DateTime LastUpdateTime { get; set; }
    
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ApplicationUserProfile ApplicationUserProfile { get; set; }
    }
}
