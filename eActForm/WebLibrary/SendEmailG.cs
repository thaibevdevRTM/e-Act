using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace WebLibrary
{
    public class MailOffice365G
    {
        private string userName { get; set; }
        private string passWord { get; set; }
        private string mailFrom { get; set; }
        private string host { get; set; }
        private int port { get; set; }
        private bool ssl { get; set; }
        private List<string> attachmentTxtList { get; set; }
        private List<Attachment> attachment { get; set; }

        private string mailTo { get; set; }
        private string subject { get; set; }
        private string body { get; set; }
        private bool isHtml { get; set; }
        private string cc { get; set; }

        public MailOffice365G(string userName, string passWord, string mailFrom
            , string mailTo, string subject, string body, string cc)
        {
            port = 587;
            host = "smtp.office365.com";
            ssl = true;
            isHtml = true;
            this.userName = userName;
            this.passWord = passWord;
            this.mailFrom = mailFrom;
            this.mailTo = mailTo;
            this.subject = subject;
            this.body = body;
            this.cc = cc;

        }

        public void Send()
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = host;
            smtp.Port = port;
            smtp.EnableSsl = ssl;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(mailFrom, passWord);
            using (var message = new MailMessage(mailFrom, mailTo))
            {
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isHtml;
                if (cc != "")
                {
                    message.CC.Add(cc);
                }
                if (attachment != null)
                {
                    foreach (var item in attachment)
                    {
                        message.Attachments.Add(item);
                    }
                }
                smtp.Send(message);
            }
        }
    }
}
