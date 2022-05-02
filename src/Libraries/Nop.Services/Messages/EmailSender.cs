﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Principal;
using Craftsman.Mail;
using Microsoft.AspNetCore.Http;
using MimeKit;
using Nop.Core.Configuration;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Messages;
using Nop.Core.Infrastructure;

namespace Nop.Services.Messages
{
    /// <summary>
    /// Email sender
    /// </summary>
    public class EmailSender : IEmailSender
    {
        #region Fields

        private readonly INopFileProvider _fileProvider;
        private readonly AlbinaConfig _albinaConfig;

        #endregion

        #region Ctor

        public EmailSender(INopFileProvider fileProvider, AlbinaConfig albinaConfig)
        {
            _fileProvider = fileProvider;
            _albinaConfig = albinaConfig;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Create an file attachment for the specific download object from DB
        /// </summary>
        /// <param name="download">Attachment download (another attachment)</param>
        /// <returns>A leaf-node MIME part that contains an attachment.</returns>
        protected MimePart CreateMimeAttachment(Download download)
        {
            if (download is null)
                throw new ArgumentNullException(nameof(download));

            var fileName = !string.IsNullOrWhiteSpace(download.Filename) ? download.Filename : download.Id.ToString();

            return CreateMimeAttachment($"{fileName}{download.Extension}", download.DownloadBinary, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow);
        }

        /// <summary>
        /// Create an file attachment for the specific file path
        /// </summary>
        /// <param name="filePath">Attachment file path</param>
        /// <param name="attachmentFileName">Attachment file name</param>
        /// <returns>A leaf-node MIME part that contains an attachment.</returns>
        protected MimePart CreateMimeAttachment(string filePath, string attachmentFileName = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (string.IsNullOrWhiteSpace(attachmentFileName))
                attachmentFileName = Path.GetFileName(filePath);

            return CreateMimeAttachment(
                    attachmentFileName,
                    _fileProvider.ReadAllBytes(filePath),
                    _fileProvider.GetCreationTime(filePath),
                    _fileProvider.GetLastWriteTime(filePath),
                    _fileProvider.GetLastAccessTime(filePath));
        }

        /// <summary>
        /// Create an file attachment for the binary data
        /// </summary>
        /// <param name="attachmentFileName">Attachment file name</param>
        /// <param name="binaryContent">The array of unsigned bytes from which to create the attachment stream.</param>
        /// <param name="cDate">Creation date and time for the specified file or directory</param>
        /// <param name="mDate">Date and time that the specified file or directory was last written to</param>
        /// <param name="rDate">Date and time that the specified file or directory was last access to.</param>
        /// <returns>A leaf-node MIME part that contains an attachment.</returns>
        protected MimePart CreateMimeAttachment(string attachmentFileName, byte[] binaryContent, DateTime cDate, DateTime mDate, DateTime rDate)
        {
            if (!ContentType.TryParse(MimeTypes.GetMimeType(attachmentFileName), out var mimeContentType))
                mimeContentType = new ContentType("application", "octet-stream");

            return new MimePart(mimeContentType)
            {
                FileName = attachmentFileName,
                Content = new MimeContent(new MemoryStream(binaryContent)),
                ContentDisposition = new ContentDisposition
                {
                    CreationDate = cDate,
                    ModificationDate = mDate,
                    ReadDate = rDate
                }
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="emailAccount">Email account to use</param>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        /// <param name="fromAddress">From address</param>
        /// <param name="fromName">From display name</param>
        /// <param name="toAddress">To address</param>
        /// <param name="toName">To display name</param>
        /// <param name="replyTo">ReplyTo address</param>
        /// <param name="replyToName">ReplyTo display name</param>
        /// <param name="bcc">BCC addresses list</param>
        /// <param name="cc">CC addresses list</param>
        /// <param name="attachmentFilePath">Attachment file path</param>
        /// <param name="attachmentFileName">Attachment file name. If specified, then this file name will be sent to a recipient. Otherwise, "AttachmentFilePath" name will be used.</param>
        /// <param name="attachedDownloadId">Attachment download ID (another attachment)</param>
        /// <param name="headers">Headers</param>
        public virtual void SendEmail(EmailAccount emailAccount, string subject, string body,
            string fromAddress, string fromName, string toAddress, string toName,
            string replyTo = null, string replyToName = null,
            IEnumerable<string> bcc = null, IEnumerable<string> cc = null,
            string attachmentFilePath = null, string attachmentFileName = null,
            int attachedDownloadId = 0, IDictionary<string, string> headers = null)
        {
            var mm = new CraftsmanMailMessage(_albinaConfig.EmailTest)
            {
                From = new MailAddress(fromAddress, fromName)
            };

            mm.To.Add(new MailAddress(toAddress, toName));

            //CC
            if (!string.IsNullOrEmpty(replyTo))
            {
                mm.ReplyToList.Add(new MailAddress(replyTo, replyToName));
            }

            //BCC
            if (bcc != null)
            {
                foreach (var address in bcc.Where(bccValue => !string.IsNullOrWhiteSpace(bccValue)))
                {
                    mm.Bcc.Add(new MailAddress(address.Trim()));
                }
            }

            //CC
            if (cc != null)
            {
                foreach (var address in cc.Where(ccValue => !string.IsNullOrWhiteSpace(ccValue)))
                {
                    mm.CC.Add(new MailAddress(address.Trim()));
                }
            }

            //content
            mm.Subject = subject;

            //headers
            if (headers != null)
                foreach (var header in headers)
                {
                    mm.Headers.Add(header.Key, header.Value);
                }

            mm.Body = body;
            mm.IsBodyHtml = true; //data stored in the db looks like it's all stored as html

            var fileStreams = new List<FileStream>();
            var attachmentFilePaths = new List<string>();
            if (!string.IsNullOrEmpty(attachmentFilePath))
            {
                attachmentFilePaths = attachmentFilePath.Split(',').ToList();
                foreach (var filePath in attachmentFilePaths)
                {
                    if (File.Exists(filePath))
                    {
                        var rdr = new FileStream(filePath, FileMode.Open);
                        fileStreams.Add(rdr);

                        mm.Attachments.Add(new Attachment(rdr, Path.GetFileName(filePath)));
                    }
                }
            }
            //send email
            
            mm.Send(emailAccount.Host);
            
            DisposeStuff(fileStreams, mm, attachmentFilePaths);
        }

        public virtual void SendErrorEmail(Exception exception, EmailAccount emailAccount, HttpRequest contextRequest, IIdentity userIdentity)
        {
            var username = userIdentity?.IsAuthenticated ?? false ? userIdentity.Name : "anonymous";
            var body = $"Albina Public Website\n\nFull Error (including inner exception & stack trace):\n{exception}\r\n\r\nURL Path: {contextRequest?.Path}\r\nURL Parameters: {contextRequest?.QueryString}\r\nUser: {username}";
            var mm = new CraftsmanMailMessage(_albinaConfig.EmailTest)
            {
                From = new MailAddress(_albinaConfig.ErrorFromAddress)
            };
            mm.To.Add(_albinaConfig.ErrorToAddress);
            mm.Subject = _albinaConfig.ErrorSubject;
            mm.Body = body;

            mm.Send(emailAccount.Host);
        }

        
        private static void DisposeStuff(List<FileStream> fileStreams, MailMessage mm, List<string> filesToDelete)
        {
            foreach (var stream in fileStreams)
            {
                stream.Close();
                stream.Dispose();
            }
            foreach (var attachment in mm.Attachments)
            {
                attachment.Dispose();
            }
            foreach (var file in filesToDelete)
            {
                File.Delete(file);
            }
            mm.Dispose();
        }

        #endregion
    }
}