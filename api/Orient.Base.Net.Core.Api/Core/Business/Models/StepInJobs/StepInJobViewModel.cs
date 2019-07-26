using Orient.Base.Net.Core.Api.Core.Common.Helpers;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.StepInJobs
{
    public class StepInJobViewModel
    {
        public StepInJobViewModel()
        {

        }

        public StepInJobViewModel(StepInJob stepInJob)
        {
            Id = stepInJob.Id;
            Order = stepInJob.RecordOrder;
            Name = stepInJob.Name;
            Status = stepInJob.Status;
            StatusText = stepInJob.Status.GetEnumDescription();
            JobId = stepInJob.JobId;
        }

        public Guid Id { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

        public string StatusText { get; set; }

        public StatusEnums.Status Status { get; set; }

        public Guid JobId { get; set; }
    }
}
