using Microsoft.EntityFrameworkCore;
using Orient.Base.Net.Core.Api.Core.Business.Models.Base;
using Orient.Base.Net.Core.Api.Core.Business.Models.TechnicalSkills;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.Common.Reflections;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Services
{
	public interface ITechnicalSkillService
	{
		Task<PagedList<TechnicalSkillViewModel>> ListTechnicalSkillAsync(TechnicalSkillRequestListViewModel technicalSkillRequestListViewModel);

		Task<ResponseModel> GetTechnicalSkillAsync(Guid? id);

		Task<IEnumerable<TechnicalSkillViewModel>> GetAllTechnicalSkillAsync();

		Task<ResponseModel> CreateTechnicalSkillAsync(TechnicalSkillManageModel technicalSkillManageModel);

		Task<ResponseModel> UpdateTechnicalSkillAsync(Guid id, TechnicalSkillManageModel technicalSkillManageModel);

		Task<ResponseModel> DeleteTechnicalSkillAsync(Guid id);

		Task<IEnumerable<TechnicalSkillViewModel>> ListTechnicalSkillByIdUserAsync(Guid? id);

	}

	public class TechnicalSkillService : ITechnicalSkillService
	{
		#region Fields

		private readonly IRepository<TechnicalSkill> _technicalSkillRepository;
		private readonly IRepository<TechnicalSkillInCandidate> _technicalSkillInCandidateRepository;

		#endregion

		#region Constructor

		public TechnicalSkillService(IRepository<TechnicalSkill> technicalSkillRepository, IRepository<TechnicalSkillInCandidate> technicalSkillInCandidateRepository)
		{
			_technicalSkillRepository = technicalSkillRepository;
			_technicalSkillInCandidateRepository = technicalSkillInCandidateRepository;
		}

		#endregion

		private IQueryable<TechnicalSkill> GetAll()
		{
			return _technicalSkillRepository.GetAll()
				.Include(x => x.TechnicalSkillInCandidates)
					.ThenInclude(x => x.Candidate);

		}

		private List<string> GetAllPropertyNameOfTechnicalSkillViewModel()
		{
			var technicalSkill = new TechnicalSkill();
			var type = technicalSkill.GetType();
			return ReflectionUtilities.GetAllPropertyNamesOfType(type);
		}

		public async Task<PagedList<TechnicalSkillViewModel>> ListTechnicalSkillAsync(TechnicalSkillRequestListViewModel technicalSkillRequestListViewModel)
		{
			var list = await GetAll()
				.Where(x => (!technicalSkillRequestListViewModel.IsActive.HasValue || x.RecordActive == technicalSkillRequestListViewModel.IsActive)
					&& (string.IsNullOrEmpty(technicalSkillRequestListViewModel.Query)
					|| (x.Name.Contains(technicalSkillRequestListViewModel.Query))))
					.Select(x => new TechnicalSkillViewModel(x))
					.ToListAsync();

			var technicalViewModelProperties = GetAllPropertyNameOfTechnicalSkillViewModel();

			var requestPropertyName = !string.IsNullOrEmpty(technicalSkillRequestListViewModel.SortName) ? technicalSkillRequestListViewModel.SortName.ToLower() : string.Empty;

			string matchedPropertyName = technicalViewModelProperties.FirstOrDefault(x => x.ToLower() == requestPropertyName);

			if (string.IsNullOrEmpty(matchedPropertyName))
			{
				matchedPropertyName = "name";
			}

			if (matchedPropertyName == "name")
			{
				list = technicalSkillRequestListViewModel.IsDesc ? list.OrderByDescending(x => x.Name).ToList() : list.OrderBy(x => x.Name).ToList();
			}
			else
			{
				var type = typeof(TechnicalSkill);
				var sortProperty = type.GetProperty(matchedPropertyName);
				list = technicalSkillRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();
			}
			return new PagedList<TechnicalSkillViewModel>(list, technicalSkillRequestListViewModel.Skip ?? CommonConstants.Config.DEFAULT_SKIP, technicalSkillRequestListViewModel.Take ?? CommonConstants.Config.DEFAULT_TAKE);
		}

		public async Task<IEnumerable<TechnicalSkillViewModel>> ListTechnicalSkillByIdUserAsync(Guid? id)
		{
			var list = await GetAll()
				.Where(x => x.TechnicalSkillInCandidates.Any(y => y.CandidateId == id))
				.OrderBy(x => x.Name)
				.Select(x => new TechnicalSkillViewModel(x))
				.ToListAsync();
			return list;
		}

		public async Task<ResponseModel> GetTechnicalSkillAsync(Guid? id)
		{
			var technicalSkill = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
			if (technicalSkill != null)
			{
				return new ResponseModel()
				{
					StatusCode = System.Net.HttpStatusCode.OK,
					Data = technicalSkill
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

		public async Task<IEnumerable<TechnicalSkillViewModel>> GetAllTechnicalSkillAsync()
		{
			var list = await GetAll()
				.OrderBy(x => x.Name)
				.Select(x => new TechnicalSkillViewModel(x))
				.ToListAsync();
			return list;
		}

		public async Task<ResponseModel> CreateTechnicalSkillAsync(TechnicalSkillManageModel technicalSkillManageModel)
		{
			var technicalSkill = await _technicalSkillRepository.FetchFirstAsync(x => x.Name == technicalSkillManageModel.Name);
			if (technicalSkill != null)
			{
				return new ResponseModel()
				{
					StatusCode = System.Net.HttpStatusCode.BadRequest,
					Message = MessageConstants.EXISTED_CREATED
				};
			}
			else
			{
				technicalSkill = new TechnicalSkill();
				technicalSkill.Name = technicalSkillManageModel.Name;
				technicalSkill.Description = technicalSkillManageModel.Description;
				//technicalSkill = AutoMapper.Mapper.Map<TechnicalSkill>(technicalSkillManageModel);
				await _technicalSkillRepository.InsertAsync(technicalSkill);
			}
			return new ResponseModel()
			{
				StatusCode = System.Net.HttpStatusCode.OK,
				Data = new TechnicalSkillViewModel(technicalSkill),
				Message = MessageConstants.CREATED_SUCCESSFULLY
			};
		}

		public async Task<ResponseModel> UpdateTechnicalSkillAsync(Guid id, TechnicalSkillManageModel technicalSkillManageModel)
		{
			var technicalSkill = await _technicalSkillRepository.FetchFirstAsync(x => x.Id == id);
			if (technicalSkill == null)
			{
				return new ResponseModel()
				{
					StatusCode = System.Net.HttpStatusCode.NotFound,
					Message = MessageConstants.NOT_FOUND
				};
			}
			else
			{
				technicalSkillManageModel.SetDataToModel(technicalSkill);
				await _technicalSkillRepository.UpdateAsync(technicalSkill);
			}
			return new ResponseModel()
			{
				StatusCode = System.Net.HttpStatusCode.OK,
				Data = new TechnicalSkillViewModel(technicalSkill),
				Message = MessageConstants.CREATED_SUCCESSFULLY
			};
		}

		public async Task<ResponseModel> DeleteTechnicalSkillAsync(Guid id)
		{
			var technicalSkill = await _technicalSkillRepository.FetchFirstAsync(x => x.Id == id);
			if (technicalSkill == null)
			{
				return new ResponseModel()
				{
					StatusCode = System.Net.HttpStatusCode.NotFound,
					Message = MessageConstants.NOT_FOUND
				};
			}
			else
			{
				// Delete TechnicalSkill
				await _technicalSkillRepository.DeleteAsync(id);

				// Delete TechnicalSkillInCandidate
				await _technicalSkillInCandidateRepository.DeleteAsync(technicalSkill.TechnicalSkillInCandidates);

				return new ResponseModel()
				{
					StatusCode = System.Net.HttpStatusCode.OK,
					Message = MessageConstants.DELETED_SUCCESSFULLY
				};
			}
		}
	}
}
