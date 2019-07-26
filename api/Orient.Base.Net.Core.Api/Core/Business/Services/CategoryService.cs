using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Orient.Base.Net.Core.Api.Core.Business.Models.Categories;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Business.Models.Base;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Common.Reflections;
using Orient.Base.Net.Core.Api.Core.Common.Constants;

namespace Orient.RMS.Api.Core.Business.Services
{
    public interface ICategoryService
    {
        Task<PagedList<CategoryViewModel>> ListCategoryAsync(CategoryRequestListViewModel categoryRequestListViewModel);

        Task<List<CategoryViewModel>> GetAllCategoryAsync(CategoryRequestGetAllModel categoryRequestGetAllModel);

        Task<ResponseModel> CreateCategoryAsync(CategoryManageModel categoryManageModel);

        Task<ResponseModel> UpdateCategoryAsync(Guid id, CategoryManageModel categoryManageModel);

        Task<ResponseModel> GetCategoryByIdAsync(Guid? id);

        Task<ResponseModel> DeleteCategoryAsync(Guid id);
    }

    public class CategoryService : ICategoryService
    {
        #region Fields

        private readonly IRepository<Category> _categoryResponstory;

        #endregion

        #region Constructor

        public CategoryService(IRepository<Category> categoryResponstory)
        {
            _categoryResponstory = categoryResponstory;
        }

        #endregion

        #region Base Methods

        #endregion

        #region Private Methods

        private IQueryable<Category> GetAll()
        {
            return _categoryResponstory.GetAll()
                    .Where(x => !x.RecordDeleted);
        }

        private List<string> GetAllPropertyNameOfCategoryViewModel()
        {
            var categoryViewModel = new CategoryViewModel();

            var type = categoryViewModel.GetType();

            return ReflectionUtilities.GetAllPropertyNamesOfType(type);
        }

        #endregion

        public async Task<PagedList<CategoryViewModel>> ListCategoryAsync(CategoryRequestListViewModel categoryRequestListViewModel)
        {
            var list = await GetAll()
                .Where(x => (!categoryRequestListViewModel.IsActive.HasValue || x.RecordActive == categoryRequestListViewModel.IsActive)
                    && (string.IsNullOrEmpty(categoryRequestListViewModel.Query)
                    || (x.Name.Contains(categoryRequestListViewModel.Query)
                )))
            .Select(x => new CategoryViewModel(x)).ToListAsync();

            var categoryViewModelProperties = GetAllPropertyNameOfCategoryViewModel();

            var requestPropertyName = !string.IsNullOrEmpty(categoryRequestListViewModel.SortName) ? categoryRequestListViewModel.SortName.ToLower() : string.Empty;

            string matchedPropertyName = string.Empty;

            foreach (var categoryViewModelProperty in categoryViewModelProperties)
            {
                var lowerTypeViewModelProperty = categoryViewModelProperty.ToLower();
                if (lowerTypeViewModelProperty.Equals(requestPropertyName))
                {
                    matchedPropertyName = categoryViewModelProperty;
                    break;
                }
            }

            if (string.IsNullOrEmpty(matchedPropertyName))
            {
                matchedPropertyName = "Name";
            }

            var type = typeof(CategoryViewModel);

            var sortProperty = type.GetProperty(matchedPropertyName);

            list = categoryRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();

            return new PagedList<CategoryViewModel>(list, categoryRequestListViewModel.Skip ?? CommonConstants.Config.DEFAULT_SKIP, categoryRequestListViewModel.Take ?? CommonConstants.Config.DEFAULT_TAKE);
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync(CategoryRequestGetAllModel categoryRequestGetAllModel)
        {
            var list = await GetAll()
                .Where(x => (!categoryRequestGetAllModel.IsActive.HasValue || x.RecordActive == categoryRequestGetAllModel.IsActive)
                    && (string.IsNullOrEmpty(categoryRequestGetAllModel.Query)
                    || (x.Name.Contains(categoryRequestGetAllModel.Query)
                )))
            .Select(x => new CategoryViewModel(x)).ToListAsync();

            return list;
        }

        public async Task<ResponseModel> CreateCategoryAsync(CategoryManageModel categoryManageModel)
        {
            var category = await _categoryResponstory.FetchFirstAsync(x => x.Name == categoryManageModel.Name);
            if (category != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = MessageConstants.EXISTED_CREATED
                };
            }
            else
            {
                category = AutoMapper.Mapper.Map<Category>(categoryManageModel);
                await _categoryResponstory.InsertAsync(category);

                category = await GetAll().FirstOrDefaultAsync(x => x.Id == category.Id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new CategoryViewModel(category),
                    Message = MessageConstants.CREATED_SUCCESSFULLY
                };
            }
        }

        public async Task<ResponseModel> UpdateCategoryAsync(Guid id, CategoryManageModel categoryManageModel)
        {
            var category = await _categoryResponstory.GetByIdAsync(id);
            if (category == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
            else
            {
                var existedCategory = await _categoryResponstory.FetchFirstAsync(x => x.Name == categoryManageModel.Name && x.Id != id);
                if (existedCategory != null)
                {
                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = MessageConstants.EXISTED_CREATED
                    };
                }
                else
                {
                    categoryManageModel.SetDateToModel(category);

                    await _categoryResponstory.UpdateAsync(category);

                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Data = new CategoryViewModel(category),
                        Message = MessageConstants.UPDATED_SUCCESSFULLY
                    };
                }
            }
        }

        public async Task<ResponseModel> GetCategoryByIdAsync(Guid? id)
        {
            var category = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (category != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new CategoryViewModel(category)
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

        public async Task<ResponseModel> DeleteCategoryAsync(Guid id)
        {
            var category = _categoryResponstory.FetchFirstAsync(x => x.Id == id);
            if (category == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
            else
            {
                await _categoryResponstory.DeleteAsync(id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
        }
    }
}
