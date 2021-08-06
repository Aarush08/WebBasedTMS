using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using Nexmo;
using Nexmo.Api;

namespace WebBasedTMS.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification
        public ActionResult Index()
        {
            return View();
        }

        public void SendSMS(string phoneno, string message)
        {
            var client = new Client(creds: new Nexmo.Api.Request.Credentials
            {
                ApiKey = "618e7a5e",
                ApiSecret = "ZfrsNeMCBZlv7ezc"
            });
            var results = client.SMS.Send(request: new SMS.SMSRequest
            {
                from = "Nexmo",
                to = phoneno,
                text = message
            });
        }
        public void SendEmails(string email, string subject, string body)
        {
            string text = string.Format(subject, body);
            string html = body;

            html += HttpUtility.HtmlEncode(body);

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(ConfigurationManager.AppSettings["Email"].ToString());
            msg.To.Add(new MailAddress(email));
            msg.Subject = subject;
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Email"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
        }
    }
}