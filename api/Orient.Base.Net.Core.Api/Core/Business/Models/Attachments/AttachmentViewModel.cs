using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Attachments
{
    public class AttachmentViewModel
    {
        public AttachmentViewModel()
        {

        }
        public AttachmentViewModel(Attachment attachment) : this()
        {
            if (attachment != null)
            {
                FileName = attachment.FileName;
                Extension = attachment.Extension;
                Link = attachment.Link;
            }
        }

        public string FileName { get; set; }

        public string Extension { get; set; }

        public string Link { get; set; }
    }
}
