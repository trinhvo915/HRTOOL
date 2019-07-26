using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("UserInJob")]
    public class UserInJob : BaseEntity
    {
        public UserInJob() : base()
        {

        }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid JobId { get; set; }

        public Job Job { get; set; }
    }
}
