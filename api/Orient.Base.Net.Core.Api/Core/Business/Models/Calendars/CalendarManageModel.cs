using Orient.Base.Net.Core.Api.Core.Business.IoC;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Calendars
{
    public class CalendarManageModel : IValidatableObject
    {
        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public Guid CalendarTypeId { get; set; }

        public string Description { get; set; }

        public Guid[] UserIds { get; set; }

        public void SetDataToModel(Calendar calendar)
        {
            calendar.DateStart = DateStart;
            calendar.DateEnd = DateEnd;
            calendar.CalendarTypeId = CalendarTypeId;
            calendar.Description = Description;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            var _userRepository = IoCHelper.GetInstance<IRepository<User>>();
            var _calendarTypeRepository = IoCHelper.GetInstance<IRepository<CalendarType>>();

            var calendarType = _calendarTypeRepository.GetAll().FirstOrDefault(x => x.Id == CalendarTypeId);
            if (calendarType == null)
            {
                yield return new ValidationResult(CalendarTypeMessagesConstants.NOT_FOUND, new string[] { "CalendarTypeId" });
            }

            if (UserIds.Length == 0)
            {
                yield return new ValidationResult(UserMessagesConstants.EMPTY_USER_LIST, new string[] { "UserIds" });
            }
            else
            {
                var users = _userRepository.GetAll().Select(x => x.Id);
                if (!UserIds.All(x => users.Contains(x)))
                {
                    yield return new ValidationResult(UserMessagesConstants.NOT_FOUND_USER_IN_LIST, new string[] { "UserIds" });
                }
            }
            if (DateStart >= DateEnd)
            {
                yield return new ValidationResult("DateEnd must be greater than DateStart", new string[] { "DateStart" });
            }
        }
    }
}
