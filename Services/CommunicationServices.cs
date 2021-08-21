using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;

namespace Server
{
    public class CommunicationServices : ICommunicationServices
    {
        private readonly MailKit.Net.Smtp.SmtpClient _smtpClient;
        private readonly IConfiguration _configuration;

        public CommunicationServices(IConfiguration configuration)
        {
            _configuration = configuration ?? 
            throw new ArgumentNullException(nameof(configuration));
            //string smtpEmail = _configuration.GetValue<string>("SMTP:Email");
            //string smtpPassword = _configuration.GetValue<string>("SMTP:Password");
            
            try
            {
                // Create SMTP Client to handle Sending Emails
                _smtpClient = new MailKit.Net.Smtp.SmtpClient();
                
                // Try Connect Smtp Client to Smtp Server
                _smtpClient.Connect("smtp.gmail.com",465,true);

                // Authenticate SMTP Client using credentials 
                _smtpClient.Authenticate("ricksanchexinfinity@gmail.com","Expl0it90");
            }
            catch (Exception ex)
            { 
                Console.WriteLine("Communications Service Crashed on Construction Reason :: {0}", ex.Message);
            }
        }

        public async Task SendEmail(Email email)
        {
           try
            {
                // Create Message to be sent to receiver
                var mimeMessage = new MimeMessage();
                // Message Sender
                mimeMessage.Sender = new MailboxAddress(email.FromName, email.From);
                // Message Receipient
                mimeMessage.To.Add(new MailboxAddress(email.ToName, email.To));
                // Mesasge Subject
                mimeMessage.Subject = email.Subject;
                // Message Body Builder 
                BodyBuilder body = new BodyBuilder();
                // Message Text Body 
                body.TextBody = email.Body;
                /// Create message body using Message BodyBuilder
                mimeMessage.Body = body.ToMessageBody();
                // Finally Send Message to SMTP Server...
                await _smtpClient.SendAsync(mimeMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email Reason :: {0}", ex.Message);
            }
        }

        public async Task SendHtmlEmail(Email email)
        {
            try
            {
                // create message to be sent
                var mimeMessage = new MimeMessage();
                // message sender
                mimeMessage.Sender = new MailboxAddress(email.FromName,email.From);
                // message receipient
                mimeMessage.To.Add(new MailboxAddress(email.ToName,email.To));
                // message subject
                mimeMessage.Subject = email.Subject;
                // message body builder
                var bodyBuilder = new BodyBuilder();
                // build html body
                bodyBuilder.HtmlBody = email.Body;
                // add message linked resources
                if(email.LinkedResources != null)
                    foreach(var resource in email.LinkedResources)
                    {
                        var resourceEntity = bodyBuilder.LinkedResources.Add(resource.ResourceID,resource.Data);
                        resourceEntity.ContentId = resource.ResourceID;
                    }
                // set mesasge html body
                mimeMessage.Body = bodyBuilder.ToMessageBody();

                // send email
                await _smtpClient.SendAsync(mimeMessage); 
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed to send html email Reason :: {0}", ex.Message);
            }                
        }
    }
}