    namespace Restoran.Api.DTOs
    {
        public class CreateProductRequestDto
        {
            public int CategoryId { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public decimal Price { get; set; }
        public bool IsAvalible { get; set; } = true;

        }
    }
