using Orient.Base.Net.Core.Api.Core.Common.Reflections.Excel;
using Orient.Base.Net.Core.Api.Core.Entities;
using System.Linq;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Candidates
{
    public class CandidateViewExcelModel
    {
        public CandidateViewExcelModel()
        {

        }

        public CandidateViewExcelModel(Candidate candidate) : this()
        {
            Name = candidate.Name;
            Age = candidate.Age.ToString();
            Email = candidate.Email;
            Phone = candidate.Mobile;
            Gender = candidate.Gender.ToString();
            About = candidate.About;
            Address = candidate.Address;
            TechnicalSkills = string.Join(",", candidate.TechnicalSkillInCandidates.Select(y => y.TechnicalSkill.Name).ToArray());
            Level = candidate.Level.ToString();
            YearOfExperienced = candidate.YearOfExperienced;
        }

        [ExportExcel(DisplayName = "Name", Priority = 1)]
        public string Name { get; set; }

        [ExportExcel(DisplayName = "Age", Priority = 2)]
        public string Age { get; set; }

        [ExportExcel(DisplayName = "Email", Priority = 3)]
        public string Email { get; set; }

        [ExportExcel(DisplayName = "Phone", Priority = 4)]
        public string Phone { get; set; }

        [ExportExcel(DisplayName = "Gender", Priority = 5)]
        public string Gender { get; set; }

        [ExportExcel(DisplayName = "About", Priority = 6)]
        public string About { get; set; }

        [ExportExcel(DisplayName = "Address", Priority = 7)]
        public string Address { get; set; }

        [ExportExcel(DisplayName = "Technical Skill", Priority = 8)]
        public string TechnicalSkills { get; set; }

        [ExportExcel(DisplayName = "Level", Priority = 9)]
        public string Level { get; set; }

        [ExportExcel(DisplayName = "Year Of Experienced", Priority = 10)]
        public int YearOfExperienced { get; set; }
    }
}
