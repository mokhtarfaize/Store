using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NoovinData.Models.Plugin
{
    public class Email
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Smtp">smtp</param>
        /// <param name="From">ایمیل فرستنده</param>
        /// <param name="Password">رمز عبور ایمیل</param>
        /// <param name="To">ایمیل گیرنده</param>
        /// <param name="Subject">موضوع ایمیل</param>
        /// <param name="Body">متن ایمیل</param>
        public  void SendEmail( string Smtp , string From , string Password ,  string To , string Subject , string Body)
        {
            MailMessage MyEmail = new MailMessage();

            MyEmail.From = new MailAddress(From);
            MyEmail.To.Add(To);
            MyEmail.Subject = Subject;
            MyEmail.Body = Body;
            MyEmail.IsBodyHtml = true;
            MyEmail.Priority = MailPriority.High;

            SmtpClient mysmtp = new SmtpClient(Smtp);
            mysmtp.UseDefaultCredentials = false;
            mysmtp.EnableSsl = true;
            //if ssl equale true port 587 else if ssl false port 465
            mysmtp.Port = 587;
            mysmtp.Credentials = new NetworkCredential(From, Password);
            mysmtp.Send(MyEmail);


        }
    }
}