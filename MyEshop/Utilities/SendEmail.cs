using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace MyEshop
{
    public class SendEmail
    {
        public static void Send(string To,string Subject,string Body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("majid.asadollahi95@gmail.com", "لوازم یدکی سعیدی");
            mail.To.Add(To);
            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("majid.asadollahi95@gmail.com", "epyw nbxe zwie anzs");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
            SmtpServer.Dispose();

        }
    }
}

//System.Net.Mail.Attachment attachment;
// attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
// mail.Attachments.Add(attachment);