namespace My.Core.Infrastructures.Implementations.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(ApplicationUserRoleMetaData))]
    public partial class ApplicationUserRole
    {
    }

    public partial class ApplicationUserRoleMetaData
    {
        [Required]
        [Display(Name = "UserName", ResourceType = typeof(ReslangMUI.MUI))]
        [UIHint("UserIDMappingDisplay")]
        public int UserId { get; set; }
        [Display(Name = "RoleName", ResourceType = typeof(ReslangMUI.MUI))]
        [Required]
        [UIHint("RoleIDMappingDisplay")]
        public int RoleId { get; set; }
        [Required]
        [UIHint("VoidDisplay")]
        [Display(Name = "Void", ResourceType = typeof(ReslangMUI.MUI))]
        public bool Void { get; set; }

        public virtual ApplicationRole ApplicationRole { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
