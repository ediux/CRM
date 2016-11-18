namespace CRM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    [MetadataType(typeof(客戶聯絡人MetaData))]
    public partial class 客戶聯絡人 : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> vaError = new List<ValidationResult>();
            try
            {
                var db = new CRMEntities();
                
                var result = from q in db.客戶資料
                             from c in q.客戶聯絡人
                             where q.Id == this.客戶Id
                             && c.Email == this.Email
                             && c.是否已刪除 == false
                             select c;

                if (result.Any())
                {
                    vaError.Add(new ValidationResult("此聯絡人eMail已存在!"));
                }
                else
                {
                    vaError.Add(ValidationResult.Success);
                }
                return vaError;
            }
            catch (Exception ex)
            {
                vaError.Add(new ValidationResult(ex.Message));
                return vaError;
            }
        }
    }

    public partial class 客戶聯絡人MetaData
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name= "客戶名稱")]
        public int 客戶Id { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        [Required]
        [Display(Name = "職稱")]
        public string 職稱 { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        [Required]
        [Display(Name = "姓名")]
        public string 姓名 { get; set; }

        [StringLength(250, ErrorMessage = "欄位長度不得大於 250 個字元")]
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        [RegularExpression("\\d{4}-\\d{6}")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "手機")]
        public string 手機 { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        [RegularExpression("\\d{2,3}-\\d{7,8}")]
        [Display(Name = "電話")]
        public string 電話 { get; set; }
        [Required]
        [Display(Name = "是否已刪除")]
        public bool 是否已刪除 { get; set; }

        public virtual 客戶資料 客戶資料 { get; set; }
    }
}
