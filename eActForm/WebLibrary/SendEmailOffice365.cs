using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;

namespace WebLibrary
{
    public class MailOffice365
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

        public MailOffice365(string userName, string passWord, string mailFrom
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
        public static string ExchangeServiceUrl = "https://autodiscover.thaibev.com/EWS/exchange.asmx";
        public static string KeyName { get { return "c1000840"; } }
        public static string Key { get { return "c1000840"; } }

        public void Send()
        {
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP2);

            service.Credentials = new WebCredentials(mailFrom, passWord);
            service.TraceEnabled = false;
            service.EnableScpLookup = false;
            service.ReturnClientRequestId = true;
            service.Url = new Uri(ExchangeServiceUrl);

            EmailMessage sendingMessage = new EmailMessage(service);
            sendingMessage.Subject = subject;
            sendingMessage.Body = body;
            sendingMessage.ToRecipients.Add(mailTo);
            ExtendedPropertyDefinition def = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, KeyName, MapiPropertyType.String);
            sendingMessage.SetExtendedProperty(def, Key);
            sendingMessage.Send();


            /*SmtpClient smtp = new SmtpClient();
            smtp.Host = host;
            smtp.Port = port;
            smtp.EnableSsl = ssl;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(userName, passWord);
            using (var message = new MailMessage(mailFrom, mailTo))
            {
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isHtml;
                message.CC.Add(cc);
                if (attachment != null)
                {
                    foreach (var item in attachment)
                    {
                        message.Attachments.Add(item);
                    }
                }
                smtp.Send(message);
            }*/
        }
    }
}
