using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("JobInCategory")]
    public class JobInCategory : BaseEntity
    {
        public JobInCategory() : base()
        {

        }

        public Guid JobId { get; set; }

        public Job Job { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
