using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("UserInCalendar")]
    public class UserInCalendar : BaseEntity
    {
        public UserInCalendar() : base()
        {

        }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid CalendarId { get; set; }

        public Calendar Calendar { get; set; }
    }
}
