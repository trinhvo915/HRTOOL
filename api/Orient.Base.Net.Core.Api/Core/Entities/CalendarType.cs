using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("CalendarType")]
    public class CalendarType : BaseEntity
    {
        public CalendarType() : base()
        {

        }

        [StringLength(512)]
        [Required]
        public string Name { get; set; }

        public List<Calendar> Calendars { get; set; }
    }
}
