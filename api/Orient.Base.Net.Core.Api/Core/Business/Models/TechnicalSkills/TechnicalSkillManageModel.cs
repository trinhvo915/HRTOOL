using Orient.Base.Net.Core.Api.Core.Business.IoC;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.TechnicalSkills
{
	public class TechnicalSkillManageModel
	{
		public TechnicalSkillManageModel()
		{

		}

		public void SetDataToModel(TechnicalSkill technicalSkill)
		{
			technicalSkill.Name = Name;
			technicalSkill.Description = Description;
		}

		[Required]
		public string Name { get; set; }

		public string Description { get; set; }
	}
}
