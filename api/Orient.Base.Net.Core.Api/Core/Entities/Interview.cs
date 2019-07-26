using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("Interview")]
    public class Interview : BaseEntity
    {
        public Interview() : base()
        {
            Status = InterviewEnums.Status.Pending;
        }

        [Required]
        public InterviewEnums.Status Status { get; set; }

        [Required]
        public Guid CandidateId { get; set; }

        public Candidate Candidate { get; set; }

        [Required]
        public Guid CalendarId { get; set; }

        public Calendar Calendar { get; set; }

        public List<AttachmentInInterview> AttachmentInInterviews { get; set; }
    }
}
