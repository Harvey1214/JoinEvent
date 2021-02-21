using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;

namespace JoinEventUI.Data
{
    public static class EmailSender
    {
        public static void Send(int participantId, string email, string content, string eventName)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential("user1325423@gmail.com", "25602560");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();

            mail.IsBodyHtml = true;

            mail.Subject = $"{eventName} - Event Info";
            mail.Body = $"<div class=\"alert alert-danger\" role=\"alert\">Your <b>Participant ID</b> is {participantId}, the event provider may require your participant id, full name and/or the email address provided.</div><br />{content}";

            //Setting From , To and CC
            mail.From = new MailAddress("user1325423@gmail.com");
            mail.To.Add(new MailAddress(email));

            smtpClient.Send(mail);
        }
    }
}
