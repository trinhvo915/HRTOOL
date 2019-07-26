using Orient.Base.Net.Core.Api.Core.Business.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Interviews
{
    public class InterviewRequestListViewModel : RequestListViewModel
    {
        public InterviewRequestListViewModel() : base()
        {

        }
        public bool? IsActive { get; set; }

        public string Query { get; set; }
    }
}
