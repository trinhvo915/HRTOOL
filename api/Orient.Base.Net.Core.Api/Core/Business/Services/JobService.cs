using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Orient.Base.Net.Core.Api.Core.Business.Models.Base;
using Orient.Base.Net.Core.Api.Core.Business.Models.EmailQueue;
using Orient.Base.Net.Core.Api.Core.Business.Models.EmailTemplates;
using Orient.Base.Net.Core.Api.Core.Business.Models.Jobs;
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
    public interface IJobService
    {
        Task<PagedList<JobViewModel>> ListJobAsync(JobRequestListViewModel jobRequestListViewModel);

        Task<ResponseModel> CreateJobAsync(Guid reporterId, JobManageModel jobManageModel);

        Task<ResponseModel> UpdateJobAsync(Guid id, UpdateJobManageModel updateJobManageModel);

        Task<ResponseModel> GetJobByIdAsync(Guid? id);

        Task<PagedList<JobViewModel>> GetListJobByUserIdAsync(Guid? id, JobRequestListViewModel jobRequestListViewModel);

        Task<ResponseModel> DeleteJobAsync(Guid id);

        Task<List<JobViewExcelModel>> GetJobViewExcel(BaseRequestGetAllViewModel baseRequestGetAllViewModel);

        Task<List<JobStatModel>> GetJobStatAnimationAsync(JobRequestStat jobRequestStat);

        Task<List<JobStatModel>> GetJobStatAsync();
    }

    public class JobService : IJobService
    {
        #region Fields

        private readonly IRepository<Job> _jobResponstory;

        private readonly IRepository<UserInJob> _userInJobResponstory;

        private readonly IRepository<JobInCategory> _jobInCategoryResponstory;

        private readonly IRepository<Attachment> _attachmentRepository;

        private readonly IRepository<AttachmentInJob> _attachmentInJobRepository;

        private readonly IRepository<Comment> _commentRepository;

        private readonly IRepository<EmailTemplate> _emailTemplateRepository;

        private readonly IRepository<EmailQueue> _emailQueueRepository;

        private readonly IRepository<User> _userRepository;

        private readonly IRepository<StepInJob> _stepInJobRepository;

        #endregion

        #region Constructor

        public JobService(IRepository<Job> jobRepository, IRepository<UserInJob> userInJobResponstory, IRepository<JobInCategory> joInCategoryResponstory, IRepository<Attachment> attachmentRepository, IRepository<AttachmentInJob> attachmentInJobRepository, IRepository<EmailTemplate> emailTemplateRepository, IRepository<EmailQueue> emailQueueRepository, IRepository<User> userRepository, IRepository<Comment> commentRepository, IRepository<StepInJob> stepInJobRepository)
        {
            _jobResponstory = jobRepository;
            _userInJobResponstory = userInJobResponstory;
            _jobInCategoryResponstory = joInCategoryResponstory;
            _attachmentRepository = attachmentRepository;
            _attachmentInJobRepository = attachmentInJobRepository;
            _emailTemplateRepository = emailTemplateRepository;
            _emailQueueRepository = emailQueueRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _stepInJobRepository = stepInJobRepository;
        }

        #endregion

        #region Private Methods

        private IQueryable<Job> GetAll()
        {
            return _jobResponstory.GetAll()
                    .Include(x => x.Reporter)
                    .Include(x => x.UserInJobs)
                        .ThenInclude(UserInJobs => UserInJobs.User)
                    .Include(x => x.Comments)
                        .ThenInclude(comment => comment.User)
                    .Include(x => x.JobInCategories)
                        .ThenInclude(JobInCategories => JobInCategories.Category)
                    .Include(x => x.AttachmentInJobs)
                        .ThenInclude(AttachmentInJobs => AttachmentInJobs.Attachment)
                    .Include(x => x.StepInJobs)
                    .Where(x => !x.RecordDeleted);
        }

        private List<string> GetAllPropertyNameOfJobViewModel()
        {
            var jobViewModel = new JobViewModel();

            var type = jobViewModel.GetType();

            return ReflectionUtilities.GetAllPropertyNamesOfType(type);
        }

        #endregion

        #region Base Methods

        public async Task<PagedList<JobViewModel>> ListJobAsync(JobRequestListViewModel jobRequestListViewModel)
        {
            var list = await GetAll()
                .Where(x => (!jobRequestListViewModel.IsActive.HasValue || x.RecordActive == jobRequestListViewModel.IsActive)
                && (string.IsNullOrEmpty(jobRequestListViewModel.Query)
                || (x.Name.Contains(jobRequestListViewModel.Query)
                ))
                )
                .Select(x => new JobViewModel(x)).ToListAsync();

            var jobViewModelProperties = GetAllPropertyNameOfJobViewModel();

            var requestPropertyName = !string.IsNullOrEmpty(jobRequestListViewModel.SortName) ? jobRequestListViewModel.SortName.ToLower() : string.Empty;

            var matchedPropertyName = jobViewModelProperties.FirstOrDefault(x => x.ToLower() == requestPropertyName);

            if (string.IsNullOrEmpty(matchedPropertyName))
            {
                matchedPropertyName = "Name";
            }

            var type = typeof(JobViewModel);

            var sortProperty = type.GetProperty(matchedPropertyName);

            list = jobRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();

            return new PagedList<JobViewModel>(list, jobRequestListViewModel.Skip ?? CommonConstants.Config.DEFAULT_SKIP, jobRequestListViewModel.Take ?? CommonConstants.Config.DEFAULT_TAKE);
        }

        public async Task<PagedList<JobViewModel>> GetListJobByUserIdAsync(Guid? userId, JobRequestListViewModel jobRequestListViewModel)
        {
            var list = await GetAll()
                 .Where(x => (!jobRequestListViewModel.IsActive.HasValue || x.RecordActive == jobRequestListViewModel.IsActive)
                && (string.IsNullOrEmpty(jobRequestListViewModel.Query)
                    || (x.Name.Contains(jobRequestListViewModel.Query)))
                && (x.UserInJobs.Any(y => y.UserId == userId))
                )
                .Select(x => new JobViewModel(x)).ToListAsync();

            var jobViewModelProperties = GetAllPropertyNameOfJobViewModel();

            var requestPropertyName = !string.IsNullOrEmpty(jobRequestListViewModel.SortName) ? jobRequestListViewModel.SortName.ToLower() : string.Empty;

            string matchedPropertyName = jobViewModelProperties.FirstOrDefault(x => x.ToLower() == requestPropertyName);

            if (string.IsNullOrEmpty(matchedPropertyName))
            {
                matchedPropertyName = "Name";
            }

            var type = typeof(JobViewModel);

            var sortProperty = type.GetProperty(matchedPropertyName);

            list = jobRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();

            return new PagedList<JobViewModel>(list, jobRequestListViewModel.Skip ?? CommonConstants.Config.DEFAULT_SKIP, jobRequestListViewModel.Take ?? CommonConstants.Config.DEFAULT_TAKE);
        }

        public async Task<ResponseModel> GetJobByIdAsync(Guid? id)
        {
            var job = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (job != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new JobViewModel(job),
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

        public async Task<ResponseModel> CreateJobAsync(Guid reporterId, JobManageModel jobManageModel)
        {
            if (reporterId == null)
            {
                return new ResponseModel()
                {
                    Message = MessageConstants.INVALID_ACCESS_TOKEN,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            var job = await _jobResponstory.FetchFirstAsync(x => x.Name == jobManageModel.Name && x.DateStart.Value.Date == jobManageModel.DateStart.Date);
            if (job != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = MessageConstants.EXISTED_CREATED
                };
            }
            else
            {
                // Create Job
                job = AutoMapper.Mapper.Map<Job>(jobManageModel);
                job.ReporterId = reporterId;
                await _jobResponstory.InsertAsync(job);

                // Create UserInJob
                var userInJobs = new List<UserInJob>();
                foreach (var userId in jobManageModel.UserIds)
                {
                    var userInJob = new UserInJob()
                    {
                        JobId = job.Id,
                        UserId = userId
                    };

                    userInJobs.Add(userInJob);
                }

                await _userInJobResponstory.InsertAsync(userInJobs);

                // Create JobInCategory
                var jobInCategories = new List<JobInCategory>();
                foreach (var categoryId in jobManageModel.CategoryIds)
                {
                    var jobInCategory = new JobInCategory()
                    {
                        CategoryId = categoryId,
                        JobId = job.Id
                    };

                    jobInCategories.Add(jobInCategory);
                }

                await _jobInCategoryResponstory.InsertAsync(jobInCategories);

                var attachments = new List<Attachment>();
                if (jobManageModel.Attachments != null && jobManageModel.Attachments.Length > 0)
                {
                    // Create Attachment
                    foreach (var attachment in jobManageModel.Attachments)
                    {
                        var att = new Attachment();
                        attachment.SetDataToModel(att);
                        attachments.Add(att);
                    }

                    await _attachmentRepository.InsertAsync(attachments);

                    // Create AttachmentInJob
                    var attachmentInJobs = new List<AttachmentInJob>();
                    foreach (var attachment in attachments)
                    {
                        var attachmentInJob = new AttachmentInJob()
                        {
                            JobId = job.Id,
                            AttachmentId = attachment.Id
                        };

                        attachmentInJobs.Add(attachmentInJob);
                    }

                    await _attachmentInJobRepository.InsertAsync(attachmentInJobs);
                }

                if (jobManageModel.Steps != null && jobManageModel.Steps.Length > 0)
                {
                    // Create StepInJobs
                    var stepInJobs = new List<StepInJob>();
                    foreach (var step in jobManageModel.Steps.Select((value, idx) => new { idx, value }))
                    {
                        var stepInJob = new StepInJob()
                        {
                            Name = step.value.Name,
                            RecordOrder = step.idx + 1,
                            JobId = job.Id
                        };

                        stepInJobs.Add(stepInJob);
                    }

                    await _stepInJobRepository.InsertAsync(stepInJobs);
                }

                job = await GetAll().FirstOrDefaultAsync(x => x.Id == job.Id);

                //create email queue
                // get job email template
                var jobEmailTemplate = await _emailTemplateRepository.GetAll().Include(x => x.TemplateAttachments).FirstOrDefaultAsync(x => x.Type == EmailTemplateType.Job);

                var userFrom = await _userRepository.GetByIdAsync(job.ReporterId);
                jobEmailTemplate.From = userFrom.Email;
                jobEmailTemplate.FromName = userFrom.Name;

                List<string> listAttachment = new List<string>();
                foreach (var attachment in jobEmailTemplate.TemplateAttachments)
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

                var listToUser = new List<User>();
                foreach (var userInJob in job.UserInJobs)
                {
                    var user = await _userRepository.GetByIdAsync(userInJob.UserId);
                    listToUser.Add(user);
                }
                foreach (var user in listToUser)
                {
                    var model = new ModelEmailJob();
                    model.ToName = user.Name;
                    model.FromName = userFrom.Name;
                    model.JobName = job.Name;
                    EmailTemplateResponseModel emailTemplateResponseModel = await EmailTemplateHelper.ParseModelAsync<ModelEmailJob>(jobEmailTemplate, model);

                    if (listToUser.IndexOf(user) != 0)
                    {
                        emailTemplateResponseModel.CC = string.Empty;
                        emailTemplateResponseModel.BCC = string.Empty;
                    }

                    EmailQueue emailQueue = new EmailQueue(emailTemplateResponseModel);
                    emailQueue.To = user.Email;
                    emailQueue.ToName = user.Name;
                    emailQueue.Attachments = attachmentEmailQueue;
                    await _emailQueueRepository.InsertAsync(emailQueue);
                }

                job = await GetAll().FirstOrDefaultAsync(x => x.Id == job.Id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new JobViewModel(job),
                    Message = MessageConstants.CREATED_SUCCESSFULLY
                };
            }
        }

        public async Task<ResponseModel> UpdateJobAsync(Guid id, UpdateJobManageModel updateJobManageModel)
        {
            var job = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (job == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
            else
            {
                var existedJob = await _jobResponstory.FetchFirstAsync(x => x.Name == updateJobManageModel.Name && x.Id != id && x.DateStart.Value.Date == updateJobManageModel.DateStart.Date);
                if (existedJob != null)
                {
                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = MessageConstants.EXISTED_CREATED
                    };
                }
                else
                {
                    updateJobManageModel.SetDataToModel(job);

                    // Update UserInJob
                    await _userInJobResponstory.DeleteAsync(job.UserInJobs);

                    var userInJobs = new List<UserInJob>();
                    foreach (var userId in updateJobManageModel.UserIds)
                    {
                        var userInJob = new UserInJob()
                        {
                            JobId = job.Id,
                            UserId = userId
                        };

                        userInJobs.Add(userInJob);
                    }

                    await _userInJobResponstory.InsertAsync(userInJobs);

                    // Update JobInCategory
                    await _jobInCategoryResponstory.DeleteAsync(job.JobInCategories);

                    var jobInCategories = new List<JobInCategory>();
                    foreach (var categoryId in updateJobManageModel.CategoryIds)
                    {
                        var jobIncategory = new JobInCategory()
                        {
                            JobId = id,
                            CategoryId = categoryId
                        };

                        jobInCategories.Add(jobIncategory);
                    }

                    await _jobInCategoryResponstory.InsertAsync(jobInCategories);

                    await _attachmentInJobRepository.DeleteAsync(job.AttachmentInJobs);

                    if (updateJobManageModel.Attachments != null && updateJobManageModel.Attachments.Length > 0)
                    {
                        // Update Attachment
                        var attachments = new List<Attachment>();
                        foreach (var attachment in updateJobManageModel.Attachments)
                        {
                            var att = new Attachment();
                            attachment.SetDataToModel(att);
                            attachments.Add(att);
                        }

                        await _attachmentRepository.InsertAsync(attachments);

                        // Update AttachmentInJob
                        var attachmentInJobs = new List<AttachmentInJob>();
                        foreach (var attachment in attachments)
                        {
                            var attachmentInJob = new AttachmentInJob()
                            {
                                JobId = job.Id,
                                AttachmentId = attachment.Id
                            };

                            attachmentInJobs.Add(attachmentInJob);
                        }

                        await _attachmentInJobRepository.InsertAsync(attachmentInJobs);
                    }

                    await _jobResponstory.UpdateAsync(job);

                    // return client
                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Data = new JobViewModel(job),
                        Message = MessageConstants.UPDATED_SUCCESSFULLY
                    };
                }
            }
        }

        public async Task<ResponseModel> DeleteJobAsync(Guid id)
        {
            var job = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (job == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
            else
            {
                // Delete UserInJob
                await _userInJobResponstory.DeleteAsync(job.UserInJobs);

                // Delete JobInCategory
                await _jobInCategoryResponstory.DeleteAsync(job.JobInCategories);

                // Delete AttchmentInJob
                await _attachmentInJobRepository.DeleteAsync(job.AttachmentInJobs);

                // Delete Comment
                await _commentRepository.DeleteAsync(job.Comments);

                // Detele StepInJob
                await _stepInJobRepository.DeleteAsync(job.StepInJobs);

                // Detele Job and return client
                await _jobResponstory.DeleteAsync(id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
        }

        public async Task<List<JobViewExcelModel>> GetJobViewExcel(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
        {
            var listJob = await GetAll()
            .Where(x => (string.IsNullOrEmpty(baseRequestGetAllViewModel.Query)
                || (x.Name.Contains(baseRequestGetAllViewModel.Query))
                ))
                .Select(x => new JobViewExcelModel(x)).ToListAsync();

            return listJob;
        }

        public async Task<List<JobStatModel>> GetJobStatAsync()
        {
            List<JobStatModel> jobStatModels = new List<JobStatModel>();
            var jobDateAlls = _jobResponstory.GetAll();
            var countJobStatAll = await jobDateAlls.CountAsync();

            var jobStatModelAll = new JobStatModel()
            {
                Label = "ALL JOB",
                Y = countJobStatAll
            };
            jobStatModels.Add(jobStatModelAll);

            var countJobStatPending = await jobDateAlls.Where(x => x.Status.Equals(StatusEnums.Status.Pending)).CountAsync();
            var jobStatModelPending = new JobStatModel()
            {
                Label = "ALL PENDING",
                Y = countJobStatPending
            };
            jobStatModels.Add(jobStatModelPending);

            var countJobStatDoing = await jobDateAlls.Where(x => x.Status.Equals(StatusEnums.Status.Doing)).CountAsync();
            var jobStatModelDoing = new JobStatModel()
            {
                Label = "ALL DOING",
                Y = countJobStatDoing
            };
            jobStatModels.Add(jobStatModelDoing);

            var countJobStatDone = await jobDateAlls.Where(x => x.Status.Equals(StatusEnums.Status.Done)).CountAsync();
            var jobStatModelDone = new JobStatModel()
            {
                Label = "ALL DONE",
                Y = countJobStatDone
            };
            jobStatModels.Add(jobStatModelDone);

            return jobStatModels;
        }

        public async Task<List<JobStatModel>> GetJobStatAnimationAsync(JobRequestStat jobRequestStat)
        {
            List<JobStatModel> jobStatModels = new List<JobStatModel>();

            var jobDateAlls = _jobResponstory.GetAll()
                .Where(x => !(x.DateEnd < jobRequestStat.DateStart && jobRequestStat.DateStart.HasValue
                    || x.DateStart > jobRequestStat.DateEnd && jobRequestStat.DateEnd.HasValue));

            var totalJob = await jobDateAlls.CountAsync();

            if (totalJob != 0)
            {
                var countJobStatPending = await jobDateAlls.Where(x => x.Status.Equals(StatusEnums.Status.Pending)).CountAsync();
                var jobStatModelPending = new JobStatModel()
                {
                    Label = "ALL PENDING",
                    Y = Math.Round(((float)countJobStatPending / (float)totalJob) * 100, 2)
                };
                jobStatModels.Add(jobStatModelPending);

                var countJobStatDoing = await jobDateAlls.Where(x => x.Status.Equals(StatusEnums.Status.Doing)).CountAsync();
                var jobStatModelDoing = new JobStatModel()
                {
                    Label = "ALL DOING",
                    Y = Math.Round(((float)countJobStatDoing / (float)totalJob) * 100, 2)
                };
                jobStatModels.Add(jobStatModelDoing);

                var countJobStatDone = await jobDateAlls.Where(x => x.Status.Equals(StatusEnums.Status.Done)).CountAsync();
                var jobStatModelDone = new JobStatModel()
                {
                    Label = "ALL DONE",
                    Y = Math.Round(((float)countJobStatDone / (float)totalJob) * 100, 2)
                };
                jobStatModels.Add(jobStatModelDone);
            }

            return jobStatModels;
        }
        #endregion Public Methods
    }
}