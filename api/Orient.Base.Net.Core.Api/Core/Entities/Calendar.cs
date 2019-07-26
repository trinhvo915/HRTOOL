using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("Calendar")]
    public class Calendar : BaseEntity
    {
        public Calendar() : base()
        {

        }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public Guid CalendarTypeId { get; set; }

        public CalendarType CalendarType { get; set; }

        public string Description { get; set; }

        public List<Interview> Interviews { get; set; }

        public List<UserInCalendar> UserInCalendars { get; set; }

        public Guid? ReporterId { get; set; }

        [ForeignKey("ReporterId")]
        public User Reporter { get; set; }
    }
}
