using Orient.Base.Net.Core.Api.Core.Business.Models.EmailTemplates;
using Orient.Base.Net.Core.Api.Core.Entities;
using RazorLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Common.Helpers
{
	public class EmailTemplateHelper
	{
		public static async Task<EmailTemplateResponseModel> ParseModelAsync<TModel>(EmailTemplate emailTemplate, TModel model)
		{
			var emailTemplateResponseModel = new EmailTemplateResponseModel(emailTemplate);
			var engine = new RazorLightEngineBuilder()
			  .UseMemoryCachingProvider()
			  .Build();
			string templatekey = $"{emailTemplate.Subject}_{ emailTemplate.Type}";
			string body = "";
			if (emailTemplate.Body.Length != 0)
			{

				body = await engine.CompileRenderAsync(templatekey, emailTemplate.Body, model);
			}
			emailTemplateResponseModel.Body = body;
			return emailTemplateResponseModel;
		}
	}
}
