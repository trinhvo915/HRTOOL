using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("Attachment")]
    public class Attachment : BaseEntity
    {
        public Attachment() : base()
        {

        }

        [StringLength(512)]
        [Required]
        public string FileName { get; set; }

        [StringLength(512)]
        [Required]
        public string Extension { get; set; }

        [StringLength(512)]
        [Required]
        public string Link { get; set; }

        public List<AttachmentInJob> AttachmentInJobs { get; set; }

        public List<AttachmentInInterview> AttachmentInInterviews { get; set; }
    }
}
