using Microsoft.EntityFrameworkCore;
using Orient.Base.Net.Core.Api.Core.Business.IoC;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Interviews
{
    public class InterviewStatusManageModel : IValidatableObject
    {
        public InterviewEnums.Status Status { get; set; }

        public void SetDataToModel(Interview interview)
        {
            interview.Status = Status;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var status = Enum.IsDefined(typeof(InterviewEnums.Status), Status);

            if (!status || Status == InterviewEnums.Status.Pending)
            {
                yield return new ValidationResult(InterviewMessagesConstants.INVALID_INTERVIEW_STATUS, new string[] { "Status" });
            }
        }
    }
}
