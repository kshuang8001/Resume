using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClockInProject.ViewModels
{
    public class LoginIndexView
    {
        [DisplayName("帳號")]
        [Required(ErrorMessage = "請輸入帳號")]
        public string UserID { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }

        public string Email { get; set; }
        public string UserName { get; set; }
        public short Type { get; set; }
    }
}