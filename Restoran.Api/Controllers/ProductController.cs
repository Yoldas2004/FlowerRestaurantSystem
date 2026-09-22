using Microsoft.AspNetCore.Mvc;
using Restoran.Api.DTOs;
using Restoran.Data.Entities;
using Restoran.Data.Repository;

namespace Restoran.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController:ControllerBase
    {
        private readonly IGenericRepository<Product> _iGenericProduct;
        private readonly IGenericRepository<Category> _iGenericCategory;
        public ProductController(IGenericRepository<Product> iGenericProduct, IGenericRepository<Category> iGenericCategory)
        {
             _iGenericCategory = iGenericCategory;
            _iGenericProduct = iGenericProduct;
        }
        [HttpPost("createproduct")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto dto)
        {
            var bRequest = await _iGenericCategory.GetByIdAsync(dto.CategoryId);
            if (bRequest == null)
            {
                return BadRequest(dto.ProductName);
            }
            Product product = new Product
            {
                Name = dto.ProductName,
                CategoryId = dto.CategoryId,
                 Price = dto.Price,
                 IsAvailable= dto.IsAvalible
            };
           
            var controlProduct = await _iGenericProduct.AddAsync(product);
            await _iGenericProduct.SaveChangesAsync();
            var response = new ProductResponseDto
            { Id =controlProduct.Id,
                ProductName = product.Name,
                CategoryId = controlProduct.CategoryId,
                Price =controlProduct.Price,
                IsAvailable =controlProduct.IsAvailable
                  };

            return Ok(response);
        
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllProducts()
        {
            var product = await  _iGenericProduct.GetAllAsync();
            var response = product.Select(x => new ProductResponseDto { Id = x.Id,
                CategoryId =x.CategoryId,
                IsAvailable=x.IsAvailable,
                Price =x.Price,
                ProductName =x.Name
            
            
            }).ToList();
            return  Ok (response);         
        }
        [HttpPut("updateproduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequestDto dto) 
        {
        var find = await _iGenericProduct.GetByIdAsync(dto.ProductId);
            if (find == null)
            {
                return BadRequest("Urun bulunamadi");
            }
            find.Price = dto.Price;
            find.IsAvailable = dto.IsAvailable;
            var response = new ProductResponseDto
            {
               Id =find.Id,
               CategoryId=find.CategoryId,
               IsAvailable  =find.IsAvailable,
               Price=dto.Price,
               ProductName =find.Name
            };
            await _iGenericProduct.SaveChangesAsync();
            return Ok (response);
        }
    }
}
