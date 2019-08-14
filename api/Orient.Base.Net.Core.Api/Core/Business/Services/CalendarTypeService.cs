using Microsoft.EntityFrameworkCore;
using Orient.Base.Net.Core.Api.Core.Business.Models.CalendarTypes;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Services
{
	public interface ICalendarTypeService
	{
		Task<List<CalendarTypeViewModel>> ListCalendarTypeAsync();
	}

	public class CalendarTypeService : ICalendarTypeService
	{
		#region Fields

		private readonly IRepository<CalendarType> _calendarTypeRepository;

		#endregion

		#region Constructor

		public CalendarTypeService(IRepository<CalendarType> calendarTypeRepository)
		{
			_calendarTypeRepository = calendarTypeRepository;
		}

		#endregion

		#region Private Methods

		private IQueryable<CalendarType> GetAll()
		{
			return _calendarTypeRepository.GetAll();
		}

		#endregion

		public async Task<List<CalendarTypeViewModel>> ListCalendarTypeAsync()
		{
			return await GetAll().Where(x => x.Name != CalendarTypeConstants.Interview).Select(x => new CalendarTypeViewModel(x)).ToListAsync();
		}
	}
}
