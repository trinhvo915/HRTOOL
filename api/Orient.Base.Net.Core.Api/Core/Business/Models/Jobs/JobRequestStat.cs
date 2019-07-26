using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Jobs
{
    public class JobRequestStat
    {
        public JobRequestStat()
        {

        }

        public DateTime ? DateStart { get; set; }

        public DateTime ? DateEnd { get; set; }
    }
}
