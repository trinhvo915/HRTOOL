using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("Comment")]
    public class Comment : BaseEntity
    {
        public Comment() : base()
        {

        }

        [Required]
        public string Content { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid JobId { get; set; }

        public Job Job { get; set; }
    }
}
