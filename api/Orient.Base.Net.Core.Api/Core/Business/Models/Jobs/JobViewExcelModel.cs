using Microsoft.EntityFrameworkCore.Internal;
using Orient.Base.Net.Core.Api.Core.Common.Reflections.Excel;
using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Linq;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Jobs
{
    public class JobViewExcelModel
    {
        public JobViewExcelModel()
        {

        }

        public JobViewExcelModel(Job job) : this()
        {
            if (job != null)
            {
                Name = job.Name;
                Categories = string.Join(",", job.JobInCategories.Select(y => y.Category.Name).ToArray());
                Status = job.Status.ToString();
                Priority = job.Priority.ToString();
                DateStart = job.DateStart;
                DateEnd = job.DateEnd;
                Description = job.Description;
                Creater = job.Reporter.Name;
                UsersReceive = string.Join(",", job.UserInJobs.Select(y => y.User.Name).ToArray());
                Steps = string.Join("\n", job.StepInJobs.OrderBy(x => x.RecordOrder).Select(x => job.StepInJobs.OrderBy(y => y.RecordOrder).IndexOf(x) + "." + x.Name).ToArray());
            }
        }

        [ExportExcel(DisplayName = "Name", Priority = 1)]
        public string Name { get; set; }

        [ExportExcel(DisplayName = "Categories", Priority = 2)]
        public string Categories { get; set; }

        [ExportExcel(DisplayName = "Status", Priority = 3)]
        public string Status { get; set; }

        [ExportExcel(DisplayName = "Priority", Priority = 4)]
        public string Priority { get; set; }

        [ExportExcel(DisplayName = "Date Start", Priority = 5)]
        public DateTime? DateStart { get; set; }

        [ExportExcel(DisplayName = "Date End", Priority = 6)]
        public DateTime? DateEnd { get; set; }

        [ExportExcel(DisplayName = "Description", Priority = 7)]
        public string Description { get; set; }

        [ExportExcel(DisplayName = "Creater", Priority = 8)]
        public string Creater { get; set; }

        [ExportExcel(DisplayName = "Users Receive", Priority = 9)]
        public string UsersReceive { get; set; }

        [ExportExcel(DisplayName = "Steps", Priority = 10)]
        public string Steps { get; set; }
    }
}
