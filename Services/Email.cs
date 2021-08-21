using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Server
{
    public class Email
    {
        /// <summary>
        /// Email Sender Address
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Email Sender Name
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// Email Receiver address
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Email receiver name
        /// </summary>
        public string ToName { get; set; }

        /// <summary>
        /// Email Subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Email Body Content
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Set to true if email is to be sent as HTML Code
        /// </summary>
        public bool IsHtml { get; set; }

        /// <summary>
        /// Html alternative view 
        /// </summary>
        /// <value></value>
        public AlternateView AlternateView { get; set; }

        public List<HtmlResource> LinkedResources { get; set; }
    }

    public class HtmlResource
    {
        public string ResourceID { get; set; }
        public Byte[] Data { get; set; }
    }
}