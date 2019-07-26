using Orient.Base.Net.Core.Api.Core.Business.Models.Base;
using Orient.Base.Net.Core.Api.Core.Entities;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Roles
{
    public class RoleViewModel : ApiBaseModel
    {
        public RoleViewModel() : base()
        {

        }

        public RoleViewModel(Role role)
        {
            if (role != null)
            {
                Id = role.Id;
                Name = role.Name;
            }
        }
    }
}
