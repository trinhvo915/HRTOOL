using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Orient.Base.Net.Core.Api.Core.Business.Filters;
using Orient.Base.Net.Core.Api.Core.Business.Models.StepInJobs;
using Orient.Base.Net.Core.Api.Core.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Controllers
{
    [Route("api/stepInJobs")]
    [EnableCors("CorsPolicy")]
    [ValidateModel]
    public class StepInJobController : BaseController
    {
        private readonly IStepInJobService _stepInJobService;

        public StepInJobController(IStepInJobService stepInJobService)
        {
            _stepInJobService = stepInJobService;
        }

        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetStepInJobs(Guid jobId, StepInJobRequestListViewModel stepInJobRequestListViewModel)
        {
            var list = await _stepInJobService.GetStepInJobAsync(jobId, stepInJobRequestListViewModel);
            return Ok(list);
        }

        [HttpPost("{jobId}/steps")]
        public async Task<IActionResult> CreateStepInJobs(Guid jobId, [FromBody] StepInJobManageModel[] stepInJobManageModel)
        {
            var responseModel = await _stepInJobService.CreateStepInJobsAsync(jobId, stepInJobManageModel);
            if(responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }

        [HttpPost("{jobId}/step")]
        public async Task<IActionResult> CreateStepInJob(Guid jobId, [FromBody] StepInJobManageModel stepInJobManageModel)
        {
            var responseModel = await _stepInJobService.CreateStepInJobAsync(jobId, stepInJobManageModel);
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
        public async Task<IActionResult> UpdateStepInJob(Guid id, [FromBody] UpdateStepInJobManageModel updateStepInJobManageModel)
        {
            var responseModel = await _stepInJobService.UpdateStepInJobAsync(id, updateStepInJobManageModel);
            if(responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStepInJob(Guid id)
        {
            var responseModel = await _stepInJobService.DeleteStepInJobAsync(id);
            if(responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel);
            }
            else
            {
                return NotFound(new { Message = "Not Found" });
            }
        }
    }
}
