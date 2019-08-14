using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Orient.Base.Net.Core.Api.Core.Business.Filters;
using Orient.Base.Net.Core.Api.Core.Business.Models.Roles;
using Orient.Base.Net.Core.Api.Core.Business.Services;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Controllers
{
	[Route("api/roles")]
	[EnableCors("CorsPolicy")]
	public class RoleController : BaseController
	{
		private readonly IRoleService _roleService;

		public RoleController(IRoleService roleService)
		{
			_roleService = roleService;

		}

		[HttpGet]
		public async Task<IActionResult> Get(RoleRequestListViewModel roleRequestListViewModel)
		{
			var roles = await _roleService.ListRoleAsync(roleRequestListViewModel);
			return Ok(roles);
		}

		[HttpGet("by-name")]
		public async Task<IActionResult> GetbyName(string name)
		{
			var role = await _roleService.GetRoleByNameAsync(name);
			if (role == null)
			{
				return NotFound("Nội dung không tìm thấy trong hệ thống!");
			}
			return Ok(role);
		}

		[HttpGet("admin")]
		public async Task<IActionResult> GetRoleAdmin()
		{
			var role = await _roleService.GetRoleAdmin();
			if (role == null)
			{
				return NotFound("Nội dung không tìm thấy trong hệ thống!");
			}
			return Ok(role);
		}
	}
}