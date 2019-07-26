using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.EmailTemplates
{
	public class EmailTemplateResponseModel
	{
		public EmailTemplateResponseModel(EmailTemplate emailTemplate)
		{
			Subject = emailTemplate.Subject;
			From = emailTemplate.From;
			FromName = emailTemplate.FromName;
			CC = emailTemplate.CC;
			BCC = emailTemplate.BCC;
			Body = emailTemplate.Body;
		}

		public string Subject { get; set; }

		public string From { get; set; }

		public string FromName { get; set; }

		public string CC { get; set; }

		public string BCC { get; set; }

		public string Body { get; set; }
	}
}
