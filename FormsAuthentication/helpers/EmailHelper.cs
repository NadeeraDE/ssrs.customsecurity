using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Web.Configuration;

namespace Microsoft.Samples.ReportingServices.CustomSecurity
{
    public static class EmailHelper
    {
        public static void SendResetPasswordEmail(string email, string newPassword)
        {
            var mail = new MailMessage();
            mail.To.Add(email);
            mail.From = new MailAddress("monumentBI@monumentconsulting.com");
            mail.Subject = "MONUMENT Report Server - Password Reset Confirmation";
            mail.Body = "We received a request to reset your password, below is the new password. You can change this password after login. <br/>" +
                          newPassword;
            mail.IsBodyHtml = true;
            //Or your Smtp Email ID and Password
            var smtp = new SmtpClient();
            smtp.Host = ConfigHelper.SmtpHost;
            smtp.Port = (ConfigHelper.SmtpPort);
            smtp.Credentials = new System.Net.NetworkCredential
                                                     (ConfigHelper.SmtpUserName, ConfigHelper.SmtpPassword);
            smtp.EnableSsl = ConfigHelper.SmtpEnableSsl;
            smtp.Send(mail);
        }
    }
}
