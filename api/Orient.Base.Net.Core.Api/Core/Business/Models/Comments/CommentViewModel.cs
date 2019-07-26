using Orient.Base.Net.Core.Api.Core.Business.Models.Users;
using Orient.Base.Net.Core.Api.Core.Entities;
using System;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Comments
{
    public class CommentViewModel
    {
        public CommentViewModel()
        {

        }

        public CommentViewModel(Comment comment) : this()
        {
            if (comment != null)
            {
                id = comment.Id;
                Content = comment.Content;
                Username = comment.User.Name;
                UserId = comment.UserId;
                DateComment = comment.CreatedOn;
            }
        }

        public Guid id { get; set; }

        public string Content { get; set; }

        public Guid UserId { get; set; }

        public string Username { get; set; }

        public DateTime? DateComment { get; set; }
    }
}
