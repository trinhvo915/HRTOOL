using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("Category")]
    public class Category : BaseEntity
    {
        public Category() : base()
        {

        }

        [StringLength(512)]
        [Required]
        public string Name { get; set; }

        public List<JobInCategory> JobInCategories { get; set; } 

    }
}
