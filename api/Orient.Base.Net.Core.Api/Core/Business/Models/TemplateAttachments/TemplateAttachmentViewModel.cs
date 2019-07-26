using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.TemplateAttachments
{
    public class TemplateAttachmentViewModel
    {
        public TemplateAttachmentViewModel(TemplateAttachment templateAttachment)
        {
            Link = templateAttachment.Link;
            FileName = templateAttachment.FileName;
            Extension = templateAttachment.Extension;
        }

        public string FileName { get; set; }

        public string Link { get; set; }

        public string Extension { get; set; }
    }
}
