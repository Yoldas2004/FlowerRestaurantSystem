namespace Restoran.Api.DTOs
{
    public class OrderResponseDto
    {public int Id { get; set; }
        public int TableId { get; set; }
        public string WaiterName { get; set; } = string.Empty;
        public string StatusInformation { get; set; } = string.Empty;
        public List<OrderItemResponseDto> OrderItems { get; set; } = new List<OrderItemResponseDto>();
    }
    public class OrderItemResponseDto
    {
        public string ProductName { get; set; } = string.Empty ;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

    }
}
