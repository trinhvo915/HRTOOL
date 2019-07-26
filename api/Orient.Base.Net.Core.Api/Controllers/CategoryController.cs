using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Cors;
using System.Threading.Tasks;
using Orient.Base.Net.Core.Api.Core.Business.Filters;
using Orient.RMS.Api.Core.Business.Services;
using Orient.Base.Net.Core.Api.Core.Business.Models.Categories;

namespace Orient.Base.Net.Core.Api.Controllers
{
    [Route("api/categories")]
    [EnableCors("CorsPolicy")]
    [ValidateModel]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CategoryRequestListViewModel categoryRequestListViewModel)
        {
            var category = await _categoryService.ListCategoryAsync(categoryRequestListViewModel);
            return Ok(category);
        }

        [HttpGet("all-categories")]
        public async Task<IActionResult> GetAllCategories(CategoryRequestGetAllModel categoryRequestGetAllModel)
        {
            var category = await _categoryService.GetAllCategoryAsync(categoryRequestGetAllModel);
            return Ok(category);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var responseModel = await _categoryService.GetCategoryByIdAsync(id);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryManageModel categoryManageModel)
        {
            var responseModel = await _categoryService.CreateCategoryAsync(categoryManageModel);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryManageModel categoryManageModel)
        {
            var responseModel = await _categoryService.UpdateCategoryAsync(id, categoryManageModel);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var responseModel = await _categoryService.DeleteCategoryAsync(id);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }
    }
}
