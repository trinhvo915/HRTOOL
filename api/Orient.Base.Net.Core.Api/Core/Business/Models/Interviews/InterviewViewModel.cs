using Orient.Base.Net.Core.Api.Core.Business.Models.Attachments;
using Orient.Base.Net.Core.Api.Core.Business.Models.Candidates;
using Orient.Base.Net.Core.Api.Core.Business.Models.Users;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Interviews
{
    public class InterviewViewModel
    {
        public InterviewViewModel()
        {

        }

        public InterviewViewModel(Interview interview) : this()
        {
            if (interview != null)
            {
                Id = interview.Id;
                Candidate = new CandidateViewModel(interview.Candidate);
                DateStart = interview.Calendar.DateStart;
                DateEnd = interview.Calendar.DateEnd;
                Description = interview.Calendar.Description;
                Status = interview.Status;
                Attachments = interview.AttachmentInInterviews != null ? interview.AttachmentInInterviews.Select(y => new AttachmentViewModel(y.Attachment)).ToArray() : null;
                Interviewers = interview.Calendar != null ?
                    (interview.Calendar.UserInCalendars != null ?
                    (interview.Calendar.UserInCalendars
                        .Select(x => new UserViewModel(x.User))
                        .OrderBy(x => x.Name)
                        .ToArray())
                    : null)
                    : null;
            }
        }

        public Guid Id { get; set; }

        public CandidateViewModel Candidate { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public string Description { get; set; }

        public InterviewEnums.Status Status { get; set; }

		public AttachmentViewModel[] Attachments { get; set; }

        public UserViewModel[] Interviewers { get; set; }
    }
}
