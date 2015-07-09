using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;

namespace FullStack.WebAPI.Services
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            using (var email = new MailMessage())
            {
                string applicationEmailAddress = ConfigurationManager.AppSettings["applicationEmail:address"];
                string applicationEmailDisplayName = ConfigurationManager.AppSettings["applicationEmail:displayName"];

                email.To.Add(message.Destination);
                email.From = new MailAddress(applicationEmailAddress, applicationEmailDisplayName);
                email.Subject = message.Subject;
                email.Body = message.Body;
                email.IsBodyHtml = true;
                email.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(message.Body, Encoding.UTF8, MediaTypeNames.Text.Html));

                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(email);
                }
            }
        }

        //// Use NuGet to install SendGrid (Basic C# client lib) 
        //// Don't forget to add 2 new keys named “emailService:Account” 
        //// and “emailService:Password” as AppSettings to store Send Grid credentials.
        //private async Task configSendGridasync(IdentityMessage message)
        //{
        //    var myMessage = new SendGridMessage();

        //    myMessage.AddTo(message.Destination);
        //    myMessage.From = new System.Net.Mail.MailAddress("taiseer@bitoftech.net", "Taiseer Joudeh");
        //    myMessage.Subject = message.Subject;
        //    myMessage.Text = message.Body;
        //    myMessage.Html = message.Body;

        //    var credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailService:Account"],
        //                                            ConfigurationManager.AppSettings["emailService:Password"]);

        //    // Create a Web transport for sending email.
        //    var transportWeb = new Web(credentials);

        //    // Send the email.
        //    if (transportWeb != null)
        //    {
        //        await transportWeb.DeliverAsync(myMessage);
        //    }
        //    else
        //    {
        //        //Trace.TraceError("Failed to create Web transport.");
        //        await Task.FromResult(0);
        //    }
        //}
    }
}