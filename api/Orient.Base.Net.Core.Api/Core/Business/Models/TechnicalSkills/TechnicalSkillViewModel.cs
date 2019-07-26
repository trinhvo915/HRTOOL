using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.TechnicalSkills
{
	public class TechnicalSkillViewModel
	{
		public TechnicalSkillViewModel()
		{

		}

		public TechnicalSkillViewModel(TechnicalSkill skill)
		{
			if (skill != null)
			{
				Id = skill.Id;
				Name = skill.Name;
				Description = skill.Description;
			}
		}

		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }
	}
}
