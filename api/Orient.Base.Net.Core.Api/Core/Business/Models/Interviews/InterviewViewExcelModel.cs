using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.Common.Reflections.Excel;
using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Linq;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Interviews
{
    public class InterviewViewExcelModel
    {

        public InterviewViewExcelModel()
        {

        }

        public InterviewViewExcelModel(Interview interview) : this()
        {
            if (interview != null)
            {
                DateStart = interview.Calendar.DateStart;
                DateEnd = interview.Calendar.DateEnd;
                Interviewer = string.Join(",", (interview.Calendar.UserInCalendars
                              .Where(x => x.User.UserInRoles.Any(y => y.RoleId == RoleConstants.DevId))
                              .Select(x => x.User.Name)
                              .ToArray()));
                Candidate = interview.Candidate.Name;
                HumanResources = string.Join(",", (interview.Calendar.UserInCalendars
                                .Where(x => x.User.UserInRoles.Any(y => y.RoleId == RoleConstants.HRId || y.RoleId == RoleConstants.HRMId))
                                .Select(x => x.User.Name)
                                .ToArray()));
                Description = interview.Calendar.Description;
                Status = interview.Status.ToString();

            }
        }

        [ExportExcel(DisplayName = "Date Start", Priority = 4)]
        public DateTime? DateStart { get; set; }

        [ExportExcel(DisplayName = "Date End", Priority = 5)]
        public DateTime? DateEnd { get; set; }

        [ExportExcel(DisplayName = "Interviewer", Priority = 2)]
        public string Interviewer { get; set; }

        [ExportExcel(DisplayName = "Candidate", Priority = 1)]
        public string Candidate { get; set; }

        [ExportExcel(DisplayName = "Human Resources", Priority = 3)]
        public string HumanResources { get; set; }

        [ExportExcel(DisplayName = "Description", Priority = 6)]
        public string Description { get; set; }

        [ExportExcel(DisplayName = "Status", Priority = 7)]
        public string Status { get; set; }
    }
}
