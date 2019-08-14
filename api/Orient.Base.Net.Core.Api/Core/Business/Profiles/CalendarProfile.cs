using AutoMapper;
using Orient.Base.Net.Core.Api.Core.Business.Models.Calendars;
using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Profiles
{
    public class CalendarProfile : Profile
    {
        public CalendarProfile()
        {
            CreateMap<CalendarManageModel, Calendar>()
                .ForMember(x => x.UserInCalendars, 
                    y => y.MapFrom(u => u.UserIds.Select(z => new UserInCalendar { UserId = z })
                .ToList()));
        }
    }
}
