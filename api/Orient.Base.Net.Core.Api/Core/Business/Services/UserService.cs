using System;
using System.Linq;
using Orient.Base.Net.Core.Api.Core.Business.Models.Users;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Orient.Base.Net.Core.Api.Core.Business.Models.Base;
using System.Collections.Generic;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Common.Utilities;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.Business.Models;
using System.Threading.Tasks;
using Orient.Base.Net.Core.Api.Core.Common.Reflections;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using Newtonsoft.Json;

namespace Orient.Base.Net.Core.Api.Core.Business.Services
{
	public interface IUserService
	{
		Task<IEnumerable<UserViewModel>> GetAllUserAsync(BaseRequestGetAllViewModel baseRequestGetAllViewModel);

        Task<List<UserTreeJsonModel>> GetHRTree(BaseRequestGetAllViewModel baseRequestGetAllViewModel);

        //Task<IEnumerable<UserViewModel>> GetAllUserInterviewerAsync(BaseRequestGetAllViewModel baseRequestGetAllViewModel);

        Task<PagedList<UserViewModel>> ListUserAsync(UserRequestListViewModel userRequestListViewModel);

		Task<UserViewModel> GetUserByIdAsync(Guid? id);

		Task<ResponseModel> RegisterAsync(UserRegisterModel userRegisterModel);

		Task<ResponseModel> UpdateProfileAsync(Guid id, UserUpdateProfileModel userUpdateProfileModel);

		Task<ResponseModel> UpdateUserAsync(Guid id, UserManageModel userColorManageModel);

		Task<ResponseModel> DeleteUserAsync(Guid id);

		Task<User> GetByEmailAsync(string email);

		Task<User> GetByIdAsync(Guid? id);
	}

	public class UserService : IUserService
	{
		#region Fields

		private readonly IRepository<User> _userRepository;
		private readonly ILogger _logger;
		private readonly IOptions<AppSettings> _appSetting;
		private readonly IRepository<UserInRole> _userInRoleRepository;
        private readonly IRepository<Department> _departmentRepository;

        #endregion

        #region Constructor

        public UserService(IRepository<User> userRepository, ILogger<UserService> logger,
			IOptions<AppSettings> appSetting, IRepository<UserInRole> userInRoleRepository, 
            IRepository<Department> departmentRepository)
		{
			_userRepository = userRepository;
			_logger = logger;
			_appSetting = appSetting;
			_userInRoleRepository = userInRoleRepository;
            _departmentRepository = departmentRepository;

        }

		#endregion

		#region Base Methods

		#endregion

		#region Private Methods

		private IQueryable<User> GetAll()
		{
			return _userRepository.GetAll()
						.Include(x => x.UserInRoles)
							.ThenInclude(user => user.Role)
                        .Include(x =>x.Department)
					.Where(x => !x.RecordDeleted);
		}

		private List<string> GetAllPropertyNameOfUserViewModel()
		{
			var userViewModel = new UserViewModel();
			var type = userViewModel.GetType();

			return ReflectionUtilities.GetAllPropertyNamesOfType(type);
		}

        private List<UserTreeJsonModel> MapToTreeModelJsonCollection(List<UserTreeModel> source)
        {
            // map all items
            var allItems = source.Select(e => new UserTreeJsonModel(e)).ToList();

            // build tree structure
            foreach (var item in allItems)
            {
                var children = allItems.Where(e => e.ParentId == item.DepartmentId).ToList();
                if (children.Any())
                {
                    item.Children = children;
                }
            }
            return allItems.Where(e => e.ParentId == null).ToList();
        }

        #endregion

        #region Other Methods

