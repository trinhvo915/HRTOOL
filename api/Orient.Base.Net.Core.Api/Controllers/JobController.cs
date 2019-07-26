using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Orient.Base.Net.Core.Api.Core.Business.Filters;
using Orient.Base.Net.Core.Api.Core.Business.Models.Jobs;
using Orient.Base.Net.Core.Api.Core.Business.Services;
using System;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Controllers
{
    [Route("api/jobs")]
    [EnableCors("CorsPolicy")]
    [ValidateModel]
    public class JobController : Controller
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll(JobRequestListViewModel jobRequestListViewModel)
        {
            var list = await _jobService.ListJobAsync(jobRequestListViewModel);
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobById(Guid id)
        {
            var responseModel = await _jobService.GetJobByIdAsync(id);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel);
            }
            else
            {
                return NotFound(new { Message = responseModel.Message });
            }
        }

        [HttpPost()]
        public async Task<IActionResult> PostJob([FromBody] JobManageModel jobManageModel)
        {
            var responseModel = await _jobService.CreateJobAsync(jobManageModel);
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
        public async Task<IActionResult> UpdateJob(Guid id, [FromBody] UpdateJobManageModel updateJobManageModel)
        {
            var responseModel = await _jobService.UpdateJobAsync(id, updateJobManageModel);
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
        public async Task<IActionResult> DeleteJob(Guid id)
        {
            var responseModel = await _jobService.DeleteJobAsync(id);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
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
