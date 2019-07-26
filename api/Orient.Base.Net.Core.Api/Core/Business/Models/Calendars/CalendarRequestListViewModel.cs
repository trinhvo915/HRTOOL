using Orient.Base.Net.Core.Api.Core.Business.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Calendars
{
    public class CalendarRequestListViewModel 
    {

        public CalendarRequestListViewModel() 
        {

        }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }
    }
}
