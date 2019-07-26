using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.TemplateAttachments
{
    public class TemplateAttachmentManageModel
    {
        [Required]
        public string Link { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string Extension { get; set; }

        public void SetDataToModel(TemplateAttachment templateAttachment)
        {
            templateAttachment.Link = Link;
            templateAttachment.FileName = FileName;
            templateAttachment.Extension = Extension;
        }
    }
}
