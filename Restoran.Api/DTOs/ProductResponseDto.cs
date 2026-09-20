namespace Restoran.Api.DTOs
{
    public class ProductResponseDto
    {
       public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }  

        public int CategoryId { get; set; }
        public bool IsAvailable { get; set; }

    }
}
