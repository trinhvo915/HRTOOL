using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("Question")]
    public class Question : BaseEntity
    {
        public Question() : base()
        {
            Status = QuestionEnums.Status.Pending;
        }

        public string Content { get; set; }

        public QuestionEnums.Status Status { get; set; }

        public QuestionEnums.Type Type { get; set; }

        public QuestionEnums.Level Level { get; set; }

        public List<Answer> Answers { get; set; }

    }
}
