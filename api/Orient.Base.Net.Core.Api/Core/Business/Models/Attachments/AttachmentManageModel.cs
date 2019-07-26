using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Attachments
{
    public class AttachmentManageModel
    {
        [Required]
        public string FileName { get; set; }

        [Required]
        public string Extension { get; set; }

        [Required]
        public string Link { get; set; }

        public void SetDataToModel(Attachment attachment)
        {
            attachment.FileName = FileName;
            attachment.Extension = Extension;
            attachment.Link = Link;
        }
    }
}
