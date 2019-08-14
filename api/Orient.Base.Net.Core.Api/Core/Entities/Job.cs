using System;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("Job")]
    public class Job : BaseEntity
    {
        public Job() : base()
        {
            Status = StatusEnums.Status.Pending;
            DateRepeat = 0;
            IsDisable = false;
            IsProcess = false;
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

        public int DateRepeat { get; set; }

        public bool IsProcess { get; set; }

        public bool IsDisable { get; set; }

        public Guid? IdLink { get; set; }
    }
}
