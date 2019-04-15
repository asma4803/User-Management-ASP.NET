using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace BSEF15M039_Assign5.Models
{
    public static class BAL
    {
        public static bool SendEmail(string toEmailAddress, string subject ,string body) {
            try {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                MailAddress to = new MailAddress(toEmailAddress);
                mail.To.Add(to);
                MailAddress from = new MailAddress("asma.eadf15@gmail.com", "UMS");
                mail.From = from;
                mail.Subject = subject;
                mail.Body = body;
                var sc = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new System.Net.NetworkCredential("asma.eadf15@gmail.com", "a1s1m1a1"), EnableSsl = true
                };
                sc.Send(mail);
                return true;

            }
            catch (Exception ex) {
                return false;
            }
        }
    }
}