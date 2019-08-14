using Microsoft.EntityFrameworkCore;
using Orient.Base.Net.Core.Api.Core.Business.Models.Base;
using Orient.Base.Net.Core.Api.Core.Business.Models.Departments;
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
    public interface IDepartmentService
    {
        Task<ResponseModel> CreateDepartmentAsync(DepartmentManageModel departmentManageModel);

        Task<PagedList<DepartmentViewModel>> GetAllDepartmentAsync(DepartmentRequestListViewModel departmentRequestListViewModel);

        Task<ResponseModel> UpdateDepartmentAsync(Guid id, DepartmentManageModel departmentManageModel);
        Task<ResponseModel> DeleteDepartmentAsync(Guid id);
    }
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepository<Department> _departmentResponstory;

        public DepartmentService(IRepository<Department> departmentResponstory)
        {
            _departmentResponstory = departmentResponstory;
        }

        private IQueryable<Department> GetAll()
        {
            return _departmentResponstory.GetAll().Where(x => !x.RecordDeleted);
        }

        private List<string> GetAllPropertyNameOfDepartmentViewModel()
        {
            var departmentViewModel = new DepartmentViewModel();

            var type = departmentViewModel.GetType();

            return ReflectionUtilities.GetAllPropertyNamesOfType(type);
        }

        public async Task<ResponseModel> CreateDepartmentAsync(DepartmentManageModel departmentManageModel)
        {
            var department = await _departmentResponstory.FetchFirstAsync(x => x.Name == departmentManageModel.Name);
            if(department != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = MessageConstants.EXISTED_CREATED
                };
            }else
            {
                department = AutoMapper.Mapper.Map<Department>(departmentManageModel);
                await _departmentResponstory.InsertAsync(department);
                department = await GetAll().FirstOrDefaultAsync(x => x.Id == department.Id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new DepartmentViewModel(department),
                    Message = MessageConstants.CREATED_SUCCESSFULLY
                };
            }
        }

        public async Task<PagedList<DepartmentViewModel>> GetAllDepartmentAsync(DepartmentRequestListViewModel departmentRequestListViewModel)
        {
            //var list = await GetAll().Select(x => new DepartmentViewModel(x)).ToListAsync();
            //return list;
            var list = await GetAll()
                .Where(x => (!departmentRequestListViewModel.IsActive.HasValue || x.RecordActive == departmentRequestListViewModel.IsActive)
                    && (string.IsNullOrEmpty(departmentRequestListViewModel.Query)
                    || (x.Name.Contains(departmentRequestListViewModel.Query)
                )))
            .Select(x => new DepartmentViewModel(x)).ToListAsync();

            var departmentViewModelProperties = GetAllPropertyNameOfDepartmentViewModel();

            var requestPropertyName = !string.IsNullOrEmpty(departmentRequestListViewModel.SortName) ? departmentRequestListViewModel.SortName.ToLower() : string.Empty;

            string matchedPropertyName = string.Empty;

            foreach (var departmentViewModelProperty in departmentViewModelProperties)
            {
                var lowerTypeViewModelProperty = departmentViewModelProperty.ToLower();
                if (lowerTypeViewModelProperty.Equals(requestPropertyName))
                {
                    matchedPropertyName = departmentViewModelProperty;
                    break;
                }
            }

            if (string.IsNullOrEmpty(matchedPropertyName))
            {
                matchedPropertyName = "Name";
            }

            var type = typeof(DepartmentViewModel);

            var sortProperty = type.GetProperty(matchedPropertyName);

            list = departmentRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();

            return new PagedList<DepartmentViewModel>(list, departmentRequestListViewModel.Skip ?? CommonConstants.Config.DEFAULT_SKIP, departmentRequestListViewModel.Take ?? CommonConstants.Config.DEFAULT_TAKE);
        }

        public async Task<ResponseModel> UpdateDepartmentAsync(Guid id, DepartmentManageModel departmentManageModel)
        {
            var department = await _departmentResponstory.GetByIdAsync(id);
            if (department == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
            else
            {
                var existeddepartment = await _departmentResponstory.FetchFirstAsync(x => x.Name == departmentManageModel.Name && x.Id != id);
                if (existeddepartment != null)
                {
                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = MessageConstants.EXISTED_CREATED
                    };
                }
                else
                {
                    departmentManageModel.SetDateToModel(department);

                    await _departmentResponstory.UpdateAsync(department);

                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Data = new DepartmentViewModel(department),
                        Message = MessageConstants.UPDATED_SUCCESSFULLY
                    };
                }
            }
        }

        public async Task<ResponseModel> DeleteDepartmentAsync(Guid id)
        {
            var department = _departmentResponstory.FetchFirstAsync(x => x.Id == id);
            if (department == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
            else
            {
                await _departmentResponstory.DeleteAsync(id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
        }
    }
}
