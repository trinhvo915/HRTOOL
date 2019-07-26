using Orient.Base.Net.Core.Api.Core.Business.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Questions
{
    public class QuestionRequestListViewModel : RequestListViewModel
    {
        public QuestionRequestListViewModel()
            : base()
        {

        }

        public string Query { get; set; }

        public bool? IsActive { get; set; }
    }
}