        public async Task<IEnumerable<UserViewModel>> GetAllUserAsync(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
		{
			var list = await GetAll()
			   .Where(x => (string.IsNullOrEmpty(baseRequestGetAllViewModel.Query)
				   || (x.Name.Contains(baseRequestGetAllViewModel.Query))
				   || (x.Email.Contains(baseRequestGetAllViewModel.Query))
			   ) && x.UserInRoles.Any(y=> y.RoleId != RoleConstants.SuperAdminId))
			   .OrderBy(x => x.Name)
			   .Select(x => new UserViewModel(x))
			   .ToListAsync();

			return list;
		}

        public async Task<List<UserTreeJsonModel>> GetHRTree(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
        {
            var list = await GetAll()
               .Where(x => (x.DepartmentId != null)&&(string.IsNullOrEmpty(baseRequestGetAllViewModel.Query)
                   || (x.Name.Contains(baseRequestGetAllViewModel.Query))
                   || (x.Email.Contains(baseRequestGetAllViewModel.Query))
               ) && x.UserInRoles.Any(y => y.RoleId != RoleConstants.SuperAdminId))
               .OrderBy(x => x.Name)
               .Select(x => new UserTreeModel(x))
               .ToListAsync();

            return MapToTreeModelJsonCollection(list);
        }

        public async Task<PagedList<UserViewModel>> ListUserAsync(UserRequestListViewModel userRequestListViewModel)
		{
			var list = await GetAll()
			.Where(x => (!userRequestListViewModel.IsActive.HasValue || x.RecordActive == userRequestListViewModel.IsActive)
				&& (string.IsNullOrEmpty(userRequestListViewModel.Query)
					|| (x.Name.Contains(userRequestListViewModel.Query)
					|| (x.Email.Contains(userRequestListViewModel.Query)
					))) && x.UserInRoles.Any(y => y.RoleId != RoleConstants.SuperAdminId))
				.Select(x => new UserViewModel(x)).ToListAsync();

			var userViewModelProperties = GetAllPropertyNameOfUserViewModel();
			var requestPropertyName = !string.IsNullOrEmpty(userRequestListViewModel.SortName) ? userRequestListViewModel.SortName.ToLower() : string.Empty;
			string matchedPropertyName = userViewModelProperties.FirstOrDefault(x => x.ToLower() == requestPropertyName);

			if (string.IsNullOrEmpty(matchedPropertyName))
			{
				matchedPropertyName = "Name";
			}

			var type = typeof(UserViewModel);
			var sortProperty = type.GetProperty(matchedPropertyName);

			list = userRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();

			return new PagedList<UserViewModel>(list, userRequestListViewModel.Skip ?? CommonConstants.Config.DEFAULT_SKIP, userRequestListViewModel.Take ?? CommonConstants.Config.DEFAULT_TAKE);
		}

		//public async Task<IEnumerable<UserViewModel>> GetAllUserInterviewerAsync(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
		//{
		//	var list = await GetAll()
		//	 .Where(x => (string.IsNullOrEmpty(baseRequestGetAllViewModel.Query)
		//			 || (x.Name.Contains(baseRequestGetAllViewModel.Query)
		//			 || (x.Email.Contains(baseRequestGetAllViewModel.Query))
		//			 ))
		//			 && (x.UserInRoles.Any(y => y.RoleId == RoleConstants.NormalUserId)))
		//	  .OrderBy(x => x.Name)
		//	  .Select(x => new UserViewModel(x))
		//	  .ToListAsync();

		//	return list;
		//}

		public async Task<UserViewModel> GetUserByIdAsync(Guid? id)
		{
			var user = await GetAll()
				.FirstOrDefaultAsync(x => x.Id == id);
			if (user == null)
			{
				return null;
			}
			else
			{
				return new UserViewModel(user);
			}
		}

		public async Task<ResponseModel> RegisterAsync(UserRegisterModel userRegisterModel)
		{
			var user = await _userRepository.FetchFirstAsync(x => x.Mobile == userRegisterModel.Email);
			if (user != null)
			{
				return new ResponseModel()
				{
					StatusCode = System.Net.HttpStatusCode.BadRequest,
					Message = "Số điện thoại đã được đăng kí. Sử dụng chức năng quên mật khẩu để lấy lại!"
				};
			}
			else
			{
				user = AutoMapper.Mapper.Map<User>(userRegisterModel);
				userRegisterModel.Password.GeneratePassword(out string saltKey, out string hashPass);

                var userInRoles = new List<UserInRole>();
                userInRoles.Add(new UserInRole { RoleId = RoleConstants.NormalUserId });

                user.UserInRoles = userInRoles;
                user.Password = hashPass;
				user.PasswordSalt = saltKey;

				return await _userRepository.InsertAsync(user);
			}
		}

		public async Task<ResponseModel> UpdateProfileAsync(Guid id, UserUpdateProfileModel userUpdateProfileModel)
		{
			var user = await _userRepository.GetByIdAsync(id);
			if (user == null)
			{
				return new ResponseModel()
				{
					StatusCode = System.Net.HttpStatusCode.NotFound,
					Message = "User không tồn tại trong hệ thống. Vui lòng kiểm tra lại!"
				};
			}
			else
			{
				user = userUpdateProfileModel.GetUserFromModel(user);
				return await _userRepository.UpdateAsync(user);
			}
		}

		public async Task<ResponseModel> UpdateUserAsync(Guid id, UserManageModel userManageModel)
		{
			var user = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
			if (user == null)
			{
				return new ResponseModel()
				{
					StatusCode = System.Net.HttpStatusCode.NotFound,
					Message = UserMessagesConstants.NOT_FOUND
				};
			}

			//Update Roles for User
			var Ids = _userInRoleRepository.GetAll().Where(x => x.UserId == user.Id).Select(y => y.Id).ToList();
			await _userInRoleRepository.DeleteAsync(Ids);

			List<UserInRole> userInRoles = new List<UserInRole>();
			foreach (var roleId in userManageModel.RoleIds)
			{
				userInRoles.Add(
					new UserInRole
					{
						UserId = user.Id,
						RoleId = roleId
					}
				);
			}
			await _userInRoleRepository.InsertAsync(userInRoles);

			//Update Color for User
			if (userManageModel.Color != UserConstants.Color
			   && GetAll().Any(x => x.Color == userManageModel.Color && x.Id != id))
			{
				return new ResponseModel()
				{
					StatusCode = System.Net.HttpStatusCode.BadRequest,
					Message = UserMessagesConstants.COLOR_EXISTED
				};
			}

			else
			{
				userManageModel.SetDataToModel(user);
				await _userRepository.UpdateAsync(user);

				return new ResponseModel()
				{
					StatusCode = System.Net.HttpStatusCode.OK,
					Data = new UserViewModel(user),
					Message = MessageConstants.UPDATED_SUCCESSFULLY
				};
			}
		}

		public async Task<ResponseModel> DeleteUserAsync(Guid id)
		{
			return await _userRepository.DeleteAsync(id);
		}

		public async Task<User> GetByEmailAsync(string email)
		{
			return await GetAll().FirstOrDefaultAsync(x => x.Email == email);
		}

		public async Task<User> GetByIdAsync(Guid? id)
		{
			return await GetAll().FirstOrDefaultAsync(x => x.Id == id);
		}

		#endregion
	}
}
