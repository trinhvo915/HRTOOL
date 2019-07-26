using Microsoft.EntityFrameworkCore;
using Orient.Base.Net.Core.Api.Core.Business.Models.Base;
using Orient.Base.Net.Core.Api.Core.Business.Models.Comments;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.Common.Reflections;
using Orient.Base.Net.Core.Api.Core.Common.Utilities;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Services
{
    public interface ICommentService
    {
        Task<PagedList<CommentViewModel>> GetAllCommentByJob(Guid jobId, CommentRequestListViewModel commentRequestListViewModel);

        Task<ResponseModel> CreateCommentAsync(Guid userId, Guid jobId, CommentManageModel commentManageModel);

        Task<ResponseModel> UpdateCommentAsync(Guid userId, Guid jobId, Guid commentId, CommentManageModel commentManageModel);

        Task<ResponseModel> DeleteCommentAsync(Guid userId, Guid jobId, Guid commentId);
    }
    public class CommentService : ICommentService
    {
        #region Fields

        private readonly IRepository<Comment> _commentResponstory;

        #endregion

        #region Constructor

        public CommentService(IRepository<Comment> commentRepository)
        {
            _commentResponstory = commentRepository;
        }

        #endregion

        #region Private Methods

        private IQueryable<Comment> GetAll()
        {
            return _commentResponstory.GetAll()
                .Include(c => c.User)
                .Include(c => c.Job);
        }

        private List<string> GetAllPropertyNameOfCommentViewModel()
        {
            var commentViewModel = new CommentViewModel();

            var type = commentViewModel.GetType();

            return ReflectionUtilities.GetAllPropertyNamesOfType(type);
        }

        #endregion

        #region Public Methods

        public async Task<PagedList<CommentViewModel>> GetAllCommentByJob(Guid jobId, CommentRequestListViewModel commentRequestListViewModel)
        {
            var list = await GetAll().Where(x => (!commentRequestListViewModel.IsActive.HasValue || x.RecordActive == commentRequestListViewModel.IsActive)
                    && (x.JobId == jobId)
                    && (string.IsNullOrEmpty(commentRequestListViewModel.Query) || (x.Content.Contains(commentRequestListViewModel.Query)))
            ).Select(x => new CommentViewModel(x)).ToListAsync();

            var commentViewModelProperties = GetAllPropertyNameOfCommentViewModel();

            var requestPropertyName = !string.IsNullOrEmpty(commentRequestListViewModel.SortName) ? commentRequestListViewModel.SortName.ToLower() : string.Empty;

            var matchedPropertyName = commentViewModelProperties.FirstOrDefault(x => x.ToLower() == requestPropertyName);

            if (string.IsNullOrEmpty(matchedPropertyName))
            {
                matchedPropertyName = "DateComment";
            }

            var type = typeof(CommentViewModel);

            var sortProperty = type.GetProperty(matchedPropertyName);

            list = commentRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();

            return new PagedList<CommentViewModel>(list, commentRequestListViewModel.Skip ?? CommonConstants.Config.DEFAULT_SKIP, commentRequestListViewModel.Take ?? CommonConstants.Config.DEFAULT_TAKE);
        }

        public async Task<ResponseModel> CreateCommentAsync(Guid userId, Guid jobId, CommentManageModel commentManageModel)
        {
            if (userId == null)
            {
                return new ResponseModel()
                {
                    Message = MessageConstants.INVALID_ACCESS_TOKEN,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }
            else
            {
                var comment = new Comment()
                {
                    UserId = userId,
                    JobId = jobId,
                    Content = StringExtension.CustomString(commentManageModel.Content)
                };

                await _commentResponstory.InsertAsync(comment);

                comment = await GetAll().FirstOrDefaultAsync(x => x.Id == comment.Id);
                return new ResponseModel()
                {
                    Data = new CommentViewModel(comment),
                    Message = MessageConstants.CREATED_SUCCESSFULLY,
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
        }

        public async Task<ResponseModel> UpdateCommentAsync(Guid userId, Guid jobId, Guid commentId, CommentManageModel commentManageModel)
        {
            if (userId == null)
            {
                return new ResponseModel()
                {
                    Message = MessageConstants.INVALID_ACCESS_TOKEN,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }
            else
            {
                var comment = await GetAll().FirstOrDefaultAsync(x => x.Id == commentId && x.JobId == jobId && x.UserId == userId);

                if (comment == null)
                {
                    return new ResponseModel()
                    {
                        Message = MessageConstants.INVALID_ACCESS_TOKEN,
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };
                }

                commentManageModel.SetDataToModel(comment);

                await _commentResponstory.UpdateAsync(comment);

                return new ResponseModel()
                {
                    Data = new CommentViewModel(comment),
                    Message = MessageConstants.CREATED_SUCCESSFULLY,
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
        }

        public async Task<ResponseModel> DeleteCommentAsync(Guid userId, Guid jobId, Guid commentId)
        {
            if (userId == null)
            {
                return new ResponseModel()
                {
                    Message = MessageConstants.INVALID_ACCESS_TOKEN,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }
            else
            {
                var comment = await GetAll().FirstOrDefaultAsync(x => x.Id == commentId && x.JobId == jobId && x.UserId == userId);

                if (comment == null)
                {
                    return new ResponseModel()
                    {
                        Message = MessageConstants.INVALID_ACCESS_TOKEN,
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };
                }

                await _commentResponstory.DeleteAsync(comment);

                return new ResponseModel()
                {
                    Data = new CommentViewModel(comment),
                    Message = MessageConstants.CREATED_SUCCESSFULLY,
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
        }

        #endregion Public Methods
    }
}
