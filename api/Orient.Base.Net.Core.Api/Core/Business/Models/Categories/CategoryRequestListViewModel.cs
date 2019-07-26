using Orient.Base.Net.Core.Api.Core.Business.Models.Base;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Categories
{
    public class CategoryRequestListViewModel : RequestListViewModel
    {
        public CategoryRequestListViewModel() : base()
        {

        }

        public string Query { get; set; }

        public bool? IsActive { get; set; }
    }
}
