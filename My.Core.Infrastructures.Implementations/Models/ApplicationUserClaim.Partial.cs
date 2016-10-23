namespace My.Core.Infrastructures.Implementations.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNet.Identity;

    [MetadataType(typeof(ApplicationUserClaimMetaData))]
    public partial class ApplicationUserClaim
    {
    }

    public partial class ApplicationUserClaimMetaData
    {
        [Display(Name = "UserName", ResourceType = typeof(ReslangMUI.MUI))]
        [Required]
        [UIHint("UserIDMappingDisplay")]
        public int UserId { get; set; }
        [Display(Name = "Id", ResourceType = typeof(ReslangMUI.MUI))]
        [Required]
        [UIHint("UserIDMappingDisplay")]
        public int Id { get; set; }
        [Display(Name = "ClaimType", ResourceType = typeof(ReslangMUI.MUI))]
        [StringLength(256, ErrorMessage = "欄位長度不得大於 256 個字元")]
        public string ClaimType { get; set; }

        [Display(Name = "ClaimValue", ResourceType = typeof(ReslangMUI.MUI))]
        public string ClaimValue { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
