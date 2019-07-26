using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Orient.Base.Net.Core.Api.Core.Business.Filters;
using Orient.Base.Net.Core.Api.Core.Business.Models.Base;
using Orient.Base.Net.Core.Api.Core.Business.Models.Candidates;
using Orient.Base.Net.Core.Api.Core.Business.Services;

namespace Orient.Base.Net.Core.Api.Controllers
{
	[Route("api/candidates")]
	[EnableCors("CorsPolicy")]
	[ValidateModel]
	public class CandidateController : Controller
	{
		private readonly ICandidateService _candidateService;

		public CandidateController(ICandidateService candidateService)
		{
			_candidateService = candidateService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll(CandidateRequestListViewModel candidateRequestListViewModel)
		{
			var candidate = await _candidateService.ListCandidateAsync(candidateRequestListViewModel);
			return Ok(candidate);
		}

        [HttpGet("all-candidates")]
        public async Task<IActionResult> GetAllCandidate(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
        {
            var candidate = await _candidateService.GetAllCandidateAsync(baseRequestGetAllViewModel);
            return Ok(candidate);
        }

        [HttpGet("{id}")]
		public async Task<IActionResult> GetCandidateById(Guid id)
		{
			var responseModel = await _candidateService.GetCandidateByIdAsync(id);
			if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
			{
				return Ok(responseModel);
			}
			else
			{
				return BadRequest(new { Message = responseModel.Message });
			}
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CandidateManageModel candidateManageModel)
		{
			var responseModel = await _candidateService.CreateCandidateAsync(candidateManageModel);
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
		public async Task<IActionResult> Update(Guid id, [FromBody] CandidateManageModel candidateManageModel)
		{
			var responseModel = await _candidateService.UpdateCandidateAsync(id, candidateManageModel);
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
		public async Task<IActionResult> Delete(Guid id)
		{
			var responseModel = await _candidateService.DeteteCandidateAsync(id);
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
