using Orient.Base.Net.Core.Api.Core.Business.Models.CalendarTypes;
using Orient.Base.Net.Core.Api.Core.Business.Models.Candidates;
using Orient.Base.Net.Core.Api.Core.Business.Models.Interviews;
using Orient.Base.Net.Core.Api.Core.Business.Models.Users;
using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Calendars
{
    public class CalendarViewModel
    {

        public CalendarViewModel()
        {

        }

        public CalendarViewModel(Calendar calendar) : this()
        {
            if (calendar != null)
            {
                Id = calendar.Id;
                DateStart = calendar.DateStart;
                DateEnd = calendar.DateEnd;
                CalendarTypeId = calendar.CalendarTypeId;
                Description = calendar.Description;
                Interviews = calendar.Interviews != null ? calendar.Interviews.Select(x => new InterviewViewModel(x)).ToArray() : null;
                CalendarType = calendar.CalendarType != null ? new CalendarTypeViewModel(calendar.CalendarType) : null;
                Users = calendar.UserInCalendars != null ? calendar.UserInCalendars.Select(y => new UserViewModel(y.User)).ToArray() : null;
                Reporter = new UserViewModel(calendar.Reporter);
            }
        }

        public Guid Id { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public Guid CalendarTypeId { get; set; }

        public string Description { get; set; }

        public CalendarTypeViewModel CalendarType { get; set; }

        public UserViewModel[] Users { get; set; }

        public InterviewViewModel[] Interviews { get; set; }

        public UserViewModel Reporter { get; set; }
    }
}
