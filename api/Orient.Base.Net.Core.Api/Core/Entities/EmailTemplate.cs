using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Entities
{

    [Table("EmailTemplate")]
    public class EmailTemplate : BaseEntity
    {
        public EmailTemplate() : base()
        {

        }

        public string Name { get; set; }

        public string Subject { get; set; }

        public string From { get; set; }

        public string FromName { get; set; }

        public string CC { get; set; }

        public string BCC { get; set; }

        public string Body { get; set; }

        public bool IsDefault { get; set; }

        public EmailTemplateType Type { get; set; }

        public virtual ICollection<TemplateAttachment> TemplateAttachments { get; set; }
    }

    public enum EmailTemplateType
    {
        [Description("Job")]
        Job = 1,
        [Description("Interview")]
        Interview = 2,
        [Description("Calendar")]
        Calendar = 3
    }


}
