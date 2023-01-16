using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NTierArchitectureServer.Business.Services.CategoryServices;
using NTierArchitectureServer.Business.Services.CategoryServices.Dtos;

namespace NTierArchitectureServer.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes ="Bearer")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(AddCategoryDto addCategoryDto)
        {
            await _categoryService.AddAsync(addCategoryDto);
            return NoContent();
        }

        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            return Ok(_categoryService.GetAll());
        }
    }
}
