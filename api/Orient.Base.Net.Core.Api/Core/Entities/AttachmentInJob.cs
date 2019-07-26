using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("AttachmentInJob")]
    public class AttachmentInJob : BaseEntity
    {
        public AttachmentInJob() : base()
        {

        }

        public Guid JobId { get; set; }

        public Job Job { get; set; }

        public Guid AttachmentId { get; set; }

        public Attachment Attachment { get; set; }
    }
}
