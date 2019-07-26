using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Entities.Enums
{
    public class QuestionEnums
    {
        public enum Status
        {
            Pending = 1,
            Rejected,
            Approved
        }

        public enum Level
        {
            Fresher = 1,
            Junior,
            Senior
        }

        public enum Type
        {
            BackEnd = 1,
            FrontEnd,
            Design,
            Testing
        }
    }
}
