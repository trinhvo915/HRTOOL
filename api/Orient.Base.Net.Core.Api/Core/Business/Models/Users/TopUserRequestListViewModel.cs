using Orient.Base.Net.Core.Api.Core.Business.Models.Base;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Users
{
    public class TopUserRequestListViewModel : RequestListViewModel
    {
        public TopUserRequestListViewModel() : base()
        {

        }

        public string Query { get; set; }
        public bool? IsActive { get; set; }
        public SortType? SortType { get; set; }

    }

    public enum SortType
    {
        Day3 = 1,
        Day7,
        Day30,
        Day90,
        Day365
    }
}
