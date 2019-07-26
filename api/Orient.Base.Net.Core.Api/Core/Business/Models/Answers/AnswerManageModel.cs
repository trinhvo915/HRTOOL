using Microsoft.EntityFrameworkCore;
using Orient.Base.Net.Core.Api.Core.Business.IoC;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Answers
{
    public class AnswerManageModel : IValidatableObject
    {
        [Required]
        public string Content { get; set; }

        public Guid QuestionId { get; set; }

        public void SetDataToModel(Answer answer)
        {
            answer.Content = Content;
            answer.QuestionId = QuestionId;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var questionRepository = IoCHelper.GetInstance<IRepository<Question>>();

            var question = questionRepository.GetAll().Include(x => x.Answers).FirstOrDefault(x => x.Id == QuestionId);
            if (question == null)
            {
                yield return new ValidationResult(QuestionMessagesConstants.NOT_FOUND, new string[] { "QuestionId" });
            }

            var isAnswerExisted = question.Answers.Any(x => x.Content.Trim().ToLower() == Content.Trim().ToLower());
            if (isAnswerExisted)
            {
                yield return new ValidationResult(AnswerMessagesConstants.EXISTED_ANSWER, new string[] { "Content" });
            }
        }
    }
}
