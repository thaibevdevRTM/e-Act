using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
namespace WebLibrary
{
    public class GMailer
    {
        public static string GmailUsername { get; set; }
        public static string GmailPassword { get; set; }
        public static string Mail_From { get; set; }
        public static string GmailHost { get; set; }
        public static int GmailPort { get; set; }
        public static bool GmailSSL { get; set; }
        public List<string> AttachmenttxtList { get; set; }
        public List<Attachment> p_Attachment { get; set; }

        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public string CC { get; set; }

        static GMailer()
        {
            GmailHost = "smtp.office365.com";
            GmailPort = 587; // Gmail can use ports 25, 465 & 587; but must be 25 for medium trust environment.
            // GmailSSL = true;    
            //GmailHost = "smtp.office365.com";
            //GmailHost = "10.7.57.20";
            //GmailPort = 25; // Gmail can use ports 25, 465 & 587; but must be 25 for medium trust environment.
            GmailSSL = true;
        }

        public void Send()
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = GmailHost;
            smtp.Port = GmailPort;
            smtp.EnableSsl = GmailSSL;
            // smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new NetworkCredential(Mail_From, GmailPassword);


            using (var message = new MailMessage(Mail_From, ToEmail))
            {
                message.Subject = Subject;
                message.Body = Body;
                message.IsBodyHtml = IsHtml;
                if (CC != null) message.CC.Add(CC);
                if (p_Attachment != null)
                {
                    foreach (var item in p_Attachment)
                    {
                        message.Attachments.Add(item);
                    }
                }

                smtp.Send(message);
            }
        }
    }
}
