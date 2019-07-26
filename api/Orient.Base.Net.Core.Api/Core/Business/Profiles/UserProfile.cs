using AutoMapper;
using Orient.Base.Net.Core.Api.Core.Business.Models.Users;
using Orient.Base.Net.Core.Api.Core.Entities;

namespace Orient.Base.Net.Core.Api.Core.Business.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserRegisterModel>().ReverseMap();
        }
    }
}
