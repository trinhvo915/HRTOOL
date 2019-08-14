using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Users
{
    public class UserTreeModel
    {
        public UserTreeModel()
        {

        }
        public UserTreeModel(User user) : this()
        {
            if (user != null)
            {
                Name = user.Name;
                AvatarUrl = user.AvatarUrl;
                TaskDescription = user.TaskDescription;
                DepartmentId = user.DepartmentId;
            }
        }

        public string Name { get; set; }

        public string AvatarUrl { get; set; }

        public string TaskDescription { get; set; }

        public Guid? DepartmentId { get; set; }

        public Guid? ParentId { get; set; }
    }
}
