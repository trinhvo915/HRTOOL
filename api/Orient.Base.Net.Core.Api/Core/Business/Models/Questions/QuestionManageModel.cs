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

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Questions
{
    public class QuestionManageModel : IValidatableObject
    {
        [Required]
        public string Question { get; set; }

        public QuestionEnums.Type Type { get; set; }

        public QuestionEnums.Level Level { get; set; }

        public Guid AnswerId { get; set; }
        
        public void SetDataToModel(Question question)
        {
            question.Content = Question;
            question.Type = Type;
            question.Level = Level;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var answerRepository = IoCHelper.GetInstance<IRepository<Answer>>();
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

            var answer = answerRepository.GetAll().FirstOrDefault(x => x.Id == AnswerId);
            if (answer == null)
            {
                yield return new ValidationResult(AnswerMessagesConstants.NOT_FOUND, new string[] { "AnswerId" });
            }

        }
    }
}

