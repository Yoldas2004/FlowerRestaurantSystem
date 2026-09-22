    using Microsoft.AspNetCore.Mvc;
    using Restoran.Api.DTOs;
    using Restoran.Data.Entities;
    using Restoran.Data.Repository;

    namespace Restoran.Api.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class CategoryController : ControllerBase
        {
            private readonly IGenericRepository<Category> _categoryRepository;
            public CategoryController(IGenericRepository<Category> categoryRepository)
            {
                _categoryRepository = categoryRepository;
            }
            [HttpPost("create")]
            public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto dto)
            {   Category category = new Category { CategoryName =dto.CategoryName };

                var controlCategory = await _categoryRepository.AddAsync(category);
                await _categoryRepository.SaveChangesAsync();
                var response = new CategoryResponseDto {  Id = controlCategory.Id ,CategoryName = controlCategory.CategoryName };
          
                return Ok(response);

            }
            [HttpGet("getall")]
            public async Task<IActionResult> GetAllCategories( )
            {
                var categories =await _categoryRepository.GetAllAsync();
                var response = categories.Select(x => new CategoryResponseDto
                {
                    Id = x.Id,
                    CategoryName = x.CategoryName,
                }
                    ).ToList();
                return Ok(response);
            }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateStatus([FromBody]  UpdateCategoryRequestDto dto)
        {
            var finder = await _categoryRepository.GetByIdAsync(dto.CategoryId);
            
            if (finder == null)
            {
                return BadRequest("Kategori Bulunamadi!!!!");

            }
            finder.CategoryName = dto.CategoryName;
            var response = new CategoryResponseDto 
            {
            CategoryName = finder.CategoryName,
            Id = finder.Id
            };
           await _categoryRepository.SaveChangesAsync();
        return Ok(response);
            
        }
        
    }
}
