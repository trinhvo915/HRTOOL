using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
	[Table("TechnicalSkillInCandidate")]
	public class TechnicalSkillInCandidate : BaseEntity
	{
		public TechnicalSkillInCandidate() : base()
		{

		}

		public Guid CandidateId { get; set; }

		public Candidate Candidate { get; set; }

		public Guid TechnicalSkillId { get; set; }

		public TechnicalSkill TechnicalSkill { get; set; }
	}
}
