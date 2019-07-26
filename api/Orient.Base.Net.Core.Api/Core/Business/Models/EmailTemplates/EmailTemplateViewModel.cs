using Orient.Base.Net.Core.Api.Core.Business.Models.TemplateAttachments;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.EmailTemplates
{
    public class EmailTemplateViewModel
    {
        public EmailTemplateViewModel()
        {

        }

        public EmailTemplateViewModel(EmailTemplate emailTemplate) : this()
        {
            if (emailTemplate != null)
            {
                Id = emailTemplate.Id;
                Name = emailTemplate.Name;
                Subject = emailTemplate.Subject;
                From = emailTemplate.From;
                FromName = emailTemplate.FromName;
                CC = emailTemplate.CC;
                BCC = emailTemplate.BCC;
                Body = emailTemplate.Body;
                Type = emailTemplate.Type;
                TemplateAttachments = emailTemplate.TemplateAttachments.Select(x => new TemplateAttachmentViewModel(x)).ToArray();
            }
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public string From { get; set; }

        public string FromName { get; set; }

        public string CC { get; set; }

        public string BCC { get; set; }

        public string Body { get; set; }

        public EmailTemplateType Type { get; set; }

        public TemplateAttachmentViewModel[] TemplateAttachments { get; set; }
    }
}
