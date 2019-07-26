using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Orient.Base.Net.Core.Api.Core.Business.Filters;
using Orient.Base.Net.Core.Api.Core.Business.Models.Questions;
using Orient.Base.Net.Core.Api.Core.Business.Services;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using RazorLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Controllers
{
    [Route("api/questions")]
    [EnableCors("CorsPolicy")]
    [ValidateModel]
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IPDFService _pdfService;

        public QuestionController(IQuestionService questionService, IPDFService pdfService)
        {
            _questionService = questionService;
            _pdfService = pdfService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllQuestion(QuestionRequestListViewModel questionRequestListViewModel)
        {
            var questions = await _questionService.ListQuestionAsync(questionRequestListViewModel);
            return Ok(questions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionById(Guid id)
        {
            var responseModel = await _questionService.GetQuestionByIdAsync(id);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel);
            }
            else
            {
                return NotFound(new { message = responseModel.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostQuestionAndAnswer([FromBody] QuestionAndAnswerManageModel questionAndAnswerManageModel)
        {
            var responseModel = await _questionService.CreateQuestionAndAnswerAsync(questionAndAnswerManageModel);
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
        public async Task<IActionResult> UpdateQuestion(Guid id, [FromBody] QuestionManageModel questionManageModel)
        {
            var responseModel = await _questionService.UpdateQuestionAsync(id, questionManageModel);
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
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            var responseModel = await _questionService.DeleteQuestionAsync(id);
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
        public async Task<IActionResult> UpdateInterviewStatus(Guid id, [FromBody] QuestionStatusManageModel interviewStatusManageModel)
        {
            var responseModel = await _questionService.UpdateQuestionStatusAsync(id, interviewStatusManageModel);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }

        [HttpGet("generate")]
        public async Task<IActionResult> GenerateRandomQuestion(QuestionGenerateRequestModel questionGenerateRequestModel)
        {
            var responseModels = await _questionService.GenerateInterviewQuestionPDF(questionGenerateRequestModel);
            var result = new List<string>();

            foreach (var responseModel in responseModels)
            {
                if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    result.Add(Url.Action("GetFile", "Media", new { folder = "PDFDocument", fileName = responseModel.Data, }, Request.Scheme));
                }
                else
                {
                    return BadRequest(new { Message = responseModel.Message });
                }
            }
            return Ok(result);
        }
    }
}
