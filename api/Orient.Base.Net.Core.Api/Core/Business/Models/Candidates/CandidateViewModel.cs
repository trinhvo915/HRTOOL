using Orient.Base.Net.Core.Api.Core.Business.Models.TechnicalSkills;
using Orient.Base.Net.Core.Api.Core.Common.Helpers;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Candidates
{
	public class CandidateViewModel
	{
		public CandidateViewModel()
		{

		}

		public CandidateViewModel(Candidate candidate) : this()
		{
			if (candidate != null)
			{
				Id = candidate.Id;
				Age = candidate.Age;
				Email = candidate.Email;
				Name = candidate.Name;
				Mobile = candidate.Mobile;
				DateOfBirth = candidate.DateOfBirth;
				Gender = candidate.Gender;
				About = candidate.About;
				AvatarUrl = candidate.AvatarUrl;
				Address = candidate.Address;
				Facebook = candidate.Facebook;
				Twitter = candidate.Twitter;
				LinkedIn = candidate.LinkedIn;
				Level = candidate.Level;
				YearOfExperienced = candidate.YearOfExperienced;
				Source = candidate.Source;
				TechnicalSkills = candidate.TechnicalSkillInCandidates != null ? candidate.TechnicalSkillInCandidates.Select(y => new TechnicalSkillViewModel(y.TechnicalSkill)).ToArray() : null;
			}
		}

		public Guid Id { get; set; }

		public int? Age { get; set; }

		public string Email { get; set; }

		public string Name { get; set; }

		public string Mobile { get; set; }

		public DateTime? DateOfBirth { get; set; }

		public UserEnums.Gender Gender { get; set; }

		public string About { get; set; }

		public string AvatarUrl { get; set; }

		public string Address { get; set; }

		public string Facebook { get; set; }

		public string Twitter { get; set; }

		public string LinkedIn { get; set; }

		public CandidateEnum.Level Level { get; set; }

		public int YearOfExperienced { get; set; }

		public string Source { get; set; }

		public TechnicalSkillViewModel[] TechnicalSkills { get; set; }
	}
}
