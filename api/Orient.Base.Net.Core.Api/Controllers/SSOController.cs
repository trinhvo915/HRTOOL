using Orient.Base.Net.Core.Api.Core.Business.Models.Users;
using Orient.Base.Net.Core.Api.Core.Business.Services;
using Orient.Base.Net.Core.Api.Core.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Orient.Base.Net.Core.Api.Core.Common.Helpers;
using Orient.Base.Net.Core.Api.Core.Business.Filters;
using Microsoft.AspNetCore.Authorization;
using Orient.Base.Net.Core.Api.Core.Business.Models;
using Microsoft.Extensions.Options;

namespace Orient.Base.Net.Core.Api.Controllers
{
	[Route("api/sso")]
	[EnableCors("CorsPolicy")]
	[ValidateModel]
	public class SSOController : BaseController
	{
		private readonly ISSOAuthService _ssoService;
		private readonly IUserService _userService;
		private readonly IAuthenticationService _authService;
		private readonly AppSettings _config;

		public SSOController(ISSOAuthService ssoService, IUserService userService, IAuthenticationService authService, IOptions<AppSettings> config)
		{
			_ssoService = ssoService;
			_userService = userService;
			_authService = authService;
			_config = config.Value;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] UserRegisterModel userRegisterModel)
		{
			var responseModel = await _userService.RegisterAsync(userRegisterModel);
			if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
			{
				var user = (User)responseModel.Data;
				return Ok(new UserViewModel(user));
			}
			else
			{
				return BadRequest(new { Message = responseModel.Message });
			}
		}

		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody] UserLoginModel userLoginModel)
		{
			var responseModel = await _ssoService.LoginAsync(userLoginModel);
			if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
			{
				return Ok(responseModel.Data);
			}
			else
			{
				return BadRequest(new { Message = responseModel.Message });
			}
		}

		[HttpGet("profile")]
		[CustomAuthorize]
		public async Task<IActionResult> Profile()
		{
			var accessToken = Request.Headers["x-access-token"].ToString();
			var jwtPayload = JwtHelper.ValidateToken(accessToken);

			if (jwtPayload == null)
			{
				return Unauthorized();
			}
			else
			{
				var userViewModel = await _userService.GetUserByIdAsync(jwtPayload.UserId);

				if (userViewModel == null)
				{
					return NotFound("Tài khoản không tìm thấy trong hệ thống. Vui lòng kiểm tra lại!");
				}
				else
				{
					return Ok(userViewModel);
				}
			}
		}
	}
}