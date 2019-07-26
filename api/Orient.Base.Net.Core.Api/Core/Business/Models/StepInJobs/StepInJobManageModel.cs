using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.StepInJobs
{
    public class StepInJobManageModel
    {
        [StringLength(512)]
        [Required]
        public string Name { get; set; }
    }
}
