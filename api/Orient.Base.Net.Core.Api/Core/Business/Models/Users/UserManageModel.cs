using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Users
{
    public class UserManageModel
    {
        public string Color { get; set; }

        public Guid[] RoleIds { get; set; }

        [Required]
        public Guid DepartmentId { get; set; }

        public void SetDataToModel(User user)
        {
            user.Color = Color;
            user.DepartmentId = DepartmentId;
        }
    }
}
