using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
	[Table("TechnicalSkill")]
	public class TechnicalSkill : BaseEntity
	{
		public TechnicalSkill() : base()
		{

		}

		public string Name { get; set; }

		public string Description { get; set; }

		public List<TechnicalSkillInCandidate> TechnicalSkillInCandidates { get; set; }
	}
}
