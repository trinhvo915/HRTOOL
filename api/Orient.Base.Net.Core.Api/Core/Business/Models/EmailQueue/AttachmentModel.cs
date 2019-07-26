using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.EmailQueue
{
	public class AttachmentModel
	{
		public AttachmentModel()
		{

		}
		public AttachmentModel(Attachment attachment)
		{
			FileName = attachment.FileName + attachment.Extension;
			Link = attachment.Link;
		}

        public AttachmentModel(TemplateAttachment attachment)
        {
            FileName = attachment.FileName + attachment.Extension;
            Link = attachment.Link;
        }

        [StringLength(512)]
		[Required]
		public string FileName { get; set; }

		[StringLength(512)]
		[Required]
		public string Link { get; set; }

	}
}
