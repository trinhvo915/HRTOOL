using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orient.Base.Net.Core.Api.Core.Business.Filters;
using Orient.Base.Net.Core.Api.Core.Business.Models.Interviews;
using Orient.Base.Net.Core.Api.Core.Business.Models.Jobs;
using Orient.Base.Net.Core.Api.Core.Business.Services;

namespace Orient.Base.Net.Core.Api.Controllers
{
    [Route("api/dashboard")]
    [EnableCors("CorsPolicy")]
    [ValidateModel]
    public class DashboardController : BaseController
    {
        private readonly IJobService _jobService;
        private readonly IInterviewService _interviewService;

        public DashboardController(IJobService jobService, IInterviewService interviewService)
        {
            _jobService = jobService;
            _interviewService = interviewService;
        }

        [HttpGet("jobs")]
        public async Task<IActionResult> GetJobAll(JobRequestStat jobRequestStat)
        {
            var listStatJob = await _jobService.GetJobStatAsync();
            return Ok(listStatJob);
        }

        [HttpGet("jobs-animation")]
        public async Task<IActionResult> GetJobAnimationAll(JobRequestStat jobRequestStat)
        {
            var listStatJob = await _jobService.GetJobStatAnimationAsync(jobRequestStat);
            return Ok(listStatJob);
        }

        [HttpGet("interviews")]
        public async Task<IActionResult> GetInterviewAll()
        {
            var listStatInterview = await _interviewService.GetInterviewStatAsync();
            return Ok(listStatInterview);
        }

        [HttpGet("interviews-animation")]
        public async Task<IActionResult> GetInterviewAnimationAll(InterviewRequestStat interviewRequestStat)
        {
            var listStatInterview = await _interviewService.GetInterviewStatAnimationAsync(interviewRequestStat);
            return Ok(listStatInterview);
        }
    }
}