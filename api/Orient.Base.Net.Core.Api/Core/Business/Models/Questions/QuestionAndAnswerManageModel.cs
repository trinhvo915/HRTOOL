using Orient.Base.Net.Core.Api.Core.Business.IoC;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Questions
{
    public class QuestionAndAnswerManageModel : IValidatableObject
    {
        [Required]
        public string Question { get; set; }

        [Required]
        public string Answer { get; set; }

        public QuestionEnums.Type Type { get; set; }

        public QuestionEnums.Level Level { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var questionRepository = IoCHelper.GetInstance<IRepository<Question>>();

            var isQuestionExisted = questionRepository.GetAll().Any(x => x.Content.Trim().ToLower() == Question.Trim().ToLower());
            if (isQuestionExisted)
            {
                yield return new ValidationResult(QuestionMessagesConstants.EXISTED_QUESTION_CONTENT, new string[] { "Question" });
            }

            var type = Enum.IsDefined(typeof(QuestionEnums.Type), Type);
            if (!type)
            {
                yield return new ValidationResult("Invalid type of Question", new string[] { "Type" });
            }

            var level = Enum.IsDefined(typeof(QuestionEnums.Level), Level);
            if (!level)
            {
                yield return new ValidationResult("Invalid level of Question", new string[] { "Level" });
            }
        }
    }
}
