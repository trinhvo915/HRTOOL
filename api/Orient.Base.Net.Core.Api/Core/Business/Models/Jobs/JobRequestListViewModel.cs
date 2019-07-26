using Orient.Base.Net.Core.Api.Core.Business.Models.Base;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Jobs
{
	public class JobRequestListViewModel : RequestListViewModel
	{
		public JobRequestListViewModel()
			: base()
		{

		}

		public string Query { get; set; }

		public bool? IsActive { get; set; }
	}
}
