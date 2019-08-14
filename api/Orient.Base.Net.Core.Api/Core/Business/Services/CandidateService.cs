using Microsoft.EntityFrameworkCore;
using Orient.Base.Net.Core.Api.Core.Business.Models.Base;
using Orient.Base.Net.Core.Api.Core.Business.Models.Candidates;
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
	public interface ICandidateService
	{
		Task<PagedList<CandidateViewModel>> ListCandidateAsync(CandidateRequestListViewModel candidateRequestListViewModel);

		Task<IEnumerable<CandidateViewModel>> GetAllCandidateAsync(BaseRequestGetAllViewModel baseRequestGetAllViewModel);

		Task<ResponseModel> CreateCandidateAsync(CandidateManageModel candidateManageModel);

		Task<ResponseModel> UpdateCandidateAsync(Guid id, CandidateManageModel candidateManageModel);

		Task<ResponseModel> GetCandidateByIdAsync(Guid? id);

		Task<ResponseModel> DeteteCandidateAsync(Guid id);

		Task<List<CandidateViewExcelModel>> GetCandidateViewExcel(BaseRequestGetAllViewModel baseRequestGetAllViewModel);
	}

	public class CandidateService : ICandidateService
	{
		#region Fields

		private readonly IRepository<Candidate> _candidateReponsitory;
		private readonly IRepository<TechnicalSkillInCandidate> _technicalSkillInCandidateReponsitory;

		#endregion

		#region Constructor

		public CandidateService(IRepository<Candidate> candidateReponsitory, IRepository<TechnicalSkillInCandidate> technicalSkillInCandidateReponsitory)
		{
			_candidateReponsitory = candidateReponsitory;
			_technicalSkillInCandidateReponsitory = technicalSkillInCandidateReponsitory;
		}

		#endregion

		#region Private Methods

		private IQueryable<Candidate> GetAll()
		{
			return _candidateReponsitory.GetAll()
					.Include(x => x.Interviews)
					.Include(y => y.TechnicalSkillInCandidates)
						.ThenInclude(y => y.TechnicalSkill)
					.Where(x => !x.RecordDeleted);
		}

		private List<string> GetAllPropertyNameOfCandidateViewModel()
		{
			var candidateViewModel = new CandidateViewModel();

			var type = candidateViewModel.GetType();

			return ReflectionUtilities.GetAllPropertyNamesOfType(type);
		}

		#endregion

		public async Task<PagedList<CandidateViewModel>> ListCandidateAsync(CandidateRequestListViewModel candidateRequestListViewModel)
		{
			var list = await GetAll()
				.Where(x => (!candidateRequestListViewModel.IsActive.HasValue || x.RecordActive == candidateRequestListViewModel.IsActive)
					&& (string.IsNullOrEmpty(candidateRequestListViewModel.Query)
					|| (x.Name.Contains(candidateRequestListViewModel.Query))
					|| (x.Email.Contains(candidateRequestListViewModel.Query))
					|| (x.Mobile.Contains(candidateRequestListViewModel.Query))
					|| (x.Address.Contains(candidateRequestListViewModel.Query))
				))
			.Select(x => new CandidateViewModel(x)).ToListAsync();

			var candidateViewModelProperties = GetAllPropertyNameOfCandidateViewModel();

			var requestPropertyName = !string.IsNullOrEmpty(candidateRequestListViewModel.SortName) ? candidateRequestListViewModel.SortName.ToLower() : string.Empty;

			string matchedPropertyName = candidateViewModelProperties.FirstOrDefault(x => x.ToLower() == requestPropertyName);

			if (string.IsNullOrEmpty(matchedPropertyName))
			{
				matchedPropertyName = "Name";
			}

			var type = typeof(CandidateViewModel);

			var sortProperty = type.GetProperty(matchedPropertyName);

			list = candidateRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();

			return new PagedList<CandidateViewModel>(list, candidateRequestListViewModel.Skip ?? CommonConstants.Config.DEFAULT_SKIP, candidateRequestListViewModel.Take ?? CommonConstants.Config.DEFAULT_TAKE);
		}

		public async Task<IEnumerable<CandidateViewModel>> GetAllCandidateAsync(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
		{
			var list = await GetAll()
				.Where(x => (string.IsNullOrEmpty(baseRequestGetAllViewModel.Query)
					|| (x.Name.Contains(baseRequestGetAllViewModel.Query))
					|| (x.Email.Contains(baseRequestGetAllViewModel.Query)))
				&& (x.Interviews.Count == 0 || x.Interviews.All(y => y.Status == InterviewEnums.Status.Failed)))
				.OrderBy(x => x.Name)
				.Select(x => new CandidateViewModel(x))
				.ToListAsync();

			return list;
		}

		public async Task<ResponseModel> GetCandidateByIdAsync(Guid? id)
		{
			var candidate = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
			if (candidate != null)
			{
				return new ResponseModel()
				{
					StatusCode = System.Net.HttpStatusCode.OK,
					Data = new CandidateViewModel(candidate)
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

		public async Task<ResponseModel> CreateCandidateAsync(CandidateManageModel candidateManageModel)
		{
			var candidate = await _candidateReponsitory.FetchFirstAsync(x => x.Email == candidateManageModel.Email);

			if (candidate != null)
			{
				return new ResponseModel()
				{
					StatusCode = System.Net.HttpStatusCode.BadRequest,
					Message = MessageConstants.EXISTED_CREATED
				};
			}
			else
			{
				//create Candidate
				candidate = AutoMapper.Mapper.Map<Candidate>(candidateManageModel);

				await _candidateReponsitory.InsertAsync(candidate);

				candidate = await GetAll().FirstOrDefaultAsync(x => x.Id == candidate.Id);

				//create TechnicalSkillInCandidate
				var technicalSkillInCandidates = new List<TechnicalSkillInCandidate>();
				if (candidateManageModel.TechnicalSkillIds != null)
				{
					foreach (var technicalSkillId in candidateManageModel.TechnicalSkillIds)
					{
						var technicalSkillInCandidate = new TechnicalSkillInCandidate();
						technicalSkillInCandidate.CandidateId = candidate.Id;
						technicalSkillInCandidate.TechnicalSkillId = technicalSkillId;
						technicalSkillInCandidates.Add(technicalSkillInCandidate);
					}
				}
				await _technicalSkillInCandidateReponsitory.InsertAsync(technicalSkillInCandidates);

				return new ResponseModel()
				{
					StatusCode = System.Net.HttpStatusCode.OK,
					Data = new CandidateViewModel(candidate),
					Message = MessageConstants.CREATED_SUCCESSFULLY
				};
			}


		}

		public async Task<ResponseModel> UpdateCandidateAsync(Guid id, CandidateManageModel candidateManageModel)
		{
			var candidate = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
			if (candidate == null)
			{
				return new ResponseModel()
				{
					StatusCode = System.Net.HttpStatusCode.NotFound,
					Message = MessageConstants.NOT_FOUND
				};
			}
			else
			{
				var exitstedCandidate = await _candidateReponsitory.FetchFirstAsync(x => x.Email == candidateManageModel.Email && x.Id != id);

				if (exitstedCandidate != null)
				{
					return new ResponseModel()
					{
						StatusCode = System.Net.HttpStatusCode.BadRequest,
						Message = MessageConstants.EXISTED_CREATED
					};
				}
				else
				{
					//update candidate
					candidateManageModel.SetDataToModel(candidate);

					await _candidateReponsitory.UpdateAsync(candidate);

					// update technicalSkillInCandidate
					await _technicalSkillInCandidateReponsitory.DeleteAsync(candidate.TechnicalSkillInCandidates);
					var technicalSkillInCandidates = new List<TechnicalSkillInCandidate>();
					if (candidateManageModel.TechnicalSkillIds != null)
					{
						foreach (var technicalSkillId in candidateManageModel.TechnicalSkillIds)
						{
							var technicalSkillInCandidate = new TechnicalSkillInCandidate();
							technicalSkillInCandidate.CandidateId = candidate.Id;
							technicalSkillInCandidate.TechnicalSkillId = technicalSkillId;
							technicalSkillInCandidates.Add(technicalSkillInCandidate);
						}
					}
					await _technicalSkillInCandidateReponsitory.InsertAsync(technicalSkillInCandidates);

					return new ResponseModel()
					{
						StatusCode = System.Net.HttpStatusCode.OK,
						Data = new CandidateViewModel(candidate),
						Message = MessageConstants.UPDATED_SUCCESSFULLY
					};
				}
			}
		}

		public async Task<ResponseModel> DeteteCandidateAsync(Guid id)
		{
			var candidate = await _candidateReponsitory.GetByIdAsync(id);
			if (candidate == null)
			{
				return new ResponseModel()
				{
					StatusCode = System.Net.HttpStatusCode.BadRequest,
					Message = MessageConstants.NOT_FOUND
				};
			}
			else
			{
				//delete candidate
				await _candidateReponsitory.DeleteAsync(id);

				// delete TechnicalSkillInCandidate
				await _technicalSkillInCandidateReponsitory.DeleteAsync(candidate.TechnicalSkillInCandidates);

				return new ResponseModel()
				{
					StatusCode = System.Net.HttpStatusCode.OK,
					Message = MessageConstants.DELETED_SUCCESSFULLY
				};
			}
		}
		public async Task<List<CandidateViewExcelModel>> GetCandidateViewExcel(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
		{
			var listCandidate = await GetAll()
			.Where(x => (string.IsNullOrEmpty(baseRequestGetAllViewModel.Query)
				|| (x.Name.Contains(baseRequestGetAllViewModel.Query))
				))
				.Select(x => new CandidateViewExcelModel(x)).ToListAsync();

			return listCandidate;
		}
	}
}
