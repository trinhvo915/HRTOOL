using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Orient.Base.Net.Core.Api.Core.Business.Models.Base;
using Orient.Base.Net.Core.Api.Core.Business.Models.EmailQueue;
using Orient.Base.Net.Core.Api.Core.Business.Models.EmailTemplates;
using Orient.Base.Net.Core.Api.Core.Business.Models.Interviews;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.Common.Helpers;
using Orient.Base.Net.Core.Api.Core.Common.Reflections;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Services
{
    public interface IInterviewService
    {
        Task<PagedList<InterviewViewModel>> ListInterviewAsync(InterviewRequestListViewModel interviewRequestListViewModel);

        Task<PagedList<InterviewViewModel>> ListInterviewByUserIdAsync(Guid? userId, InterviewRequestListViewModel interviewRequestListViewModel);

        Task<List<InterviewViewExcelModel>> GetInterviewViewExcel(BaseRequestGetAllViewModel baseRequestGetAllViewModel);

        Task<ResponseModel> GetInterviewByIdAsync(Guid? Id);

        Task<ResponseModel> CreateInterviewAsync(Guid currentUserId, InterviewManageModel interviewManageModel);

        Task<ResponseModel> UpdateInterviewAsync(Guid Id, InterviewManageModel interviewManageModel);

        Task<ResponseModel> UpdateInterviewStatus(Guid Id, InterviewStatusManageModel interviewStatusManageModel);

        Task<ResponseModel> DeleteInterviewAsync(Guid Id);

        Task<List<InterviewStatModel>> GetInterviewStatAnimationAsync(InterviewRequestStat interviewRequestStat);

        Task<List<InterviewStatModel>> GetInterviewStatAsync();
    }

    public class InterviewService : IInterviewService
    {
        #region Fields

        private readonly IRepository<Interview> _interviewRepository;
        private readonly IRepository<CalendarType> _calendarTypeRepository;
        private readonly IRepository<Calendar> _calendarRepository;
        private readonly IRepository<Attachment> _attachmentRepository;
        private readonly IRepository<AttachmentInInterview> _attachmentInInterviewRepository;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;
        private readonly IRepository<EmailQueue> _emailQueueRepository;
        private readonly IRepository<Candidate> _candidateRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserInRole> _userInRoleRepository;
        #endregion

        #region Constructor

        public InterviewService(
            IRepository<Interview> interviewRepository,
            IRepository<CalendarType> calendarTypeRepository,
            IRepository<Attachment> attachmentRepository,
            IRepository<AttachmentInInterview> attachmentInInterviewRepository,
            IRepository<Calendar> calendarRepository,
            IRepository<EmailTemplate> emailTemplateRepository,
            IRepository<EmailQueue> emailQueueRepository,
            IRepository<Candidate> candidateRepository,
            IRepository<User> userRepository,
            IRepository<UserInRole> userInRoleRepository)
        {
            _interviewRepository = interviewRepository;
            _calendarTypeRepository = calendarTypeRepository;
            _attachmentRepository = attachmentRepository;
            _attachmentInInterviewRepository = attachmentInInterviewRepository;
            _calendarRepository = calendarRepository;
            _emailTemplateRepository = emailTemplateRepository;
            _emailQueueRepository = emailQueueRepository;
            _candidateRepository = candidateRepository;
            _userRepository = userRepository;
            _userInRoleRepository = userInRoleRepository;
        }

        #endregion

        #region Private Methods

        private IQueryable<Interview> GetAll()
        {
            return _interviewRepository.GetAll()
                    .Include(x => x.Calendar)
                        .ThenInclude(x => x.UserInCalendars)
                            .ThenInclude(x => x.User)
                                .ThenInclude(x => x.UserInRoles)
                                    .ThenInclude(x => x.Role)
                    .Include(x => x.Candidate)
                    .Include(x => x.AttachmentInInterviews)
                        .ThenInclude(AttachmentInInterviews => AttachmentInInterviews.Attachment)
                .Where(x => !x.RecordDeleted);
        }

        private List<string> GetAllPropertyNameOfInterviewViewModel()
        {
            var userViewModel = new InterviewViewModel();
            var type = userViewModel.GetType();
            return ReflectionUtilities.GetAllPropertyNamesOfType(type);
        }

        #endregion

        #region Other Method

        public async Task<PagedList<InterviewViewModel>> ListInterviewAsync(InterviewRequestListViewModel interviewRequestListViewModel)
        {
            var list = await GetAll()
            .Where(x => (!interviewRequestListViewModel.IsActive.HasValue || x.RecordActive == interviewRequestListViewModel.IsActive)
                && (string.IsNullOrEmpty(interviewRequestListViewModel.Query)
                    || (x.Candidate.Name.Contains(interviewRequestListViewModel.Query)
                    || (x.Candidate.Email.Contains(interviewRequestListViewModel.Query))
                    || (x.Calendar.UserInCalendars.Any(y => y.User.Name.Contains(interviewRequestListViewModel.Query)))
                    || (x.Calendar.UserInCalendars.Any(y => y.User.Email.Contains(interviewRequestListViewModel.Query)))
                    )))
               .Select(x => new InterviewViewModel(x)).ToListAsync();

            var interviewViewModelProperties = GetAllPropertyNameOfInterviewViewModel();
            var requestPropertyName = !string.IsNullOrEmpty(interviewRequestListViewModel.SortName) ? interviewRequestListViewModel.SortName.ToLower() : string.Empty;
            string matchedPropertyName = interviewViewModelProperties.FirstOrDefault(x => x.ToLower() == requestPropertyName);

            if (string.IsNullOrEmpty(matchedPropertyName))
            {
                matchedPropertyName = "Status";
            }

            if (matchedPropertyName == "Candidate")
            {
                list = interviewRequestListViewModel.IsDesc ? list.OrderByDescending(x => x.Candidate.Name).ToList() : list.OrderBy(x => x.Candidate.Name).ToList();
            }
            else
            {
                var type = typeof(InterviewViewModel);
                var sortProperty = type.GetProperty(matchedPropertyName);
                list = interviewRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();
            }

            return new PagedList<InterviewViewModel>(list, interviewRequestListViewModel.Skip ?? CommonConstants.Config.DEFAULT_SKIP, interviewRequestListViewModel.Take ?? CommonConstants.Config.DEFAULT_TAKE);
        }

        public async Task<PagedList<InterviewViewModel>> ListInterviewByUserIdAsync(Guid? userId, InterviewRequestListViewModel interviewRequestListViewModel)
        {
            var list = await GetAll()
            .Where(x => (!interviewRequestListViewModel.IsActive.HasValue || x.RecordActive == interviewRequestListViewModel.IsActive)
                && (string.IsNullOrEmpty(interviewRequestListViewModel.Query)
                 || (x.Candidate.Name.Contains(interviewRequestListViewModel.Query)
                 || (x.Candidate.Email.Contains(interviewRequestListViewModel.Query))
                    ))
                  && (x.Calendar.UserInCalendars.Any(y => y.UserId == userId)))
               .Select(x => new InterviewViewModel(x)).ToListAsync();

            var interviewViewModelProperties = GetAllPropertyNameOfInterviewViewModel();
            var requestPropertyName = !string.IsNullOrEmpty(interviewRequestListViewModel.SortName) ? interviewRequestListViewModel.SortName.ToLower() : string.Empty;
            string matchedPropertyName = interviewViewModelProperties.FirstOrDefault(x => x.ToLower() == requestPropertyName);

            if (string.IsNullOrEmpty(matchedPropertyName))
            {
                matchedPropertyName = "Status";
            }

            if (matchedPropertyName == "Candidate")
            {
                list = interviewRequestListViewModel.IsDesc ? list.OrderByDescending(x => x.Candidate.Name).ToList() : list.OrderBy(x => x.Candidate.Name).ToList();
            }
            else
            {
                var type = typeof(InterviewViewModel);
                var sortProperty = type.GetProperty(matchedPropertyName);
                list = interviewRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();
            }

            return new PagedList<InterviewViewModel>(list, interviewRequestListViewModel.Skip ?? CommonConstants.Config.DEFAULT_SKIP, interviewRequestListViewModel.Take ?? CommonConstants.Config.DEFAULT_TAKE);
        }

        public async Task<List<InterviewViewExcelModel>> GetInterviewViewExcel(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
        {
            var listInterview = await GetAll()
            .Where(x => (string.IsNullOrEmpty(baseRequestGetAllViewModel.Query)
                    || (x.Candidate.Name.Contains(baseRequestGetAllViewModel.Query)
                    || (x.Candidate.Email.Contains(baseRequestGetAllViewModel.Query))
                    || (x.Calendar.UserInCalendars.Any(y => y.User.Name.Contains(baseRequestGetAllViewModel.Query)))
                    || (x.Calendar.UserInCalendars.Any(y => y.User.Email.Contains(baseRequestGetAllViewModel.Query)))
                    )))
               .Select(x => new InterviewViewExcelModel(x)).ToListAsync();

            return listInterview;
        }

        public async Task<ResponseModel> GetInterviewByIdAsync(Guid? id)
        {
            var interview = await GetAll().FirstOrDefaultAsync(x => x.Id == id);

            if (interview != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new InterviewViewModel(interview)
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

        public async Task<ResponseModel> CreateInterviewAsync(Guid currentUserId, InterviewManageModel interviewManageModel)
        {
            var calendarType = await _calendarTypeRepository.FetchFirstAsync(x => x.Name == "Interview");
            if (calendarType == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = CalendarTypeMessagesConstants.NOT_FOUND
                };
            }

            var isInterviewExist = _interviewRepository.GetAll().Any(x => x.CandidateId == interviewManageModel.CandidateId && x.Status != InterviewEnums.Status.Failed);
            if (isInterviewExist)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = InterviewMessagesConstants.EXISTED_CANDIDATE
                };
            }
            var interview = new Interview();

            var calendar = new Calendar();
            calendar.CalendarTypeId = calendarType.Id;
            calendar.DateEnd = interviewManageModel.DateEnd;
            calendar.DateStart = interviewManageModel.DateStart;
            calendar.Description = interviewManageModel.Description;
            calendar.ReporterId = currentUserId;

            var userInCalendars = new List<UserInCalendar>();
            var hr = await _userInRoleRepository.FetchFirstAsync(x => x.RoleId == RoleConstants.HRMId);
            Guid hrId = hr.UserId;
            var bod = await _userInRoleRepository.FetchFirstAsync(x => x.RoleId == RoleConstants.BODId);
            Guid bodId = bod.UserId;
            foreach (var userId in interviewManageModel.InterviewerIds.Concat(new Guid[] { hrId, bodId }))
            {
                var userInCalendar = new UserInCalendar()
                {
                    CalendarId = calendar.Id,
                    UserId = userId
                };
                userInCalendars.Add(userInCalendar);
            }

            calendar.UserInCalendars = userInCalendars;

            interview.Calendar = calendar;
            interview.CandidateId = interviewManageModel.CandidateId;
            await _interviewRepository.InsertAsync(interview);

            var attachments = new List<Attachment>();
            if (interviewManageModel.Attachments != null && interviewManageModel.Attachments.Length > 0)
            {
                // Create Attachment
                foreach (var attachment in interviewManageModel.Attachments)
                {
                    var att = new Attachment();
                    attachment.SetDataToModel(att);
                    attachments.Add(att);
                }

                await _attachmentRepository.InsertAsync(attachments);

                // Create AttachmentInInterview
                var attachmentInInterviews = new List<AttachmentInInterview>();
                foreach (var attachment in attachments)
                {
                    var attachmentInInterview = new AttachmentInInterview()
                    {
                        InterviewId = interview.Id,
                        AttachmentId = attachment.Id
                    };

                    attachmentInInterviews.Add(attachmentInInterview);
                }

                await _attachmentInInterviewRepository.InsertAsync(attachmentInInterviews);
            }

            //create email queue
            // get job email template
            var interviewEmailTemplate = await _emailTemplateRepository.GetAll().Include(x => x.TemplateAttachments).FirstOrDefaultAsync(x => x.Type == EmailTemplateType.Interview);

            //// parse data
            var userFrom = await _userRepository.GetByIdAsync(currentUserId);
            interviewEmailTemplate.From = userFrom.Email;
            interviewEmailTemplate.FromName = userFrom.Name;

            //attachments
            List<string> listAttachment = new List<string>();

            foreach (var attachment in interviewEmailTemplate.TemplateAttachments)
            {
                var attachmentQueue = new AttachmentModel(attachment);
                var attachmentString = JsonConvert.SerializeObject(attachmentQueue, Formatting.Indented);
                listAttachment.Add(attachmentString);
            }

            foreach (var attachment in attachments)
            {
                var attachmentQueue = new AttachmentModel(attachment);
                var attachmentString = JsonConvert.SerializeObject(attachmentQueue, Formatting.Indented);
                listAttachment.Add(attachmentString);
            }

            var attachmentEmailQueue = String.Join(EmailQueueConstants.UNIT_ATTACHMENT_SPLIT, listAttachment);

            //insert EmailQueue Candidate
            var candidate = await _candidateRepository.GetByIdAsync(interview.CandidateId);
            var model = new ModelEmailInterview();
            model.ToName = candidate.Name;
            model.FromName = userFrom.Name;
            model.JobName = "Interview";
            EmailTemplateResponseModel emailTemplateResponseModelCandidate = await EmailTemplateHelper.ParseModelAsync<ModelEmailInterview>(interviewEmailTemplate, model);

            EmailQueue emailQueue = new EmailQueue(emailTemplateResponseModelCandidate);
            emailQueue.To = candidate.Email;
            emailQueue.ToName = candidate.Name;
            emailQueue.Attachments = attachmentEmailQueue;
            await _emailQueueRepository.InsertAsync(emailQueue);

            //insert EmailQueue interviewer
            var listToUser = new List<User>();

            foreach (var userInCalendar in calendar.UserInCalendars)
            {
                var user = await _userRepository.GetByIdAsync(userInCalendar.UserId);
                listToUser.Add(user);
            }

            foreach (var user in listToUser)
            {
                var modelInterviewer = new ModelEmailInterview();
                modelInterviewer.ToName = user.Name;
                modelInterviewer.FromName = userFrom.Name;
                modelInterviewer.JobName = "Interview";
                EmailTemplateResponseModel emailTemplateResponseModelInterviewer = await EmailTemplateHelper.ParseModelAsync<ModelEmailInterview>(interviewEmailTemplate, modelInterviewer);

                EmailQueue emailQueueInterviewer = new EmailQueue(emailTemplateResponseModelInterviewer);
                emailQueueInterviewer.To = user.Email;
                emailQueueInterviewer.ToName = user.Name;
                emailQueueInterviewer.Attachments = attachmentEmailQueue;
                await _emailQueueRepository.InsertAsync(emailQueueInterviewer);
            }

            interview = await GetAll().FirstOrDefaultAsync(x => x.Id == interview.Id);
            return new ResponseModel()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = new InterviewViewModel(interview),
                Message = MessageConstants.CREATED_SUCCESSFULLY
            };
        }

        public async Task<ResponseModel> UpdateInterviewAsync(Guid id, InterviewManageModel interviewManageModel)
        {
            var interview = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (interview == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
            else if (interview.Status != InterviewEnums.Status.Pending)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = InterviewMessagesConstants.INTERVIEW_NOT_PENDING
                };
            }
            else
            {
                var calendar = interview.Calendar;
                calendar.DateEnd = interviewManageModel.DateEnd;
                calendar.DateStart = interviewManageModel.DateStart;
                calendar.Description = interviewManageModel.Description;

                var userInCalendars = new List<UserInCalendar>();
                var hr = await _userInRoleRepository.FetchFirstAsync(x => x.RoleId == RoleConstants.HRMId);
                Guid hrId = hr.UserId;
                var bod = await _userInRoleRepository.FetchFirstAsync(x => x.RoleId == RoleConstants.BODId);
                Guid bodId = bod.UserId;
                foreach (var userId in interviewManageModel.InterviewerIds.Concat(new Guid[] { hrId, bodId }))
                {
                    var userInCalendar = new UserInCalendar()
                    {
                        CalendarId = calendar.Id,
                        UserId = userId,
                    };
                    userInCalendars.Add(userInCalendar);
                }
                interview.Calendar.UserInCalendars = userInCalendars;

                // Update InterviewInAttachments
                await _attachmentInInterviewRepository.DeleteAsync(interview.AttachmentInInterviews);
                if (interviewManageModel.Attachments != null && interviewManageModel.Attachments.Length > 0)
                {
                    // Update Attachment
                    var attachments = new List<Attachment>();
                    foreach (var attachment in interviewManageModel.Attachments)
                    {
                        var att = new Attachment();
                        attachment.SetDataToModel(att);
                        attachments.Add(att);
                    }

                    await _attachmentRepository.InsertAsync(attachments);

                    // Update AttachmentInAttachment
                    var attachmentInInterviews = new List<AttachmentInInterview>();
                    foreach (var attachment in attachments)
                    {
                        var attachmentInInterview = new AttachmentInInterview()
                        {
                            InterviewId = interview.Id,
                            AttachmentId = attachment.Id
                        };

                        attachmentInInterviews.Add(attachmentInInterview);
                    }

                    await _attachmentInInterviewRepository.InsertAsync(attachmentInInterviews);
                }


                interview.CandidateId = interviewManageModel.CandidateId;
                await _interviewRepository.UpdateAsync(interview);

                interview = await GetAll().FirstOrDefaultAsync(x => x.Id == interview.Id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new InterviewViewModel(interview),
                    Message = MessageConstants.UPDATED_SUCCESSFULLY
                };
            }
        }

        public async Task<ResponseModel> UpdateInterviewStatus(Guid id, InterviewStatusManageModel interviewStatusManageModel)
        {
            var interview = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (interview == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
            else if (interview.Status != InterviewEnums.Status.Waiting && interview.Status != InterviewEnums.Status.Pending)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = InterviewMessagesConstants.INVALID_INTERVIEW_STATUS
                };
            }
            else
            {
                interviewStatusManageModel.SetDataToModel(interview);
                await _interviewRepository.UpdateAsync(interview);

                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new InterviewViewModel(interview),
                    Message = MessageConstants.UPDATED_SUCCESSFULLY
                };
            }
        }

        public async Task<ResponseModel> DeleteInterviewAsync(Guid id)
        {
            var interview = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            var attachments = interview.AttachmentInInterviews.Select(x => x.Attachment).ToList();

            if (interview == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
            else
            {
                await _attachmentInInterviewRepository.DeleteAsync(interview.AttachmentInInterviews);
                await _interviewRepository.DeleteAsync(interview);
                await _attachmentRepository.DeleteAsync(attachments);

                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
        }

        public async Task<List<InterviewStatModel>> GetInterviewStatAnimationAsync(InterviewRequestStat interviewRequestStat)
        {
            List<InterviewStatModel> interviewStatModels = new List<InterviewStatModel>();
            var interviewDateAlls = _interviewRepository.GetAll().Include(x => x.Calendar).Where(x => !x.RecordDeleted)
                .Where(x => !(x.Calendar.DateEnd < interviewRequestStat.DateStart && interviewRequestStat.DateStart.HasValue
                    || x.Calendar.DateStart > interviewRequestStat.DateEnd && interviewRequestStat.DateEnd.HasValue));

            var totalInterview = await interviewDateAlls.CountAsync();
            if (totalInterview != 0)
            {
                var interviewStatPending = await interviewDateAlls.Where(x => x.Status.Equals(InterviewEnums.Status.Pending)).CountAsync();
                var interviewStatModelPending = new InterviewStatModel()
                {
                    Label = "PENDING INTERVIEW",
                    Y = Math.Round(((float)interviewStatPending / (float)totalInterview) * 100, 2)
                };
                interviewStatModels.Add(interviewStatModelPending);

                var interviewStatWaiting = await interviewDateAlls.Where(x => x.Status.Equals(InterviewEnums.Status.Waiting)).CountAsync();
                var interviewStatModelWaiting = new InterviewStatModel()
                {
                    Label = "WAITING INTERVIEW",
                    Y = Math.Round(((float)interviewStatWaiting / (float)totalInterview) * 100, 2)
                };
                interviewStatModels.Add(interviewStatModelWaiting);

                var interviewStatPassed = await interviewDateAlls.Where(x => x.Status.Equals(InterviewEnums.Status.Passed)).CountAsync();
                var interviewStatModelPassed = new InterviewStatModel()
                {
                    Label = "PASSED INTERVIEW",
                    Y = Math.Round(((float)interviewStatPassed / (float)totalInterview) * 100, 2)
                };
                interviewStatModels.Add(interviewStatModelPassed);

                var interviewStatFailed = await interviewDateAlls.Where(x => x.Status.Equals(InterviewEnums.Status.Failed)).CountAsync();
                var interviewStatModelFailed = new InterviewStatModel()
                {
                    Label = "FAILED INTERVIEW",
                    Y = Math.Round(((float)interviewStatFailed / (float)totalInterview) * 100, 2)
                };
                interviewStatModels.Add(interviewStatModelFailed);
            }

            return interviewStatModels;
        }

        public async Task<List<InterviewStatModel>> GetInterviewStatAsync()
        {
            List<InterviewStatModel> interviewStatModels = new List<InterviewStatModel>();
            var interviewDateAlls = _interviewRepository.GetAll().Include(x => x.Calendar).Where(x => !x.RecordDeleted);

            var interviewStatPending = await interviewDateAlls.Where(x => x.Status.Equals(InterviewEnums.Status.Pending)).CountAsync();
            var interviewStatModelPending = new InterviewStatModel()
            {
                Label = "PENDING INTERVIEW",
                Y = interviewStatPending
            };
            interviewStatModels.Add(interviewStatModelPending);

            var interviewStatWaiting = await interviewDateAlls.Where(x => x.Status.Equals(InterviewEnums.Status.Waiting)).CountAsync();
            var interviewStatModelWaiting = new InterviewStatModel()
            {
                Label = "WAITING INTERVIEW",
                Y = interviewStatWaiting
            };
            interviewStatModels.Add(interviewStatModelWaiting);

            var interviewStatPassed = await interviewDateAlls.Where(x => x.Status.Equals(InterviewEnums.Status.Passed)).CountAsync();
            var interviewStatModelPassed = new InterviewStatModel()
            {
                Label = "PASSED INTERVIEW",
                Y = interviewStatPassed
            };
            interviewStatModels.Add(interviewStatModelPassed);

            var interviewStatFailed = await interviewDateAlls.Where(x => x.Status.Equals(InterviewEnums.Status.Failed)).CountAsync();
            var interviewStatModelFailed = new InterviewStatModel()
            {
                Label = "FAILED INTERVIEW",
                Y = interviewStatFailed
            };
            interviewStatModels.Add(interviewStatModelFailed);

            return interviewStatModels;
        }

        #endregion
    }
}
