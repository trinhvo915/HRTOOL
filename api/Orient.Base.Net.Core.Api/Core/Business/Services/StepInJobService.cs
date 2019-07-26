using Microsoft.EntityFrameworkCore;
using Orient.Base.Net.Core.Api.Core.Business.Models.Base;
using Orient.Base.Net.Core.Api.Core.Business.Models.StepInJobs;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
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
    public interface IStepInJobService
    {
        Task<PagedList<StepInJobViewModel>> GetStepInJobAsync(Guid jobId, StepInJobRequestListViewModel stepInJobRequestListViewModel);

        Task<ResponseModel> CreateStepInJobsAsync(Guid jobId, StepInJobManageModel[] stepInJobManageModel);

        Task<ResponseModel> CreateStepInJobAsync(Guid jobId, StepInJobManageModel stepInJobManageModel);

        Task<ResponseModel> UpdateStepInJobAsync(Guid id, UpdateStepInJobManageModel updateStepInJobManageModel);

        Task<ResponseModel> DeleteStepInJobAsync(Guid id);
    }

    public class StepInJobService : IStepInJobService
    {
        #region Fields

        private readonly IRepository<StepInJob> _stepInJobRepository;

        private readonly IRepository<Job> _jobRepository;

        #endregion

        #region Constructor

        public StepInJobService(IRepository<StepInJob> stepInJobRepository, IRepository<Job> jobRepository)
        {
            _stepInJobRepository = stepInJobRepository;
            _jobRepository = jobRepository;
        }

        #endregion

        #region Base Methods

        private IQueryable<StepInJob> GetAll()
        {
            return _stepInJobRepository.GetAll();
        }

        private List<string> GetAllPropertyNameOfStepInJobViewModel()
        {
            var stepInJobViewModel = new StepInJobViewModel();

            var type = stepInJobViewModel.GetType();

            return ReflectionUtilities.GetAllPropertyNamesOfType(type);
        }

        #endregion

        public async Task<PagedList<StepInJobViewModel>> GetStepInJobAsync(Guid jobId, StepInJobRequestListViewModel stepInJobRequestListViewModel)
        {
            var list = await GetAll()
                        .Where(x => (!stepInJobRequestListViewModel.IsActive.HasValue || x.RecordActive == stepInJobRequestListViewModel.IsActive)
                        && (x.JobId == jobId)
                        && (string.IsNullOrEmpty(stepInJobRequestListViewModel.Query) || (x.Name.Contains(stepInJobRequestListViewModel.Query)))
                        ).Select(x => new StepInJobViewModel(x)).ToListAsync();

            var stepInJobViewModelProperties = GetAllPropertyNameOfStepInJobViewModel();

            var requestPropertyName = !string.IsNullOrEmpty(stepInJobRequestListViewModel.SortName) ? stepInJobRequestListViewModel.SortName.ToLower() : string.Empty;

            var matchedPropertyName = stepInJobViewModelProperties.FirstOrDefault(x => x.ToLower() == requestPropertyName);

            if (string.IsNullOrEmpty(matchedPropertyName))
            {
                matchedPropertyName = "Order";
            }

            var type = typeof(StepInJobViewModel);

            var sortProperty = type.GetProperty(matchedPropertyName);

            list = stepInJobRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();

            return new PagedList<StepInJobViewModel>(list, stepInJobRequestListViewModel.Skip ?? CommonConstants.Config.DEFAULT_SKIP, stepInJobRequestListViewModel.Take ?? CommonConstants.Config.DEFAULT_TAKE);
        }

        public async Task<ResponseModel> CreateStepInJobsAsync(Guid jobId, StepInJobManageModel[] stepInJobManageModel)
        {
            var order = await GetAll().OrderBy(x => x.RecordOrder).Select(x => x.RecordOrder).LastOrDefaultAsync();
            var stepInJobs = new List<StepInJob>();
            foreach(var stepInJob in stepInJobManageModel)
            {
                var step = AutoMapper.Mapper.Map<StepInJob>(stepInJob);
                step.JobId = jobId;
                step.Status = StatusEnums.Status.Pending;
                step.RecordOrder = ++order;
                stepInJobs.Add(step);
            }
            
            await _stepInJobRepository.InsertAsync(stepInJobs);

            return new ResponseModel()
            {
                Data = stepInJobs.Select(x => new StepInJobViewModel(x)).ToArray(),
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = MessageConstants.CREATED_SUCCESSFULLY
            };
        }

        public async Task UpdateStatusJob(Guid jobId)
        {
            var job = await _jobRepository.GetAll().Include(x => x.StepInJobs).FirstOrDefaultAsync(x => x.Id == jobId);
            
            var statusJob = job.StepInJobs.All(x => x.Status.ToString().Equals("Done"));
            if (statusJob)
            {
                job.Status = StatusEnums.Status.Done;
            }
            else
            {
                statusJob = job.StepInJobs.Any(x => x.Status.ToString().Equals("Doing"));
                if (statusJob)
                {
                    job.Status = StatusEnums.Status.Doing;
                }
                else
                {
                    job.Status = StatusEnums.Status.Pending;
                }
            }
            await _jobRepository.UpdateAsync(job);
        }

        public async Task<ResponseModel> CreateStepInJobAsync(Guid jobId, StepInJobManageModel stepInJobManageModel)
        {
            var existedStepInJob = await GetAll().FirstOrDefaultAsync(x => x.Name == stepInJobManageModel.Name && x.JobId == jobId);
            if(existedStepInJob != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = MessageConstants.EXISTED_CREATED
                };
            }
            else
            {
                var order = await GetAll().OrderBy(x => x.RecordOrder).Select(x => x.RecordOrder).LastOrDefaultAsync();
                var stepInJob = AutoMapper.Mapper.Map<StepInJob>(stepInJobManageModel);
                stepInJob.JobId = jobId;
                stepInJob.Status = StatusEnums.Status.Pending;
                stepInJob.RecordOrder = order + 1;

                await _stepInJobRepository.InsertAsync(stepInJob);
                await UpdateStatusJob(jobId);

                return new ResponseModel()
                {
                    Data = new StepInJobViewModel(stepInJob),
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.CREATED_SUCCESSFULLY
                };
            }
        }

        public async Task<ResponseModel> UpdateStepInJobAsync(Guid id, UpdateStepInJobManageModel updateStepInJobManageModel)
        {
            var stepInJob = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (stepInJob == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = MessageConstants.NOT_FOUND
                };
            }
            else
            {
                var existedStepInJob = await GetAll().FirstOrDefaultAsync(x => x.Id != id && x.Name == updateStepInJobManageModel.Name && x.JobId == updateStepInJobManageModel.JobId);
                if(existedStepInJob != null)
                {
                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = MessageConstants.EXISTED_CREATED
                    };
                }
                else
                {
                    updateStepInJobManageModel.SetData(stepInJob);
                    await _stepInJobRepository.UpdateAsync(stepInJob);
                    await UpdateStatusJob(stepInJob.JobId);

                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Data = new StepInJobViewModel(stepInJob),
                        Message = MessageConstants.UPDATED_SUCCESSFULLY
                    };
                }
            }
        }

        public async Task<ResponseModel> DeleteStepInJobAsync(Guid id)
        {
            var stepInJob = await _stepInJobRepository.FetchFirstAsync(x => x.Id == id);
            if(stepInJob != null)
            {
                var jobId = stepInJob.JobId;
                await _stepInJobRepository.DeleteAsync(stepInJob);
                await UpdateStatusJob(jobId);

                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
            else
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
        }
    }
}
