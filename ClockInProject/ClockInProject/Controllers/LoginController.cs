using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClockInProject.Models;
using ClockInProject.Service;
using System.Web.Security;
using ClockInProject.ViewModels;

namespace ClockInProject.Controllers
{
    public class LoginController : Controller
    {
        private LoginService loginService = new LoginService();
        private MailService mailService = new MailService();
        private ClockIn_Entity db = new ClockIn_Entity();

        // GET: Login
        public ActionResult Index()
        {
            
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }


            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginIndexView loginView, int? Remember)
        {
            #region 變數條件
            bool BolFirstFB = false;
            string StrValidate = "", StrRole = "", StrEnticket = "", StrUserID = "";
            DateTime time;
            #endregion

            #region FB登入處理
            if (loginView.Type == 1)
            {
                BolFirstFB = loginService.FirstFBLogin(loginView.Email);
                if (BolFirstFB)
                {
                    StrUserID = loginService.FunG_MaxUserID_FB(loginView.UserID);
                    loginView.UserID = StrUserID;
                    loginService.CreateAccountFB(StrUserID, loginView.UserName, loginView.Email);
                }
                else
                {
                    StrUserID = db.SysUserTable.Where(x => x.Email == loginView.Email && x.Type == 1).FirstOrDefault().UserID;
                    loginView.UserID = StrUserID;
                }
            }
            #endregion

            StrValidate = loginService.LoginCheck(loginView.UserID, loginView.Password);

            if (string.IsNullOrEmpty(StrValidate))
            {
                StrRole = loginService.GetRole(loginView.UserID);
                if (Remember == 1 || loginView.Type == 1)
                    time = DateTime.Now.AddMonths(30);
                else
                    time = DateTime.Now.AddMinutes(30); 
                //使用者資訊加入Cookie
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, loginView.UserID, DateTime.Now, time, false, StrRole, FormsAuthentication.FormsCookiePath);
                StrEnticket = FormsAuthentication.Encrypt(ticket);
                System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, StrEnticket));
                //Cookie有效期設定
                System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Expires = time;


                //導回首頁
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", StrValidate);
                return View();
            }


        }

        [HttpPost]
        public ActionResult Register(LoginRegisterView loginRegisterView)
        {

            if (ModelState.IsValid)
            {
                //密碼加密
                loginRegisterView.sysUserTable.Password = loginService.HashPassword(loginRegisterView.Password);
                //驗證碼處理
                string StrAuthCode = mailService.GetValidateCode();
                loginRegisterView.sysUserTable.AuthCode = StrAuthCode;
                //其他欄位
                loginRegisterView.sysUserTable.Role = 0;
                loginRegisterView.sysUserTable.Type = 0;
                //儲存
                db.SysUserTable.Add(loginRegisterView.sysUserTable);
                db.SaveChanges();

                string StrTempMail = System.IO.File.ReadAllText(Server.MapPath("~/Views/Shared/RegisterEmail.html"));
                UriBuilder ValidateUrl = new UriBuilder(Request.Url) { Path = Url.Action("EmailValidate", "Login", new { UserID = loginRegisterView.sysUserTable.UserID, Username = loginRegisterView.sysUserTable.UserName, AuthCode = StrAuthCode }) };
                string StrMailBody = mailService.GetRegisterMailBody(StrTempMail, loginRegisterView.sysUserTable.UserName, ValidateUrl.ToString().Replace("%3F", "?"));
                mailService.SendMail(StrMailBody, loginRegisterView.sysUserTable.Email, "KS系統| 註冊驗證信");
                TempData["RegisterState"] = "註冊成功，已發送驗證信到Email信箱。";
                return RedirectToAction("RegisterResult");
            }

            loginRegisterView.Password = "";
            loginRegisterView.PasswordCheck = "";
            
            return View(loginRegisterView);


        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string UserID)
        {
            if(ModelState.IsValid)
            {
                SysUserTable sysUserTable = db.SysUserTable.Find(UserID);
                string StrRandomPassword = loginService.GetRandomPassword();
                sysUserTable.Password = loginService.HashPassword(StrRandomPassword);
                db.SaveChanges();
               
                string StrTempMail = System.IO.File.ReadAllText(Server.MapPath("~/Views/Shared/ForgotPasswordEmail.html"));
                //UriBuilder ValidateUrl = new UriBuilder(Request.Url) { Path = Url.Action("EmailValidate", "Login", new { UserID = loginRegisterView.sysUserTable.UserID, Username = loginRegisterView.sysUserTable.UserName, AuthCode = StrAuthCode }) };
                string StrMailBody = mailService.GetForgotPasswordMailBody(StrTempMail, StrRandomPassword);
                mailService.SendMail(StrMailBody, sysUserTable.Email, "KS系統| 新密碼通知");
                TempData["ForgotState"] = "已將密碼發送到註冊Email信箱。";

                return RedirectToAction("ForgotPasswordResult");
            }

            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        public ActionResult ForgotPasswordResult()
        {
            return View();
        }

        public ActionResult RegisterResult()
        {

            return View();
        }

        public ActionResult EmailValidate(string UserID, string AuthCode)
        {
            ViewData["EmailValidate"] = loginService.EmailValidate(UserID, AuthCode);
            return View();
             
        }

        
        public ActionResult Logout()
        {
            //清空Cookie
            int IntCookies = Request.Cookies.Count;

            for (int i = 0; i < IntCookies; i++)
            {
                HttpCookie cookie = Request.Cookies[i];
                cookie.Expires = DateTime.Now.AddDays(-1);
                cookie.Values.Clear();
                cookie.Value = string.Empty;
                Response.Cookies.Set(cookie);
            }
            //清空驗證
            FormsAuthentication.SignOut();
            //傳到前端執行FB登出
            TempData["Logout"] = true;

            return RedirectToAction("Index", "Login");
        }
    }
}

//ViewContext.RouteData.Values["controller"] 當前CONTROLER
//ViewContext.RouteData.Values["action"] 當前ACTION