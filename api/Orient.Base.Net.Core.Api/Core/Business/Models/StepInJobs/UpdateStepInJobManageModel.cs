using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.StepInJobs
{
    public class UpdateStepInJobManageModel : IValidatableObject
    {
        [StringLength(512)]
        [Required]
        public string Name { get; set; }

        public StatusEnums.Status Status { get; set; }

        public Guid JobId { get; set; }

        public void SetData(StepInJob stepInJob)
        {
            stepInJob.Name = Name;
            stepInJob.Status = Status;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var status = Enum.IsDefined(typeof(StatusEnums.Status), Status);
            if (!status)
            {
                yield return new ValidationResult("Invalid status of Job", new string[] { "Status" });
            }
        }
    }
}
