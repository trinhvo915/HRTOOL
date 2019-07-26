using AutoMapper;
using Orient.Base.Net.Core.Api.Core.Business.Models.StepInJobs;
using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Profiles
{
    public class StepInJobProfile : Profile
    {
        public StepInJobProfile()
        {
            CreateMap<StepInJob, StepInJobManageModel>().ReverseMap();
        }
    }
}
