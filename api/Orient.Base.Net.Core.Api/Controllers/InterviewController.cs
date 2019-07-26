using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Orient.Base.Net.Core.Api.Core.Business.Filters;
using Orient.Base.Net.Core.Api.Core.Business.Models.Base;
using Orient.Base.Net.Core.Api.Core.Business.Models.Interviews;
using Orient.Base.Net.Core.Api.Core.Business.Services;
using Orient.Base.Net.Core.Api.Core.Common.Reflections;

namespace Orient.Base.Net.Core.Api.Controllers
{
    [Route("api/interviews")]
    [EnableCors("CorsPolicy")]
    [ValidateModel]
    public class InterviewController : BaseController
    {
        private readonly IInterviewService _interviewService;

        public InterviewController(IInterviewService interviewService)
        {
            _interviewService = interviewService;
        }

        [HttpGet]
        //[CustomAuthorize(Roles = new string[] {RoleConstants.SA })]
        public async Task<IActionResult> GetAll(InterviewRequestListViewModel interviewRequestListViewModel)
        {
            var interviews = await _interviewService.ListInterviewAsync(interviewRequestListViewModel);
            return Ok(interviews);
        }

        [HttpGet("{id}")]
        //[CustomAuthorize(Roles = new string[] {RoleConstants.SA })]
        public async Task<IActionResult> GetInterviewById(Guid id)
        {
            var responseModel = await _interviewService.GetInterviewByIdAsync(id);
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
        //[CustomAuthorize(Roles = new string[] {RoleConstants.SA })]
        public async Task<IActionResult> CreateInterview([FromBody] InterviewManageModel interviewManageModel)
        {
            var responseModel = await _interviewService.CreateInterviewAsync(CurrentUserId, interviewManageModel);
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
        // [CustomAuthorize(Roles = new string[] {RoleConstants.SA })]
        public async Task<IActionResult> UpdateInterview(Guid id, [FromBody] InterviewManageModel interviewManageModel)
        {
            var responseModel = await _interviewService.UpdateInterviewAsync(id, interviewManageModel);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }

        [HttpPut("{id}/status")]
        // [CustomAuthorize(Roles = new string[] {RoleConstants.SA })]
        public async Task<IActionResult> UpdateInterviewStatus(Guid id, [FromBody] InterviewStatusManageModel interviewStatusManageModel)
        {
            var responseModel = await _interviewService.UpdateInterviewStatus(id, interviewStatusManageModel);
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
        // [CustomAuthorize(Roles = new string[] {RoleConstants.SA })]
        public async Task<IActionResult> DeleteInterview(Guid id)
        {
            var responseModel = await _interviewService.DeleteInterviewAsync(id);
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