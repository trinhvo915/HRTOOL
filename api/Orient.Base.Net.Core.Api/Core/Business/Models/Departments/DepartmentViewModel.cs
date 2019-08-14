using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Departments
{
    public class DepartmentViewModel
    {
        public DepartmentViewModel()
        {

        }

        public DepartmentViewModel(Department department)
        {
            if (department != null)
            {
                Id = department.Id;
                Name = department.Name;
            }
        }

        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
