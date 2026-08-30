namespace Restoran.Api.DTOs
{
    public class AddOrderItemRequestDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? Note { get; set; }
    }
}
