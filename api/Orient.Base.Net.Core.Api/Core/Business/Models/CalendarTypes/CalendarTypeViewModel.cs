using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.CalendarTypes
{
    public class CalendarTypeViewModel
    {
        public CalendarTypeViewModel()
        {

        }

        public CalendarTypeViewModel(CalendarType calendarType) : this()
        {
            if (calendarType != null)
            {
                Id = calendarType.Id;
                Name = calendarType.Name;
            }
        }

        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
