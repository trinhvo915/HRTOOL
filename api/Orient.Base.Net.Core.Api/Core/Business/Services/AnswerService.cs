using Microsoft.EntityFrameworkCore;
using Orient.Base.Net.Core.Api.Core.Business.Models.Answers;
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
    public interface IAnswerService
    {
        Task<ResponseModel> CreateAnswerAsync(AnswerManageModel answerManageModel);

        Task<ResponseModel> UpdateAnswerAsync(Guid id, AnswerManageModel answerManageModel);

        Task<ResponseModel> DeleteAnswerAsync(Guid id);
    }
    public class AnswerService : IAnswerService
    {
        #region Fields

        private readonly IRepository<Answer> _answerRepository;

        #endregion

        #region Constructor

        public AnswerService(IRepository<Answer> answerRepository)
        {
            _answerRepository = answerRepository;
        }

        #endregion

        #region Base Methods

        #endregion

        #region Private Methods

        private IQueryable<Answer> GetAll()
        {
            return _answerRepository.GetAll()
                    .Include(x => x.Question);
        }

        #endregion

        #region Public Methods

        public async Task<ResponseModel> CreateAnswerAsync(AnswerManageModel answerManageModel)
        {
            var answer = AutoMapper.Mapper.Map<Answer>(answerManageModel);
            await _answerRepository.InsertAsync(answer);

            return new ResponseModel()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = MessageConstants.CREATED_SUCCESSFULLY
            };
        }

        public async Task<ResponseModel> UpdateAnswerAsync(Guid id, AnswerManageModel answerManageModel)
        {
            var answer = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (answer == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
            else
            {
                var existedAnswer = await _answerRepository.FetchFirstAsync(x => x.Content == answerManageModel.Content);
                if (existedAnswer != null)
                {
                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = MessageConstants.EXISTED_CREATED
                    };
                }
                else
                {
                    answerManageModel.SetDataToModel(answer);

                    await _answerRepository.UpdateAsync(answer);

                    // return answer
                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Data = new AnswerViewModel(answer),
                        Message = MessageConstants.UPDATED_SUCCESSFULLY
                    };
                }
            }
        }

        public async Task<ResponseModel> DeleteAnswerAsync(Guid id)
        {
            var question = await _answerRepository.GetByIdAsync(id);

            if (question == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
            else
            {
                await _answerRepository.DeleteAsync(id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
        }

        #endregion Public Methods
    }
}
