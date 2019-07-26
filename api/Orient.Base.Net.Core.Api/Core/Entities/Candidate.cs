using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
	[Table("Candidate")]
	public class Candidate : BaseEntity
	{
		public Candidate() : base()
		{

		}

		public int? Age
		{
			get
			{
				if (DateOfBirth.HasValue)
				{
					return (int)(DateTime.Now - DateOfBirth.Value).TotalDays / 365;
				}
				return null;
			}
		}

		[StringLength(255)]
		public string Email { get; set; }

		[StringLength(512)]
		[Required]
		public string Name { get; set; }

		[StringLength(50)]
		[Required]
		public string Mobile { get; set; }

		public DateTime? DateOfBirth { get; set; }

		public UserEnums.Gender Gender { get; set; }

		public string About { get; set; }

		[StringLength(512)]
		public string AvatarUrl { get; set; }

		[StringLength(1024)]
		public string Address { get; set; }

		[StringLength(512)]
		public string Facebook { get; set; }

		[StringLength(512)]
		public string Twitter { get; set; }

		[StringLength(512)]
		public string LinkedIn { get; set; }

		public CandidateEnum.Level Level { get; set; }

		public int YearOfExperienced { get; set; }

		public string Source { get; set; }

		public List<Interview> Interviews { get; set; }

		public List<TechnicalSkillInCandidate> TechnicalSkillInCandidates { get; set; }
	}
}
