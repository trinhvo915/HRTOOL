using AutoMapper;
using Orient.Base.Net.Core.Api.Core.Business.Models.Jobs;
using Orient.Base.Net.Core.Api.Core.Entities;

namespace Orient.Base.Net.Core.Api.Core.Business.Profiles
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<Job, JobManageModel>().ReverseMap();
        }
    }
}
