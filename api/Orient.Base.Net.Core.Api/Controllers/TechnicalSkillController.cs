using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Orient.Base.Net.Core.Api.Core.Business.Filters;
using Orient.Base.Net.Core.Api.Core.Business.Models.TechnicalSkills;
using Orient.Base.Net.Core.Api.Core.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Controllers
{
	[Route("api/technical-skills")]
	[EnableCors("CorsPolicy")]
	[ValidateModel]
	public class TechnicalSkillController : BaseController
	{
		private readonly ITechnicalSkillService _technicalSkillService;
		public TechnicalSkillController(ITechnicalSkillService technicalSkillService)
		{
			_technicalSkillService = technicalSkillService;
		}

		[HttpGet]
		public async Task<IActionResult> Get(TechnicalSkillRequestListViewModel technicalSkillRequestListViewModel)
		{
			var technicalSkills = await _technicalSkillService.ListTechnicalSkillAsync(technicalSkillRequestListViewModel);
			return Ok(technicalSkills);
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAllTechnicalSkill(TechnicalSkillRequestListViewModel technicalSkillRequestListViewModel)
		{
			var technicalSkills = await _technicalSkillService.GetAllTechnicalSkillAsync();
			return Ok(technicalSkills);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetTechnicalSkillByCandidateId(Guid? id)
		{
			var technicalSkills = await _technicalSkillService.ListTechnicalSkillByIdUserAsync(id);
			return Ok(technicalSkills);
		}

		[HttpPost]
		public async Task<IActionResult> PostTechnicalSkill([FromBody] TechnicalSkillManageModel technicalSkillManageModel)
		{
			var responseModel = await _technicalSkillService.CreateTechnicalSkillAsync(technicalSkillManageModel);
			if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
			{
				return Ok(responseModel);
			}
			else
			{
				return BadRequest(new { Message = responseModel.Message });
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutTechnicalSkill(Guid id, [FromBody] TechnicalSkillManageModel technicalSkillManageModel)
		{
			var responseModel = await _technicalSkillService.UpdateTechnicalSkillAsync(id, technicalSkillManageModel);
			if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
			{
				return Ok(responseModel);
			}
			else
			{
				return BadRequest(new { Message = responseModel.Message });
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteTechnicalSkill(Guid id)
		{
			var responseModel = await _technicalSkillService.DeleteTechnicalSkillAsync(id);
			if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
			{
				return Ok(responseModel);
			}
			else
			{
				return BadRequest(new { Message = responseModel.Message });
			}
		}
	}
}
