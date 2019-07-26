using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Orient.Base.Net.Core.Api.Core.Business.Models.Base;
using Orient.Base.Net.Core.Api.Core.Business.Models.Calendars;
using Orient.Base.Net.Core.Api.Core.Business.Models.EmailQueue;
using Orient.Base.Net.Core.Api.Core.Business.Models.EmailTemplates;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.Common.Helpers;
using Orient.Base.Net.Core.Api.Core.Common.Reflections;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Services
{
    public interface ICalendarService
    {
        Task<IEnumerable<CalendarViewModel>> ListCalendarAsync(CalendarRequestListViewModel calendarRequestListViewModel);

        Task<ResponseModel> GetCalendarByIdAsync(Guid? id);

        Task<ResponseModel> CreateCalendarAsync(Guid CurrentUserId, CalendarManageModel calendarManageModel);

        Task<ResponseModel> UpdateCalendarAsync(Guid currentUserId, Guid id, CalendarManageModel calendarManageModel);

        Task<ResponseModel> DeteteCalendarAsync(Guid currentUserId, Guid id);

        Task<IEnumerable<CalendarViewModel>> ListCalendarByIdUserAsync(Guid? id, CalendarRequestListViewModel calendarRequestListViewModel);
    }

    public class CalendarService : ICalendarService
    {
        #region Fields

        private readonly IRepository<Calendar> _calendarRepository;
        private readonly IRepository<UserInCalendar> _userInCalendarRepository;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;
        private readonly IRepository<EmailQueue> _emailQueueRepository;
        private readonly IRepository<User> _userRepository;

        #endregion

        #region Constructor

        public CalendarService(IRepository<Calendar> calendarRepository, IRepository<UserInCalendar> userInCalendarRepository, IRepository<EmailTemplate> emailTemplateRepository, IRepository<EmailQueue> emailQueueRepository, IRepository<User> userRepository)
        {
            _calendarRepository = calendarRepository;
            _userInCalendarRepository = userInCalendarRepository;
            _emailTemplateRepository = emailTemplateRepository;
            _emailQueueRepository = emailQueueRepository;
            _userRepository = userRepository;
        }

        #endregion

        private IQueryable<Calendar> GetAll()
        {
            return _calendarRepository.GetAll()
                    .Include(x => x.CalendarType)
                        .Include(x => x.UserInCalendars)
                            .ThenInclude(user => user.User)
                                .ThenInclude(y => y.UserInRoles)
                                    .ThenInclude(z => z.Role)
                    .Include(x => x.Interviews)
                        .ThenInclude(interview => interview.Candidate)
                    .Include(x => x.Interviews)
						.ThenInclude(a => a.AttachmentInInterviews)
                            .ThenInclude(AttachmentInInterviews => AttachmentInInterviews.Attachment)
					.Include(x => x.Reporter);
        }

        private List<string> GetAllPropertyNameOfCalendarViewModel()
        {
            var calendarViewModel = new CalendarViewModel();

            var type = calendarViewModel.GetType();

            return ReflectionUtilities.GetAllPropertyNamesOfType(type);
        }

        public async Task<IEnumerable<CalendarViewModel>> ListCalendarAsync(CalendarRequestListViewModel calendarRequestListViewModel)
        {
            var list = await GetAll()
                .Where(x => !((x.DateEnd <= calendarRequestListViewModel.DateStart) && (calendarRequestListViewModel.DateStart.HasValue)
                || (x.DateStart >= calendarRequestListViewModel.DateEnd) && (calendarRequestListViewModel.DateEnd.HasValue)))
                .Select(x => new CalendarViewModel(x)).ToListAsync();

            return list;
        }

        public async Task<IEnumerable<CalendarViewModel>> ListCalendarByIdUserAsync(Guid? currentUserId, CalendarRequestListViewModel calendarRequestListViewModel)
        {
            var list = await GetAll()
                .Where(x => !((x.DateEnd <= calendarRequestListViewModel.DateStart) && (calendarRequestListViewModel.DateStart.HasValue)
                || (x.DateStart >= calendarRequestListViewModel.DateEnd) && (calendarRequestListViewModel.DateEnd.HasValue))
                && (x.UserInCalendars.Any(y => y.UserId == currentUserId) || (x.ReporterId == currentUserId)))
                .Select(x => new CalendarViewModel(x)).ToListAsync();

            return list;
        }

        public async Task<ResponseModel> GetCalendarByIdAsync(Guid? id)
        {
            var calendar = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (calendar != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new CalendarViewModel(calendar)
                };
            }
            else
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
        }

        public async Task<ResponseModel> CreateCalendarAsync(Guid currentUserId, CalendarManageModel calendarManageModel)
        {
            var calendar = AutoMapper.Mapper.Map<Calendar>(calendarManageModel);
            calendar.ReporterId = currentUserId;
            await _calendarRepository.InsertAsync(calendar);

            //create email queue
            // get job email template
            var jobEmailTemplate = await _emailTemplateRepository.GetAll().Include(x => x.TemplateAttachments).FirstOrDefaultAsync(x => x.Type == EmailTemplateType.Calendar);

            //// parse data
            var userFrom = await _userRepository.GetByIdAsync(currentUserId);
            jobEmailTemplate.From = userFrom.Email;
            jobEmailTemplate.FromName = userFrom.Name;

            List<string> listAttachment = new List<string>();
            foreach (var attachment in jobEmailTemplate.TemplateAttachments)
            {
                var attachmentQueue = new AttachmentModel(attachment);
                var attachmentString = JsonConvert.SerializeObject(attachmentQueue, Formatting.Indented);
                listAttachment.Add(attachmentString);
            }

            var attachmentEmailQueue = String.Join(EmailQueueConstants.UNIT_ATTACHMENT_SPLIT, listAttachment);

            var listToUser = new List<User>();
            foreach (var userInCalendar in calendar.UserInCalendars)
            {
                var user = await _userRepository.GetByIdAsync(userInCalendar.UserId);
                listToUser.Add(user);
            }
            foreach (var user in listToUser)
            {
                var model = new ModelEmailCalendar();
                model.ToName = user.Name;
                model.FromName = userFrom.Name;
                model.JobName = "Calendar";
                EmailTemplateResponseModel emailTemplateResponseModel = await EmailTemplateHelper.ParseModelAsync<ModelEmailCalendar>(jobEmailTemplate, model);
                EmailQueue emailQueue = new EmailQueue(emailTemplateResponseModel);
                emailQueue.To = user.Email;
                emailQueue.ToName = user.Name;
                emailQueue.Attachments = attachmentEmailQueue;
                await _emailQueueRepository.InsertAsync(emailQueue);
            }

            return new ResponseModel()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = MessageConstants.CREATED_SUCCESSFULLY
            };
        }

        public async Task<ResponseModel> UpdateCalendarAsync(Guid currentUserId, Guid id, CalendarManageModel calendarManageModel)
        {
            var calendar = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (calendar == null || calendar.ReporterId != currentUserId)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
            else
            {
                calendarManageModel.SetDataToModel(calendar);
                await _userInCalendarRepository.DeleteAsync(calendar.UserInCalendars);

                List<UserInCalendar> userInCalendars = new List<UserInCalendar>();

                foreach (var userId in calendarManageModel.UserIds)
                {
                    var calendarInUser = new UserInCalendar()
                    {
                        UserId = userId,
                        CalendarId = id
                    };
                    userInCalendars.Add(calendarInUser);
                }

                await _calendarRepository.UpdateAsync(calendar);

                await _userInCalendarRepository.InsertAsync(userInCalendars);

                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new CalendarViewModel(calendar),
                    Message = MessageConstants.UPDATED_SUCCESSFULLY
                };

            }
        }

        public async Task<ResponseModel> DeteteCalendarAsync(Guid currentUserId, Guid id)
        {
            var calendar = await _calendarRepository.GetByIdAsync(id);

            if (calendar == null || calendar.ReporterId != currentUserId)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
            else
            {
                await _calendarRepository.DeleteAsync(id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
        }
    }
}
