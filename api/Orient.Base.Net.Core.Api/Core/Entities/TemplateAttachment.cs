using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("TemplateAttachment")]
    public class TemplateAttachment : BaseEntity
    {
        public TemplateAttachment() : base()
        {

        }

        public string FileName { get; set; }

        public string Link { get; set; }

        public string Extension { get; set; }

        public Guid EmailTemplateId { get; set; }

        public virtual EmailTemplate EmailTemplate { get; set; }
    }
}
