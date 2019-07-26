using Orient.Base.Net.Core.Api.Core.Business.Models.Questions;
using Orient.Base.Net.Core.Api.Core.Common.Helpers;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Answers
{
    public class AnswerViewModel
    {
        public AnswerViewModel()
        {

        }

        public AnswerViewModel(Answer answer) : this()
        {
            if (answer != null)
            {
                Id = answer.Id;
                Content = answer.Content;
                StatusText = answer.Status.GetEnumDescription();
                Status = answer.Status;
            }
        }
        public Guid Id { get; set; }

        public string Content { get; set; }

        public string StatusText { get; set; }

        public AnswerEnums.Status Status { get; set; }
    }
}
