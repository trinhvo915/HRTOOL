using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Orient.Base.Net.Core.Api.Core.Business.Filters;
using Orient.Base.Net.Core.Api.Core.Business.Models.Calendars;
using Orient.Base.Net.Core.Api.Core.Business.Services;

namespace Orient.Base.Net.Core.Api.Controllers
{
    [Route("api/calendars")]
    [EnableCors("CorsPolicy")]
    [ValidateModel]
    public class CalendarController : BaseController
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CalendarRequestListViewModel calendarRequestListViewModel)
        {
            var calendar = await _calendarService.ListCalendarAsync(calendarRequestListViewModel);
            return Ok(calendar);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCalendarById(Guid id)
        {
            var responseModel = await _calendarService.GetCalendarByIdAsync(id);
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
        public async Task<IActionResult> Post([FromBody] CalendarManageModel calendarManageModel)
        {
            var responseModel = await _calendarService.CreateCalendarAsync(CurrentUserId, calendarManageModel);
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
        [CustomAuthorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] CalendarManageModel calendarManageModel)
        {
            var responseModel = await _calendarService.UpdateCalendarAsync(CurrentUserId, id, calendarManageModel);
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
            var responseModel = await _calendarService.DeteteCalendarAsync(CurrentUserId, id);
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