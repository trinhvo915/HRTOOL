using Orient.Base.Net.Core.Api.Core.Business.Models.Users;
using Orient.Base.Net.Core.Api.Core.Common.Utilities;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using System.Linq;
using System.Threading.Tasks;
using Orient.Base.Net.Core.Api.Core.Common.Helpers;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.Business.Models;
using Microsoft.Extensions.Options;

namespace Orient.Base.Net.Core.Api.Core.Business.Services
{
    public interface ISSOAuthService
    {
        Task<ResponseModel> LoginAsync(UserLoginModel userLoginModel);

        ResponseModel VerifyTokenAsync(string token);
    }

    public class SSOAuthService : ISSOAuthService
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authService;
        private readonly AppSettings _config;

        public SSOAuthService(IUserService userService, IAuthenticationService authenticationService, IOptions<AppSettings> config)
        {
            _userService = userService;
            _authService = authenticationService;
            _config = config.Value;
        }

        #region Private Methods

        private string LoginDirectory(string email, string password)
        {
            if (_authService.CheckUser(_config.Admin, _config.Password))
            {
                return _authService.GetUserInfo(email, password);
            }
            return string.Empty;
        }

        #endregion

        #region Public Methods

        public async Task<ResponseModel> LoginAsync(UserLoginModel userLoginModel)
        {
            var user = await _userService.GetByEmailAsync(userLoginModel.Email);

            if (user != null)
            {
                var result = false;
                var roleIds = user.UserInRoles.Select(x => x.RoleId).ToList();

                result = PasswordUtilities.ValidatePass(user.Password, userLoginModel.Password, user.PasswordSalt);
                
                if (!string.IsNullOrEmpty(LoginDirectory(userLoginModel.Email, userLoginModel.Password)))
                {
                    result = true;
                }

                var jwtPayload = new JwtPayload()
                {
                    UserId = user.Id,
                    Name = user.Name,
                    RoleIds = user.UserInRoles != null ? user.UserInRoles.Select(x => x.RoleId).ToList() : null
                };

                var token = JwtHelper.GenerateToken(jwtPayload);

                //result = PasswordUtilities.ValidatePass(user.Password, userLoginModel.Password, user.PasswordSalt);
                if (result)
                {
                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Data = token
                    };
                }
                else
                {
                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Email or Password is incorrect. Please try again!"
                    };
                }
            }
            else
            {
                var name = LoginDirectory(userLoginModel.Email, userLoginModel.Password);
                if (!string.IsNullOrEmpty(name))
                {
                    await _userService.RegisterAsync(new UserRegisterModel()
                    {
                        Email = userLoginModel.Email,
                        Name = name
                    });

                    user = await _userService.GetByEmailAsync(userLoginModel.Email);

                    var jwtPayload = new JwtPayload()
                    {
                        UserId = user.Id,
                        //Mobile = user.Mobile,
                        Name = user.Name,
                        RoleIds = user.UserInRoles != null ? user.UserInRoles.Select(x => x.RoleId).ToList() : null
                    };

                    var token = JwtHelper.GenerateToken(jwtPayload);

                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Data = token
                    };
                }

                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Email is incorrect. Please try again!"
                };
            }
        }

        public ResponseModel VerifyTokenAsync(string token)
        {
            var jwtPayload = JwtHelper.ValidateToken(token);

            if (jwtPayload == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Unauthorized request"
                };
            }
            else
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = jwtPayload
                };
            }
        }
    }

    #endregion
}
