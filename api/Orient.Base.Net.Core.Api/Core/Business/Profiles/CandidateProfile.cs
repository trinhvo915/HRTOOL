using AutoMapper;
using Orient.Base.Net.Core.Api.Core.Business.Models.Candidates;
using Orient.Base.Net.Core.Api.Core.Entities;

namespace Orient.Base.Net.Core.Api.Core.Business.Profiles
{
    public class CandidateProfile : Profile
    {
        public CandidateProfile()
        {
            CreateMap<Candidate, CandidateManageModel>().ReverseMap();
        }
    }
}
