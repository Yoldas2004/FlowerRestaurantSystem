namespace Restoran.Api.DTOs
{
    public class UpdateProductRequestDto
    {
        public int ProductId { get; set; }
        public decimal  Price { get; set; }
        public bool IsAvailable { get; set; }

    }
}
