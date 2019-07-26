using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Questions
{
    public class QuestionStatusManageModel : IValidatableObject
    {
        public QuestionEnums.Status Status { get; set; }

        public void SetDataToModel(Question question)
        {
            question.Status = Status;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var status = Enum.IsDefined(typeof(QuestionEnums.Status), Status);

            if (!status)
            {
                yield return new ValidationResult(QuestionMessagesConstants.INVALID_QUESTION_STATUS, new string[] { "Status" });
            }
        }
    }
}
