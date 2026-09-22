namespace Restoran.Api.DTOs
{
    public class UpdateCategoryRequestDto
    { 
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
