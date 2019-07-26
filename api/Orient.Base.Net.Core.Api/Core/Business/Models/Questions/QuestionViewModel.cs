using Orient.Base.Net.Core.Api.Core.Business.Models.Answers;
using Orient.Base.Net.Core.Api.Core.Common.Helpers;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Questions
{
    public class QuestionViewModel
    {
        public QuestionViewModel()
        {

        }

        public QuestionViewModel(Question question) : this()
        {
            if (question != null)
            {
                Id = question.Id;
                Content = question.Content;
                StatusText = question.Status.GetEnumDescription();
                Status = question.Status;
                TypeText = question.Type.GetEnumDescription();
                Type = question.Type;
                LevelText = question.Level.GetEnumDescription();
                Level = question.Level;
                Answers = question.Answers != null ? question.Answers.OrderByDescending(x=> x.Status).Select(x => new AnswerViewModel(x)).ToArray() : null;
            }
        }

        public Guid Id { get; set; }

        public string Content { get; set; }

        public string StatusText { get; set; }

        public QuestionEnums.Status Status { get; set; }

        public string TypeText { get; set; }
        
        public QuestionEnums.Type Type { get; set; }

        public string LevelText { get; set; }

        public QuestionEnums.Level Level { get; set; }

        public AnswerViewModel[] Answers { get; set; }

    }
}
