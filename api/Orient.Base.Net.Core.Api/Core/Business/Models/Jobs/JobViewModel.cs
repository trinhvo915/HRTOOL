using Orient.Base.Net.Core.Api.Core.Business.Models.Categories;
using Orient.Base.Net.Core.Api.Core.Business.Models.Users;
using Orient.Base.Net.Core.Api.Core.Business.Models.Comments;
using Orient.Base.Net.Core.Api.Core.Common.Helpers;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Linq;
using Orient.Base.Net.Core.Api.Core.Business.Models.Attachments;
using Orient.Base.Net.Core.Api.Core.Business.Models.StepInJobs;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Jobs
{
    public class JobViewModel
    {
        public JobViewModel()
        {

        }

        public JobViewModel(Job job) : this()
        {
            if (job != null)
            {
                Id = job.Id;
                Name = job.Name;
                StatusText = job.Status.GetEnumDescription();
                Status = job.Status;
                PriorityText = job.Priority.GetEnumDescription();
                Priority = job.Priority;
                DateStart = job.DateStart;
                DateEnd = job.DateEnd;
                Description = job.Description;
                Reporter = job.Reporter != null ? new BaseUserViewModel(job.Reporter) : null;
                Users = job.UserInJobs != null ? job.UserInJobs.Select(y => new BaseUserViewModel(y.User)).ToArray() : null;
                Categories = job.JobInCategories != null ? job.JobInCategories.Select(y => new CategoryViewModel(y.Category)).ToArray() : null;
                Users = job.UserInJobs != null ? job.UserInJobs.Select(y => new BaseUserViewModel(y.User)).ToArray() : null;
                Categories = job.JobInCategories != null ? job.JobInCategories.Select(y => new CategoryViewModel(y.Category)).ToArray() : null;
                Comments = job.Comments != null ? job.Comments.OrderBy(x => x.CreatedOn).Select(y => new CommentViewModel(y)).ToArray() : null;
                Attachments = job.AttachmentInJobs != null ? job.AttachmentInJobs.Select(y => new AttachmentViewModel(y.Attachment)).ToArray() : null;
                Steps = job.StepInJobs != null ? job.StepInJobs.Select(x => new StepInJobViewModel(x)).OrderBy(x => x.Order).ToArray() : null;
            }
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string StatusText { get; set; }

        public StatusEnums.Status Status { get; set; }

        public string PriorityText { get; set; }

        public PriorityEnums.Priority Priority { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public string Description { get; set; }

        public BaseUserViewModel Reporter { get; set; }

        public BaseUserViewModel[] Users { get; set; }

        public CategoryViewModel[] Categories { get; set; }

        public CommentViewModel[] Comments { get; set; }

        public AttachmentViewModel[] Attachments { get; set; }

        public StepInJobViewModel[] Steps { get; set; }
    }
}
