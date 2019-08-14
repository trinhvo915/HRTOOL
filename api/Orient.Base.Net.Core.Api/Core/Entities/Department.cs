using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("Department")]
    public class Department : BaseEntity
    {
        public Department() : base()
        {

        }

        [Required]
        public string Name { get; set; }

        public Guid? ParentId { get; set; }

        List<User> Users { get; set; }
    }
}
