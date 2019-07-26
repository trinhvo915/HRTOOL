using Orient.Base.Net.Core.Api.Core.Business.Models.TemplateAttachments;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.EmailTemplates
{
    public class EmailTemplateManageModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string From { get; set; }

        [Required]
        public string FromName { get; set; }

        [Required]
        public string Subject { get; set; }

        public string CC { get; set; }

        public string BCC { get; set; }

        public TemplateAttachmentManageModel[] TemplateAttachments { get; set; }

        [Required]
        public string Body { get; set; }

        public void SetDataToModel(EmailTemplate emailTemplate)
        {
            emailTemplate.Name = Name;
            emailTemplate.From = From;
            emailTemplate.FromName = FromName;
            emailTemplate.Subject = Subject;
            emailTemplate.CC = CC;
            emailTemplate.BCC = BCC;
            emailTemplate.Body = Body;
        }
    }
}
