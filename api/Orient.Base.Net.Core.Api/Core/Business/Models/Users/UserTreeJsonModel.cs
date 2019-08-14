using Newtonsoft.Json;
using Orient.Base.Net.Core.Api.Core.Business.IoC;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Users
{
    public class UserTreeJsonModel
    {
        public UserTreeJsonModel() 
        {

        }

        public UserTreeJsonModel(UserTreeModel e) : this()
        {
            Name = e.Name.ToString();
            AvatarUrl = e.AvatarUrl != null ? e.AvatarUrl.ToString() : null;
            TaskDescription = e.TaskDescription != null ? e.TaskDescription.ToString() : null;
            DepartmentId = e.DepartmentId;
            ParentId = IoCHelper.GetInstance<IRepository<Department>>()
                .GetAll().Where(x => x.Id == e.DepartmentId)
                .Select(x => x.ParentId)
                .SingleOrDefault();
        }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("AvatarUrl")]
        public string AvatarUrl { get; set; }

        [JsonProperty("TaskDescription")]
        public string TaskDescription { get; set; }

        [JsonProperty("DepartmentId")]
        public Guid? DepartmentId { get; set; }

        [JsonProperty("Children")]
        public List<UserTreeJsonModel> Children { get; set; }

        [JsonProperty("Parent")]
        public Guid? ParentId { get; set; }
    }
}
