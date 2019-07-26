using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Questions
{
    public class QuestionGenerateRequestModel : IValidatableObject
    {
        public QuestionEnums.Level Level { get; set; }

        public QuestionEnums.Type[] Types { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Invalid quantity")]
        public int? Quantity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var level = Enum.IsDefined(typeof(QuestionEnums.Level), Level);
            if (!level)
            {
                yield return new ValidationResult("Invalid level of Question", new string[] { "Level" });
            }

            if (Types != null)
            {
                foreach (var type in Types)
                {
                    var validType = Enum.IsDefined(typeof(QuestionEnums.Type), type);
                    if (!validType)
                    {
                        yield return new ValidationResult("Invalid Type of Question", new string[] { "Types" });
                    }
                }
            }
            else
            {
                yield return new ValidationResult("Missing Types of Question", new string[] { "Types" });
            }
        }
    }
}
