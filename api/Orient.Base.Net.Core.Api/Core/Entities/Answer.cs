using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("Answer")]
    public class Answer : BaseEntity
    {
        public Answer() : base()
        {
            Status = AnswerEnums.Status.Pending;
        }

        public string Content { get; set; }

        public AnswerEnums.Status Status { get; set; }

        public Guid QuestionId { get; set; }

        public Question Question { get; set; }

    }
}