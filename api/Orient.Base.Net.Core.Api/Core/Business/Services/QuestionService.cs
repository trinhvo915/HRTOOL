using Microsoft.EntityFrameworkCore;
using Orient.Base.Net.Core.Api.Core.Business.Models.Base;
using Orient.Base.Net.Core.Api.Core.Business.Models.Questions;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.Common.Reflections;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Services
{
    public interface IQuestionService
    {
        Task<PagedList<QuestionViewModel>> ListQuestionAsync(QuestionRequestListViewModel questionRequestListViewModel);

        Task<ResponseModel> GetQuestionByIdAsync(Guid? id);

        Task<ResponseModel> CreateQuestionAndAnswerAsync(QuestionAndAnswerManageModel questionAndAnswerManageModel);

        Task<ResponseModel> UpdateQuestionAsync(Guid id, QuestionManageModel questionManageModel);

        Task<ResponseModel> DeleteQuestionAsync(Guid id);

        Task<ResponseModel> UpdateQuestionStatusAsync(Guid id, QuestionStatusManageModel questionStatusManageModel);

        Task<List<ResponseModel>> GenerateInterviewQuestionPDF(QuestionGenerateRequestModel questionGenerateRequestModel);


    }
    public class QuestionService : IQuestionService
    {
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Answer> _answerRepository;
        private readonly IPDFService _pdfService;

        public QuestionService(IRepository<Question> questionRepository, IRepository<Answer> answerRepository, IPDFService pdfService)
        {
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _pdfService = pdfService;
        }

        private IQueryable<Question> GetAll()
        {
            return _questionRepository.GetAll()
                .Include(x => x.Answers);
        }

        private List<string> GetAllPropertyNameOfQuestionViewModel()
        {
            var questionViewModel = new QuestionViewModel();

            var type = questionViewModel.GetType();

            return ReflectionUtilities.GetAllPropertyNamesOfType(type);
        }

        public async Task<PagedList<QuestionViewModel>> ListQuestionAsync(QuestionRequestListViewModel questionRequestListViewModel)
        {
            var list = await GetAll()
                .Where(x => (!questionRequestListViewModel.IsActive.HasValue || x.RecordActive == questionRequestListViewModel.IsActive)
                && (string.IsNullOrEmpty(questionRequestListViewModel.Query)
                || (x.Content.Contains(questionRequestListViewModel.Query)
                ))
                )
                .Select(x => new QuestionViewModel(x)).ToListAsync();
            var questionViewModelProperties = GetAllPropertyNameOfQuestionViewModel();

            var requestPropertyName = !string.IsNullOrEmpty(questionRequestListViewModel.SortName) ? questionRequestListViewModel.SortName.ToLower() : string.Empty;

            var matchedPropertyName = questionViewModelProperties.FirstOrDefault(x => x.ToLower() == requestPropertyName);

            if (string.IsNullOrEmpty(matchedPropertyName))
            {
                matchedPropertyName = "Content";
            }

            var type = typeof(QuestionViewModel);

            var sortProperty = type.GetProperty(matchedPropertyName);

            list = questionRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();

            return new PagedList<QuestionViewModel>(list, questionRequestListViewModel.Skip ?? CommonConstants.Config.DEFAULT_SKIP, questionRequestListViewModel.Take ?? CommonConstants.Config.DEFAULT_TAKE);

        }

        public async Task<ResponseModel> GetQuestionByIdAsync(Guid? id)
        {
            var question = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (question != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new QuestionViewModel(question)
                };
            }
            else
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
        }

        public async Task<ResponseModel> UpdateQuestionAsync(Guid id, QuestionManageModel questionManageModel)
        {
            var question = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (question == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
            else
            {
                var exitstedQuestion = await _questionRepository.FetchFirstAsync(x => x.Content == questionManageModel.Question && x.Id != id);
                if (exitstedQuestion != null)
                {
                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = MessageConstants.EXISTED_CREATED
                    };
                }
                else
                {
                    questionManageModel.SetDataToModel(question);
                    await _questionRepository.UpdateAsync(question);

                    var answer = await _answerRepository.GetAll().FirstOrDefaultAsync(x => x.Id == questionManageModel.AnswerId);
                    if (question.Answers.Contains(answer))
                    {
                        var rejectAnswers = await _answerRepository.GetAll().Where(x => x.QuestionId == id).ToListAsync();
                        foreach (var rejectAnswer in rejectAnswers)
                        {
                            rejectAnswer.Status = AnswerEnums.Status.Rejected;
                        }
                        answer.Status = AnswerEnums.Status.Approved;
                        await _answerRepository.UpdateAsync(answer);
                    }
                   
                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Data = new QuestionViewModel(question),
                        Message = MessageConstants.UPDATED_SUCCESSFULLY
                    };
                }

            }
        }

        public async Task<ResponseModel> DeleteQuestionAsync(Guid id)
        {
            var question = await _questionRepository.GetByIdAsync(id);

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
                await _questionRepository.DeleteAsync(id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
        }

        public async Task<ResponseModel> CreateQuestionAndAnswerAsync(QuestionAndAnswerManageModel questionAndAnswerManageModel)
        {
            var question = new Question
            {
                Content = questionAndAnswerManageModel.Question,
                Type = questionAndAnswerManageModel.Type,
                Level = questionAndAnswerManageModel.Level
            };

            var answer = new Answer
            {
                Content = questionAndAnswerManageModel.Answer,
                QuestionId = question.Id
            };

            await _questionRepository.InsertAsync(question);
            await _answerRepository.InsertAsync(answer);
            return new ResponseModel()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = MessageConstants.CREATED_SUCCESSFULLY
            };
        }

        public async Task<ResponseModel> UpdateQuestionStatusAsync(Guid id, QuestionStatusManageModel questionStatusManageModel)
        {
            var question = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (question == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
            else
            {
                questionStatusManageModel.SetDataToModel(question);
                await _questionRepository.UpdateAsync(question);

                if(questionStatusManageModel.Status == QuestionEnums.Status.Approved && question.Answers.Count() == 1)
                {
                    var answer = question.Answers.FirstOrDefault();
                    answer.Status = AnswerEnums.Status.Approved;
                    await _answerRepository.UpdateAsync(answer);
                }

                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new QuestionViewModel(question),
                    Message = MessageConstants.UPDATED_SUCCESSFULLY
                };
            }
        }

        public async Task<List<ResponseModel>> GenerateInterviewQuestionPDF(QuestionGenerateRequestModel questionGenerateRequestModel)
        {
            var responseModels = new List<ResponseModel>();
            foreach(var type in questionGenerateRequestModel.Types)
            {
                var list = await GetAll()
                .Where(x => x.Level == questionGenerateRequestModel.Level
                    && x.Type == type
                    && x.Status == QuestionEnums.Status.Approved
                    && x.Answers.Any(y => y.Status == AnswerEnums.Status.Approved))
                .Select(x => new QuestionAnswerPDFModel(x))
                .ToListAsync();

                var quantity = questionGenerateRequestModel.Quantity ?? CommonConstants.Config.DEFAULT_TAKE;

                var randomQuestionList = list.OrderBy(x => Guid.NewGuid()).Take(quantity).ToList();

                string templatePath = $"Core\\Business\\Resources\\PDFTemplates\\QuestionListTemplate.cshtml";

                string fileName = "QuestionInterview_"
                    + questionGenerateRequestModel.Level
                    + type + "_"
                    + quantity.ToString() + "Questions" + "_"
                    + DateTime.Now.ToString("MM-dd-yyyy_hh-mm-ss")
                    + ".pdf";
                var responseModel = await _pdfService.CreatePDFAsync(randomQuestionList, templatePath, fileName);
                responseModels.Add(responseModel);
            }

            return responseModels;
        }
    }
}
