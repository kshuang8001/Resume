using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ClockInProject.Service
{


    public class MailService
    {
        private string gmail_account = "54yhgfgnbc@gmail.com";
        private string gmail_password = "dszd1234";
        private string gmail_mail = "54yhgfgnbc@gmail.com";

        public string GetValidateCode()
        {
            string[] Code = { "A", "B", "C", "1", "2", "3", "a", "b", "c" };
            string ValidateCode = string.Empty;
            Random rd = new Random();
            for (int i = 0; i < 10; i++)
            {
                ValidateCode += Code[rd.Next(Code.Count())];
            }

            return ValidateCode;
        }

        public string GetRegisterMailBody(string TempString, string UserName, string ValidateUrl)
        {
            TempString = TempString.Replace("{{UserName}}", UserName);
            TempString = TempString.Replace("{{ValidateUrl}}", ValidateUrl);
            return TempString;
        }

        public string GetForgotPasswordMailBody(string TempString, string Password)
        {
            TempString = TempString.Replace("{{Password}}", Password);
            return TempString;
        }

        public void SendMail(string MailBody, string ToEmail, string Subject)
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new NetworkCredential(gmail_account, gmail_password);
            SmtpServer.EnableSsl = true;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(gmail_mail);
            mail.To.Add(ToEmail);
            mail.Subject = Subject;
            mail.Body = MailBody;
            mail.IsBodyHtml = true;
            SmtpServer.Send(mail);
        }
    }
}