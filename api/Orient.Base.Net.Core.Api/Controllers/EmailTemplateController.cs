using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orient.Base.Net.Core.Api.Core.Business.Filters;
using Orient.Base.Net.Core.Api.Core.Business.Models.EmailTemplates;
using Orient.Base.Net.Core.Api.Core.Business.Services;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;

namespace Orient.Base.Net.Core.Api.Controllers
{
    [Route("api/emailtemplates")]
    [EnableCors("CorsPolicy")]
    [ValidateModel]
    public class EmailTemplateController : BaseController
    {
        private readonly IEmailTemplateService _emailTemplateService;

        public EmailTemplateController(IEmailTemplateService emailTemplateService)
        {
            _emailTemplateService = emailTemplateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(EmailTemplateRequestListViewModel emailTemplateRequestListViewModel)
        {
            var emailTemplate = await _emailTemplateService.ListEmailTemplateAsync(emailTemplateRequestListViewModel);
            return Ok(emailTemplate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInterview(Guid id, [FromBody] EmailTemplateManageModel emailTemplateManageModel)
        {
            var responseModel = await _emailTemplateService.UpdateEmailTemplateAsync(id, emailTemplateManageModel);
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