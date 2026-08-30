namespace Restoran.Api.DTOs
{
    public class CreateOrderRequestDto
    {
        public int TableId { get; set; }
        public int WaiterId { get; set; }
    }
}
