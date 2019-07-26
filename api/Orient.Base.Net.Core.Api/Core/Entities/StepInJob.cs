using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("StepInJob")]
    public class StepInJob : BaseEntity
    {
        public StepInJob() : base()
        {
            Status = StatusEnums.Status.Pending;
        }

        [StringLength(512)]
        [Required]
        public string Name { get; set; }

        public StatusEnums.Status Status { get; set; }

        public Guid JobId { get; set; }

        public Job Job { get; set; }
    }
}
