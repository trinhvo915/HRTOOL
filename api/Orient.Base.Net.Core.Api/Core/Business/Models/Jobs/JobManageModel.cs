using Orient.Base.Net.Core.Api.Core.Business.IoC;
using Orient.Base.Net.Core.Api.Core.Business.Models.Attachments;
using Orient.Base.Net.Core.Api.Core.Business.Models.StepInJobs;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Jobs
{
    public class JobManageModel : IValidatableObject
    {
        [Required]
        public string Name { get; set; }

        public PriorityEnums.Priority Priority { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public string Description { get; set; }

        public Guid[] UserIds { get; set; }

        public Guid[] CategoryIds { get; set; }

        public AttachmentManageModel[] Attachments { get; set; }

        public StepInJobManageModel[] Steps { get; set; }

        public int DateRepeat { get; set; }

        public Guid? IdLink { get; set; }

        public JobManageModel() { }

        public JobManageModel(Job job)
        {
            Name = job.Name;
            Priority = job.Priority;
            DateStart = job.DateStart.Value.AddDays(job.DateRepeat);
            DateEnd = job.DateEnd != null ? job.DateEnd.Value.AddDays(job.DateRepeat) : (DateTime?)null;
            Description = job.Description;
            UserIds = job.UserInJobs.Select(x => x.UserId).ToArray();
            CategoryIds = job.JobInCategories.Select(x => x.CategoryId).ToArray();
            Attachments = job.AttachmentInJobs.Select(x => new AttachmentManageModel(x.Attachment)).ToArray();
            Steps = job.StepInJobs.OrderBy(x => x.RecordOrder).Select(x => new StepInJobManageModel(x)).ToArray();
            DateRepeat = job.DateRepeat;
            IdLink = job.Id;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var userRepository = IoCHelper.GetInstance<IRepository<User>>();
            var categoryRepository = IoCHelper.GetInstance<IRepository<Category>>();

            var priority = Enum.IsDefined(typeof(PriorityEnums.Priority), Priority);
            if (!priority)
            {
                yield return new ValidationResult("Invalid priority of Job", new string[] { "Priority" });
            }

            if (DateStart > DateEnd)
            {
                yield return new ValidationResult(MessageConstants.INVALID_DATE_END, new string[] { "DateEnd" });
            }

            if (CategoryIds == null || CategoryIds.Length == 0)
            {
                yield return new ValidationResult("Category not be null", new string[] { "CategoryIds" });
            }

            if (!CategoryIds.All(x => categoryRepository.GetAll().Select(y => y.Id).Contains(x)))
            {
                yield return new ValidationResult("Invalid category", new string[] { "CategoryIds" });
            }

            if (UserIds == null || UserIds.Length == 0)
            {
                yield return new ValidationResult("User not be null", new string[] { "UserIds" });
            }

            if (!UserIds.All(x => userRepository.GetAll().Select(y => y.Id).Contains(x)))
            {
                yield return new ValidationResult(UserMessagesConstants.NOT_FOUND, new string[] { "UserIds" });
            }

            if (DateRepeat <= 0)
            {
                yield return new ValidationResult("DateRepeat is less than or equal to Zero", new string[] { "DateRepeat" });
            }
        }
    }
}
