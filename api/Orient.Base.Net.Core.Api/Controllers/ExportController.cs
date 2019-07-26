using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Orient.Base.Net.Core.Api.Core.Business.Models.Base;
using Orient.Base.Net.Core.Api.Core.Business.Models.Candidates;
using Orient.Base.Net.Core.Api.Core.Business.Models.Interviews;
using Orient.Base.Net.Core.Api.Core.Business.Models.Jobs;
using Orient.Base.Net.Core.Api.Core.Business.Services;
using Orient.Base.Net.Core.Api.Core.Common.Reflections;

namespace Orient.Base.Net.Core.Api.Controllers
{
    [Route("api/export")]
    [EnableCors("CorsPolicy")]
    public class ExportController : BaseController
    {
        private IInterviewService _interviewService;

        private IJobService _jobService;

        private ICandidateService _candidateService;

        public ExportController(IInterviewService interviewService, IJobService jobService, ICandidateService candidateService)
        {
            _interviewService = interviewService;
            _jobService = jobService;
            _candidateService = candidateService;
        }

        //export interview
        [HttpGet("interviews")]
        public async Task<IActionResult> GetInterviewsExcel(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
        {
            var interviews = await _interviewService.GetInterviewViewExcel(baseRequestGetAllViewModel);

            var stream = ExportExcelUtilities.ExportExcel<InterviewViewExcelModel>(interviews);

            string excelName = $"Interview-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        //export Job
        [HttpGet("jobs")]
        public async Task<IActionResult> GetJobsExcel(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
        {
            var jobs = await _jobService.GetJobViewExcel(baseRequestGetAllViewModel);

            var stream = ExportExcelUtilities.ExportExcel<JobViewExcelModel>(jobs);

            string excelName = $"Job-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        //export Candidate
        [HttpGet("candidates")]
        public async Task<IActionResult> GetCandidatesExcel(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
        {
            var candidates = await _candidateService.GetCandidateViewExcel(baseRequestGetAllViewModel);

            var stream = ExportExcelUtilities.ExportExcel<CandidateViewExcelModel>(candidates);

            string excelName = $"Candidate-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}