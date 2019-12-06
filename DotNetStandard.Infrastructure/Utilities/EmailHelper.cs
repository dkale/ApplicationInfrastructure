using DotNetStandard.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;

namespace DotNetStandard.Infrastructure.Utilities
{
    public class EmailMessage
    {
        public string FromAddress { get; set; }

        public string ToAddress { get; set; }

        public string CcAddress { get; set; }

        public string BccAddress { get; set; }

        public string Subject { get; set; }

        public List<string> AttachmentFileList { get; set; } = new List<string>();

        public string EmailBody { get; set; }

        public bool IsEmailValid()
        {
            var isEmailValid = !string.IsNullOrEmpty(ToAddress) &&
                               !string.IsNullOrEmpty(FromAddress) &&
                               !string.IsNullOrEmpty(Subject) &&
                               !string.IsNullOrEmpty(EmailBody);
            return isEmailValid;
        }

        public override string ToString()
        {
            return $"ToAddress: {ToAddress}, CcAddress: {CcAddress}, Subject: {Subject}";
        }
    }

    public interface IEmailHelper
    {
        List<EmailMessage> EmailQueue { get; }

        List<EmailMessage> AddEmail(EmailMessage emailMessage);

        EmailMessage Compose(string toAddress, string fromAddress, string subject, string emailBody);

        void SendEmails();
    }

    public class EmailHelper : IEmailHelper
    {
        public ILogger Logger { get; set; }

        private readonly int _port;
        private readonly string _host;
        private readonly string _user;
        private readonly string _pass;
        private readonly bool _ssl;
        public List<EmailMessage> EmailQueue { get; } = new List<EmailMessage>();

        public EmailHelper(int port, string host)
        {
            _port = port;
            _host = host;
        }

        public EmailHelper(int port, string mailServerHost, string authUser = null, string authPass = null, bool enableSSL = false)
        {
            _port = port;
            _host = mailServerHost;
            _user = authUser;
            _pass = authPass;
            _ssl = enableSSL;
        }

        public EmailMessage Compose(string toAddress, string fromAddress, string subject, string emailBody)
        {
            var emailMessage = new EmailMessage
            {
                ToAddress = toAddress,
                FromAddress = fromAddress,
                Subject = subject,
                EmailBody = emailBody
            };

            return emailMessage;
        }

        public List<EmailMessage> AddEmail(EmailMessage emailMessage)
        {
            EmailQueue.Add(emailMessage);

            return EmailQueue;
        }

        public void SendEmails()
        {
            if (_port == 0 || string.IsNullOrEmpty(_host))
                throw new Exception("Failed to initialize EmailHelper. Port number or Host not specified");

            if (EmailQueue.Count == 0)
                return;

            var smtpClient = new SmtpClient
            {
                Port = _port,
                Host = _host,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false, // !string.IsNullOrEmpty(_user) && !string.IsNullOrEmpty(_pass) ? new NetworkCredential(_user, _pass) : false;
                                               //Credentials = !string.IsNullOrEmpty(_user) && !string.IsNullOrEmpty(_pass) ? new NetworkCredential(_user, _pass) : null
            };

            foreach (var emailMessage in EmailQueue)
            {
                try
                {
                    if (!emailMessage.IsEmailValid())
                        continue;

                    var mail = new MailMessage(emailMessage.FromAddress, emailMessage.ToAddress, emailMessage.Subject,
                        emailMessage.EmailBody);

                    if (!string.IsNullOrEmpty(emailMessage.CcAddress))
                    {
                        var ccAddressList = emailMessage.CcAddress.Split(',', ';').ToList();
                        ccAddressList.ForEach(ccEmail => { mail.CC.Add(ccEmail); });
                    }

                    if (!string.IsNullOrEmpty(emailMessage.BccAddress))
                    {
                        var bccAddressList = emailMessage.BccAddress.Split(',', ';').ToList();
                        bccAddressList.ForEach(bccEmail => { mail.Bcc.Add(bccEmail); });
                    }

                    if (emailMessage.AttachmentFileList.Count > 0)
                    {
                        emailMessage.AttachmentFileList
                            .Select(GetMailAttachment)
                            .ToList()
                            .ForEach(file => mail.Attachments.Add(file));
                    }

                    mail.IsBodyHtml = true;

                    smtpClient.Send(mail);
                }
                catch (FileNotFoundException ex)
                {
                    Logger.LogError($"Email could not be sent. {ex.GetInnerExceptionMessage()}");
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to send email. Error: {ex.GetInnerExceptionMessage()}");
                }
                finally
                {
                    EmailQueue.Clear();
                }
            }
        }

        private Attachment GetMailAttachment(string attachmentFile)
        {
            if (!File.Exists(attachmentFile))
                throw new FileNotFoundException($"Attachment failed. File: {attachmentFile} no longer exists.");

            var memoryStream = new MemoryStream();
            var fileByteArray = File.ReadAllBytes(attachmentFile);
            memoryStream.Write(fileByteArray, 0, fileByteArray.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
            var attachment = new Attachment(memoryStream, new ContentType("text/csv")) { Name = Path.GetFileName(attachmentFile) };
            return attachment;
        }
    }
}
