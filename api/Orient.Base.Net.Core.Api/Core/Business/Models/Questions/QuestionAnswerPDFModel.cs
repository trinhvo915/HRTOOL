using Orient.Base.Net.Core.Api.Core.Business.Models.Answers;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Questions
{
    public class QuestionAnswerPDFModel
    {
        public string Question { get; set; }
        public AnswerViewModel Answer { get; set; }

        public QuestionAnswerPDFModel(Question question) 
        {
            Question = question.Content;
            Answer = question.Answers != null ? question.Answers.Where(x=>x.Status == AnswerEnums.Status.Approved ).Select(x => new AnswerViewModel(x)).FirstOrDefault() : null;
        }
    }
}
