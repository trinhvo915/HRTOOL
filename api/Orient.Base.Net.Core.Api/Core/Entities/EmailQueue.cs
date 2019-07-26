using Orient.Base.Net.Core.Api.Core.Business.Models.EmailTemplates;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
	[Table("EmailQueue")]
	public class EmailQueue : BaseEntity
	{
		public EmailQueue() : base()
		{

		}

		public EmailQueue(EmailTemplateResponseModel emailTemplateResponseModel) : this()
		{
			From = emailTemplateResponseModel.From;
			FromName = emailTemplateResponseModel.FromName;
			Subject = emailTemplateResponseModel.Subject;
			CC = emailTemplateResponseModel.CC;
			BCC = emailTemplateResponseModel.BCC;
			Body = emailTemplateResponseModel.Body;
		}

		#region Public Properties

		public EmailPriority Priority { get; set; }

		[StringLength(500)]
		public string From { get; set; }

		[StringLength(500)]
		public string FromName { get; set; }

		[StringLength(500)]
		public string To { get; set; }

		[StringLength(500)]
		public string ToName { get; set; }

		[StringLength(500)]
		public string CC { get; set; }

		[StringLength(500)]
		public string BCC { get; set; }

		[StringLength(500)]
		public string ReplyTo { get; set; }

		[StringLength(1000)]
		public string Subject { get; set; }

		public string Body { get; set; }

		public DateTime? SendLater { get; set; }

		public int SentTries { get; set; }

		public DateTime? SentOn { get; set; }

		public string Message { get; set; }

		public string Attachments { get; set; }

		#endregion
	}

	public enum EmailPriority
	{
		Low = 1,
		Medium = 2,
		High = 3
	}
}
