using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.Common.Utilities;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Candidates
{
	public class CandidateManageModel : IValidatableObject
	{
		[RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = MessageConstants.INVALID_EMAIL)]
		public string Email { get; set; }

		public string Name { get; set; }

		public string Mobile { get; set; }

		public DateTime? DateOfBirth { get; set; }

		public UserEnums.Gender Gender { get; set; }

		public string About { get; set; }

		public string AvatarUrl { get; set; }

		public string Address { get; set; }

		[RegularExpression(@"(?:https?:\/\/)?(?:www\.)?(?:facebook|fb|m\.facebook)\.(?:com|me)\/(?:(?:\w)*#!\/)?(?:pages\/)?(?:[\w\-]*\/)*([\w\-\.]+)(?:\/)?", ErrorMessage = MessageConstants.INVALID_FACEBOOK)]
		public string Facebook { get; set; }

		[RegularExpression(@"(?:http(?:s)?:\/\/)?(?:www\.)?twitter\.com\/([a-zA-Z0-9_]+)", ErrorMessage = MessageConstants.INVALID_TWITTER)]
		public string Twitter { get; set; }

		[RegularExpression(@"((?:http(s?):\/\/)*([a-zA-Z0-9\-])*\.|[linkedin])[linkedin/~\-]+\.[a-zA-Z0-9/~\-_,&=\?\.;]+[^\.,\s<]", ErrorMessage = MessageConstants.INVALID_LINKEDIN)]
		public string LinkedIn { get; set; }

		public CandidateEnum.Level Level { get; set; }

		[Range(0, int.MaxValue, ErrorMessage = "Invalid year of experienced")]
		public int YearOfExperienced { get; set; }

		public string Source { get; set; }

		public Guid[] TechnicalSkillIds { get; set; }

		public void SetDataToModel(Candidate candidate)
		{
			candidate.Email = Email;
			candidate.Name = Name;
			candidate.Mobile = Mobile;
			candidate.DateOfBirth = DateOfBirth;
			candidate.About = About;
			candidate.AvatarUrl = AvatarUrl;
			candidate.Gender = Gender;
			candidate.Address = Address;
			candidate.Facebook = Facebook;
			candidate.Twitter = Twitter;
			candidate.LinkedIn = LinkedIn;
			candidate.Level = Level;
			candidate.YearOfExperienced = YearOfExperienced;
			candidate.Source = Source;
		}

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var gender = Enum.IsDefined(typeof(UserEnums.Gender), Gender);

			if (!gender)
			{
				yield return new ValidationResult("Invalid gender of Candidate", new string[] { "Gender" });
			}

			var level = Enum.IsDefined(typeof(CandidateEnum.Level), Level);

			if (!level)
			{
				yield return new ValidationResult("Invalid level of Candidate", new string[] { "Level" });
			}
		}
	}
}
