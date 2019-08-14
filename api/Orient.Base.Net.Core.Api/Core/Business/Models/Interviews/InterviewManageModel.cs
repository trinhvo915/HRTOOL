using Microsoft.EntityFrameworkCore;
using Orient.Base.Net.Core.Api.Core.Business.IoC;
using Orient.Base.Net.Core.Api.Core.Business.Models.Attachments;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Interviews
{
    public class InterviewManageModel : IValidatableObject
    {
        [Required]
        public DateTime? DateStart { get; set; }

        [Required]
        public DateTime? DateEnd { get; set; }

        public string Description { get; set; }

        [Required]
        public Guid[] InterviewerIds { get; set; }

        public Guid CandidateId { get; set; }

		[Required]
		public AttachmentManageModel[] Attachments { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var _candidateRepository = IoCHelper.GetInstance<IRepository<Candidate>>();
            var _userRepository = IoCHelper.GetInstance<IRepository<User>>();
            var _interviewRepository = IoCHelper.GetInstance<IRepository<Interview>>();

            var candidate = _candidateRepository.GetAll().FirstOrDefault(x => x.Id == CandidateId);
            if (candidate == null)
            {
                yield return new ValidationResult(CandidateMessagesConstants.NOT_FOUND, new string[] { "CandidateId" });
            }

            if (!InterviewerIds.Any())
            {
                yield return new ValidationResult(InterviewerMessagesConstants.EMPTY_INTERVIEWER_LIST, new string[] { "InterviewerIds" });
            }
            else
            {
                var interviewerIds = _userRepository.GetAll()
                    .Include(x => x.UserInRoles)
                    .Select(x => x.Id);
                if (!InterviewerIds.All(x => interviewerIds.Contains(x)))
                {
                    yield return new ValidationResult(InterviewerMessagesConstants.NOT_FOUND_INTERVIEWER_IN_LIST, new string[] { "InterviewerIds" });
                }
            }

            if (DateStart >= DateEnd)
            {
                yield return new ValidationResult(CalendarMessagesConstants.DATE_END_INVALIDATE, new string[] { "DateEnd" });
            }
        }
    }
}
