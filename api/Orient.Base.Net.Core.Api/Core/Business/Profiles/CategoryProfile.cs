using AutoMapper;
using Orient.Base.Net.Core.Api.Core.Business.Models.Categories;
using Orient.Base.Net.Core.Api.Core.Entities;

namespace Orient.Base.Net.Core.Api.Core.Business.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryManageModel>().ReverseMap();
        }
    }
}
