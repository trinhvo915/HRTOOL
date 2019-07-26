using Orient.Base.Net.Core.Api.Core.Entities;
using System;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.EmailQueue
{
	public class EmailQueueManagerModel
	{
		public string From { get; set; }

		public string FromName { get; set; }

		public string To { get; set; }

		public string ToName { get; set; }

		public string CC { get; set; }

		public string BCC { get; set; }

		public string ReplyTo { get; set; }

		public string Subject { get; set; }

		public string Body { get; set; }

		public DateTime? SendLater { get; set; }

		public int SentTries { get; set; }

		public DateTime? SentOn { get; set; }

		public string Message { get; set; }

		public string Attachments { get; set; }

		public Guid? EmailAccountId { get; set; }

		public virtual EmailAccount EmailAccount { get; set; }

		public Guid? CustomerId { get; set; }

		public void setDataToModel(Entities.EmailQueue emailQueue)
		{
			emailQueue.SentOn = SentOn;
		}
	}
}
