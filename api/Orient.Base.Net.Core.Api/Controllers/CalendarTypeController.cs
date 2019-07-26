using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orient.Base.Net.Core.Api.Core.Business.Services;

namespace Orient.Base.Net.Core.Api.Controllers
{
    [Route("api/calendartypes")]
    [EnableCors("CorsPolicy")]
    public class CalendarTypeController : Controller
    {
        private readonly ICalendarTypeService _calendarTypeService;

        public CalendarTypeController(ICalendarTypeService calendarTypeService)
        {
            _calendarTypeService = calendarTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var calendarTypes = await _calendarTypeService.ListCalendarTypeAsync();
            return Ok(calendarTypes);
        }
    }
}