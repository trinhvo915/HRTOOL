using Orient.Base.Net.Core.Api.Core.Business.Models.Base;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Users
{
    public class UserRequestListViewModel : RequestListViewModel
    {
        public UserRequestListViewModel() : base()
        {

        }

        public string Query { get; set; }
        public bool? IsActive { get; set; }
    }
}
