using Restoran.Data.Enums;

namespace Restoran.Api.DTOs
{
    public class PayOrderRequestDto
    {
        public int OrderId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
