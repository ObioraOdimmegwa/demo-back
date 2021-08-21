using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace Server
{
    public static class CommunicationHelpers
    {
        // public static async Task SendEmailVerification(this ICommunicationServices comms, ApplicationUser user, string link,string baseUrl)
        // {
        //     /// Get mail verification template
        //     // TODO :: REPLACE THIS ACCESS TO FILE SYSTEM STORING HTML TEMPLATES
        //     string templatePath =  "./wwwroot/emails/account_verification.html";
        //     // string logoPath = "./wwwroot/images/earn-edge-logo.png";
        //     // string logoWithText = "./wwwroot/images/earn-edge-text.png";

        //     string HtmlFormat = string.Empty;
        //     List<HtmlResource> resources = new List<HtmlResource>();
            
        //     // read html template to string 
        //     using(FileStream fs = new FileStream(templatePath, FileMode.Open))
        //     {
        //         using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
        //         {
        //             HtmlFormat = sr.ReadToEnd();
        //         }
        //     }

        //     // // Add picture to embedded resources and replace links to pictures in the message
        //     // var logoResource = new HtmlResource {
        //     //     ResourceID = Guid.NewGuid().ToString(),
        //     //     Data = System.IO.File.ReadAllBytes(logoPath),
        //     // };
        //     // HtmlFormat = HtmlFormat.Replace("$logo", string.Format("cid:{0}", logoResource.ResourceID));
        //     // resources.Add(logoResource);

        //     // // Add picture to embedded resources and replace links to pictures in the message
        //     // var logoWithTextResource = new HtmlResource {
        //     //     ResourceID = Guid.NewGuid().ToString(),
        //     //     Data = System.IO.File.ReadAllBytes(logoWithText),
        //     // };
        //     // HtmlFormat = HtmlFormat.Replace("$textlogo", string.Format("cid:{0}", logoWithTextResource.ResourceID));
        //     // resources.Add(logoWithTextResource);

        //      // add user's name, link and expiration time to html template
        //      HtmlFormat = HtmlFormat.Replace("$link",link)
        //          .Replace("$baseUrl", baseUrl);

        //     // add user firstname, code and expiration time to html template
        //     HtmlFormat = HtmlFormat.Replace("$baseUrl",baseUrl);
        //     // email to send 
        //     var emailModel = new Email{
        //         To = user.Email,
        //         ToName = $"{user.FirstName} {user.LastName}",
        //         From = "noreply@EarnEdge.com",
        //         FromName = "EarnEdge",
        //         IsHtml = true,
        //         Subject = "Account Verification",
        //         Body = HtmlFormat,
        //         LinkedResources = resources
        //     };

        //     // send email
        //     await comms.SendHtmlEmail(emailModel);
        //     Log.Information($"OTP email sent to {user.Email}");
        // }

        public static async Task SendPasswordReset(this ICommunicationServices comms, User user, string link, string baseUrl)
        {
            // Get mail verification template
            // TODO :: REPLACE THIS ACCESS TO FILE SYSTEM STORING HTML TEMPLATES
            string templatePath =  "./wwwroot/emails/reset.html";

            string HtmlFormat = string.Empty;
            List<HtmlResource> resources = new List<HtmlResource>();
            // read html template to string 
            using(FileStream fs = new FileStream(templatePath, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    HtmlFormat = sr.ReadToEnd();
                }
            }
            // add user's name, link and expiration time to html template
            HtmlFormat = HtmlFormat.Replace("$link",link)
                .Replace("$baseUrl", baseUrl)
                .Replace("$name", string.IsNullOrEmpty(user.DisplayName) ? user.Email : user.DisplayName);
            
            // email to send 
            var emailModel = new Email{
                To = user.Email,
                ToName = user.Email,
                From = "noreply@finlexis.com",
                FromName = "Finlexis Wallet",
                IsHtml = true,
                Subject = "Password Reset Instructions",
                Body = HtmlFormat,
                LinkedResources = resources
            };

            // send email
            await comms.SendHtmlEmail(emailModel);
            Console.WriteLine($"Password Reset email sent to {user.Email}");
        }
           public static async Task Send2FactorCode(this ICommunicationServices comms, User user, string code, string baseUrl)
        {
            // Get mail verification template
            // TODO :: REPLACE THIS ACCESS TO FILE SYSTEM STORING HTML TEMPLATES
            string templatePath =  "./wwwroot/emails/2factor.html";

            string HtmlFormat = string.Empty;
            List<HtmlResource> resources = new List<HtmlResource>();
            // read html template to string 
            using(FileStream fs = new FileStream(templatePath, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    HtmlFormat = sr.ReadToEnd();
                }
            }
            // add user's name, link and expiration time to html template
            HtmlFormat = HtmlFormat.Replace("$code",code)
                .Replace("$baseUrl", baseUrl)
                .Replace("$name", string.IsNullOrEmpty(user.DisplayName) ? user.Email : user.DisplayName);
            
            // email to send 
            var emailModel = new Email{
                To = user.Email,
                ToName = user.Email,
                From = "noreply@finlexis.com",
                FromName = "Finlexis Wallet",
                IsHtml = true,
                Subject = "Authentication Code",
                Body = HtmlFormat,
                LinkedResources = resources
            };

            // send email
            await comms.SendHtmlEmail(emailModel);
            Console.WriteLine($"@ factor code sent to {user.Email}");
        }


        public static string CapFirstLetter(this string word)
        {
            word = word.ToLower();
            var fl = word.First().ToString().ToUpper();
            word = word.Remove(0,1);
            return word = fl+word;
        }
        public static string RemoveLastChar(this string url)
        {
            return url.Remove(url.Length - 1,1);
        }
    }
}