using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("AttachmentInInterview")]
    public class AttachmentInInterview : BaseEntity
    {
        public AttachmentInInterview() : base()
        {

        }

        public Guid InterviewId { get; set; }

        public Interview Interview { get; set; }

        public Guid AttachmentId { get; set; }

        public Attachment Attachment { get; set; }
    }
}
