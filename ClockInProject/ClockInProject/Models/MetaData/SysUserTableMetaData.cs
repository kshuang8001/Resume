using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClockInProject.Models
{
    [MetadataType(typeof(SysUserTableMetaData))]
    public partial class SysUserTable
    {
        public class SysUserTableMetaData
        {
            [DisplayName("帳號")]
            [Required(ErrorMessage = "請輸入帳號")]
            [StringLength(20, MinimumLength = 6, ErrorMessage = "帳號長度需介於6-20")]
            [Remote("UserIDCheck", "Login", ErrorMessage = "此帳號已被註冊過")]
            public string UserID { get; set; }

            [DisplayName("名稱")]
            [Required(ErrorMessage = "請輸入名稱")]
            [StringLength(50, ErrorMessage = "名稱長度最大50")]
            public string UserName { get; set; }

            [DisplayName("密碼")]
            //[Required(ErrorMessage = "請輸入密碼")]
            //[StringLength(20, ErrorMessage = "密碼長度最大20")]
            public string Password { get; set; }

            [DisplayName("Email")]
            [Required(ErrorMessage = "請輸入Email")]
            [StringLength(200, ErrorMessage = "Email長度最多100字元")]
            [EmailAddress(ErrorMessage = "格式錯誤")]
            public string Email { get; set; }

            public bool? IsStopUse { get; set; }
            public short? Type { get; set; }
            public short? Role { get; set; }
        }
    }
}