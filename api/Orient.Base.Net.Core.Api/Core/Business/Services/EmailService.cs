using Orient.Base.Net.Core.Api.Core.Business.Models;
using Orient.Base.Net.Core.Api.Core.Entities;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using Orient.Base.Net.Core.Api.Core.Common.Constants;

namespace Orient.Base.Net.Core.Api.Core.Business.Services
{
	public interface IEmailService
	{
		//Task SendEmail(string subject, string htmlContent, User user);
		Task<Response> SendEmail(EmailQueue emailQueue);
	}

	public class EmailService : IEmailService
	{
		private readonly IOptions<AppSettings> _appSetting;

		public EmailService(IOptions<AppSettings> appSetting)
		{
			_appSetting = appSetting;
		}

		public static async Task<byte[]> DownloadFile(string url)
		{
			using (var client = new HttpClient())
			{
				using (var result = await client.GetAsync(url))
				{
					if (result.IsSuccessStatusCode)
					{
						return await result.Content.ReadAsByteArrayAsync();
					}
				}
			}
			return null;
		}

		public async Task<Response> SendEmail(EmailQueue emailQueue)
		{
			_appSetting.Value.SendGridKey = EmailQueueConstants.SEND_GIRD_KEY;
			var apiKey = _appSetting.Value.SendGridKey;
			var client = new SendGridClient(apiKey);
			var from = new EmailAddress(emailQueue.From, emailQueue.FromName);
			var emailTos = emailQueue.To.Split(EmailQueueConstants.UNIT_EMAIL_SPLIT);
			var emailToNames = emailQueue.ToName.Split(EmailQueueConstants.UNIT_NAME_SPLIT);
			var listEmailTo = new List<EmailAddress>();

			for (int i = 0; i < emailTos.Length; i++)
			{
				listEmailTo.Add(new EmailAddress(emailTos[i], emailToNames[i]));
			}

			var subject = emailQueue.Subject;
			var htmlContent = emailQueue.Body;
			var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, listEmailTo, subject, "", htmlContent);

			//List CC
			if (!"".Equals(emailQueue.CC.Trim()))
			{
				var cc_emailQueues = emailQueue.CC.Split(EmailQueueConstants.UNIT_EMAIL_SPLIT);
				foreach (var cc_emailQueue in cc_emailQueues)
				{
					msg.AddCc(new EmailAddress(cc_emailQueue, ""));
				}
			}

			//List BCC
			if (!"".Equals(emailQueue.BCC.Trim()))
			{
				var bcc_emailQueues = emailQueue.BCC.Split(EmailQueueConstants.UNIT_EMAIL_SPLIT);
				foreach (var bcc_emailQueue in bcc_emailQueues)
				{
					msg.AddBcc(new EmailAddress(bcc_emailQueue, ""));
				}
			}

			//List attachments
			if (!"".Equals(emailQueue.Attachments.Trim()))
			{
				var attachments = emailQueue.Attachments.Split(EmailQueueConstants.UNIT_ATTACHMENT_SPLIT);
				foreach (var attachment in attachments)
				{
					//parser data from string to json
					TemplateAttachment jsonAttachment = JsonConvert.DeserializeObject<TemplateAttachment>(attachment);
					var bytes = await DownloadFile(jsonAttachment.Link);
					var file = Convert.ToBase64String(bytes);
					msg.AddAttachment(jsonAttachment.FileName, file);
				}
			}

			var response = await client.SendEmailAsync(msg);
			return response;
		}
	}
}
