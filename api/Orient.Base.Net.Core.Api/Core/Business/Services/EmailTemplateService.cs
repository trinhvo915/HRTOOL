using Microsoft.EntityFrameworkCore;
using Orient.Base.Net.Core.Api.Core.Business.Models.Base;
using Orient.Base.Net.Core.Api.Core.Business.Models.EmailTemplates;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.Common.Reflections;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Services
{
    public interface IEmailTemplateService
    {
        Task<PagedList<EmailTemplateViewModel>> ListEmailTemplateAsync(EmailTemplateRequestListViewModel emailTemplateRequestListViewModel);
        Task<ResponseModel> UpdateEmailTemplateAsync(Guid id, EmailTemplateManageModel emailTemplateManageModel);

    }
    public class EmailTemplateService : IEmailTemplateService
    {
        public readonly IRepository<EmailTemplate> _emailTemplateRepository;

        public EmailTemplateService(IRepository<EmailTemplate> emailTemplateRepository)
        {
            _emailTemplateRepository = emailTemplateRepository;

        }

        #region Private Methods

        private IQueryable<EmailTemplate> GetAll()
        {
            return _emailTemplateRepository.GetAll()
                .Include(x => x.TemplateAttachments)
                .Where(x => !x.RecordDeleted);
        }

        private List<string> GetAllPropertyNameOfEmailTemplateViewModel()
        {
            var emailTemplateViewModel = new EmailTemplateViewModel();
            var type = emailTemplateViewModel.GetType();
            return ReflectionUtilities.GetAllPropertyNamesOfType(type);
        }

        #endregion

        public async Task<PagedList<EmailTemplateViewModel>> ListEmailTemplateAsync(EmailTemplateRequestListViewModel emailTemplateRequestListViewModel)
        {
            var list = await GetAll()
                    .Where(x => (!emailTemplateRequestListViewModel.IsActive.HasValue || x.RecordActive == emailTemplateRequestListViewModel.IsActive)
                     && (string.IsNullOrEmpty(emailTemplateRequestListViewModel.Query)
                     || (x.Name.Contains(emailTemplateRequestListViewModel.Query))
                     ))
                    .Select(x => new EmailTemplateViewModel(x)).ToListAsync();

            var interviewViewModelProperties = GetAllPropertyNameOfEmailTemplateViewModel();
            var requestPropertyName = !string.IsNullOrEmpty(emailTemplateRequestListViewModel.SortName) ? emailTemplateRequestListViewModel.SortName.ToLower() : string.Empty;
            string matchedPropertyName = interviewViewModelProperties.FirstOrDefault(x => x.ToLower() == requestPropertyName);

            if (string.IsNullOrEmpty(matchedPropertyName))
            {
                matchedPropertyName = "Name";
            }

            var type = typeof(EmailTemplateViewModel);
            var sortProperty = type.GetProperty(matchedPropertyName);
            list = emailTemplateRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();

            return new PagedList<EmailTemplateViewModel>(list, emailTemplateRequestListViewModel.Skip ?? CommonConstants.Config.DEFAULT_SKIP, emailTemplateRequestListViewModel.Take ?? CommonConstants.Config.DEFAULT_TAKE);

        }

        public async Task<ResponseModel> UpdateEmailTemplateAsync(Guid id, EmailTemplateManageModel emailTemplateManageModel)
        {
            var emailTemplate = await GetAll().FirstOrDefaultAsync(x => x.Id == id);

            if (emailTemplate == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
            else
            {
                emailTemplateManageModel.SetDataToModel(emailTemplate);

                var templateAttachments = new List<TemplateAttachment>();
                foreach (var templateAttachmentsManageModel in emailTemplateManageModel.TemplateAttachments)
                {
                    var templateAttachment = new TemplateAttachment();
                    templateAttachmentsManageModel.SetDataToModel(templateAttachment);
                    templateAttachments.Add(templateAttachment);
                }
                emailTemplate.TemplateAttachments = templateAttachments;

                await _emailTemplateRepository.UpdateAsync(emailTemplate);

                emailTemplate = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
                return new ResponseModel
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new EmailTemplateViewModel(emailTemplate),
                    Message = MessageConstants.UPDATED_SUCCESSFULLY
                };
            }
        }
    }
}
