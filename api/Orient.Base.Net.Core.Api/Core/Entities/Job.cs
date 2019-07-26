using System;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("Job")]
    public class Job : BaseEntity
    {
        public Job() : base()
        {
            Status = StatusEnums.Status.Pending;
        }

        [StringLength(512)]
        [Required]
        public string Name { get; set; }

        public StatusEnums.Status Status { get; set; }

        public PriorityEnums.Priority Priority { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public string Description { get; set; }

        public Guid ReporterId { get; set; }

        [ForeignKey("ReporterId")]
        public User Reporter { get; set; }

        public List<Comment> Comments { get; set; }

        public List<UserInJob> UserInJobs { get; set; }

        public List<JobInCategory> JobInCategories { get; set; }

        public List<AttachmentInJob> AttachmentInJobs { get; set; }

        public List<StepInJob> StepInJobs { get; set; }
    }
}
