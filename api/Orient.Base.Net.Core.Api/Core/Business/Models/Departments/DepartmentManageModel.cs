using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Departments
{
    public class DepartmentManageModel
    {
        public DepartmentManageModel()
        {

        }

        [Required]
        public string Name { get; set; }

        public void SetDateToModel(Department department)
        {
            department.Name = Name;
        }
    }
}
