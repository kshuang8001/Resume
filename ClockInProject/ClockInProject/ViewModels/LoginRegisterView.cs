using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ClockInProject.Models;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace ClockInProject.ViewModels
{
    public class LoginRegisterView
    {
        public SysUserTable sysUserTable { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        [StringLength(64, ErrorMessage = "密碼長度最大20")]
        public string Password { get; set; }

        [DisplayName("確認密碼")]
        [Required(ErrorMessage = "請再次輸入密碼")]
        [Compare("Password", ErrorMessage = "兩次密碼輸入不一樣")]
        public string PasswordCheck { get; set; }



    }
}