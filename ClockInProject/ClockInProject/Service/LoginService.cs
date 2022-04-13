using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using ClockInProject.Models;

namespace ClockInProject.Service
{
    public class LoginService
    {
        private ClockIn_Entity db = new ClockIn_Entity();

        public string GetRandomPassword()
        {
            string[] StrCode = { "A", "B", "C", "1", "2", "3", "a", "b", "c", "d" };
            string StrRandomCode = string.Empty;
            Random rd = new Random();
            for (int i = 0; i < 11; i++)
            {
                StrRandomCode += StrCode[rd.Next(StrCode.Count())];
            }

            return StrRandomCode;
        }

        public string HashPassword(string password)
        {
            string Strsaltkey = "sEweropewrkofdl;m32orpe";
            string StrsaltAndPassword = string.Concat(password, Strsaltkey);
            SHA1CryptoServiceProvider sha1Hasher = new SHA1CryptoServiceProvider();
            byte[] BytPasswordData = Encoding.Default.GetBytes(StrsaltAndPassword);
            byte[] BytHashDate = sha1Hasher.ComputeHash(BytPasswordData);
            string StrHashresult = "";
            for (int i = 0; i < BytHashDate.Length; i++)
            {
                StrHashresult += BytHashDate[i].ToString("x2");
            }

            return StrHashresult;
        }

        public string LoginCheck(string UserID, string Password)
        {
             
            SysUserTable sysUserTable = db.SysUserTable.Find(UserID);

            if (sysUserTable != null)
            {
                if (string.IsNullOrWhiteSpace(sysUserTable.AuthCode))
                {
                    if (sysUserTable.Type == 1 || PasswordCheck(sysUserTable, Password))
                    {
                        return "";
                    }
                    else
                    {
                        return "密碼輸入錯誤";
                    }
                }
                else
                {
                    return "未經過驗證請去收信";
                }
                 
            }
            else
            {
                return "無此會員帳號";
            }
        }

        private bool PasswordCheck(SysUserTable CheckMember, string Password)
        {
            bool result = CheckMember.Password.Equals(HashPassword(Password));
            return result;
        }

        public string GetRole(string UserID)
        {
            SysUserTable sysUserTable = db.SysUserTable.Find(UserID);
            if (sysUserTable.Role == 1)
            {
                return "admin";
            }
            else
            {
                return "user";
            }
        }

        public string EmailValidate(string UserID, string AuthCode)
        {
            SysUserTable sysUserTable = db.SysUserTable.Find(UserID);
            string StrValidate = "";
            if (sysUserTable != null)
            {
                if (sysUserTable.AuthCode == AuthCode)
                {
                    sysUserTable.AuthCode = string.Empty;
                    db.SaveChanges();
                    StrValidate = "驗證成功，可以開始登入。";
                }
                else
                {
                    StrValidate = "驗證錯誤";
                }
            }
            else
            {
                StrValidate = "傳送錯誤";
            }

            return StrValidate;
        }

        public bool FirstFBLogin(string Email)
        {
            SysUserTable sysUserTable = db.SysUserTable.Where(x => x.Email == Email && x.Type == 1).FirstOrDefault();

            if (sysUserTable != null)
                return false;

            return true;
        }

        public void CreateAccountFB(string UserID, string UserName, string Email)
        {
            SysUserTable sysUserTable = new SysUserTable();
            sysUserTable.UserID = UserID;
            sysUserTable.UserName = UserName;
            sysUserTable.Email = Email;
            sysUserTable.Password = "";
            sysUserTable.IsStopUse = false;
            sysUserTable.Type = 1;
            sysUserTable.Role = 0;
            sysUserTable.AuthCode = "";
            db.SysUserTable.Add(sysUserTable);
            db.SaveChanges();
        }

        public string FunG_MaxUserID_FB(string Email)
        {
            string StrUserID = "";
            SysUserTable sysUserTable;

            sysUserTable = db.SysUserTable.Where(x => x.Type == 1).OrderByDescending(x => x.UserID).FirstOrDefault();

            if (sysUserTable != null)
                StrUserID = (Convert.ToDecimal(sysUserTable.UserID) + 1).ToString().PadLeft(20, '0');
            else
                StrUserID = "1".PadLeft(20, '0');

            return StrUserID;
        } 
    }
}