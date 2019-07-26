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
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Jobs
{
    public class UpdateJobManageModel : IValidatableObject
    {
        [Required]
        public string Name { get; set; }

        public StatusEnums.Status Status { get; set; }

        public PriorityEnums.Priority Priority { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public string Description { get; set; }

        public Guid ReporterId { get; set; }

        public Guid[] UserIds { get; set; }

        public Guid[] CategoryIds { get; set; }

        public AttachmentManageModel[] Attachments { get; set; }

        public void SetDataToModel(Job job)
        {
            job.Name = Name;
            job.Status = Status;
            job.Priority = Priority;
            job.DateStart = DateStart;
            job.DateEnd = DateEnd;
            job.Description = Description;
            job.ReporterId = ReporterId;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var userRepository = IoCHelper.GetInstance<IRepository<User>>();
            var categoryRepository = IoCHelper.GetInstance<IRepository<Category>>();

            var user = userRepository.GetAll().FirstOrDefault(x => x.Id == ReporterId);
            if (user == null)
            {
                yield return new ValidationResult(UserMessagesConstants.NOT_FOUND, new string[] { "ReporterId" });
            }

            var priority = Enum.IsDefined(typeof(PriorityEnums.Priority), Priority);
            var status = Enum.IsDefined(typeof(StatusEnums.Status), Status);

            if (!priority)
            {
                yield return new ValidationResult("Invalid priority of Job", new string[] { "Priority" });
            }
            if (!status)
            {
                yield return new ValidationResult("Invalid status of Job", new string[] { "Status" });
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
                yield return new ValidationResult("Invalid user", new string[] { "UserIds" });
            }
        }
    }
}
