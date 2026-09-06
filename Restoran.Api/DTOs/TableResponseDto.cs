using Restoran.Data.Enums;

namespace Restoran.Api.DTOs
{
    public class TableResponseDto
    { 
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public string TableStatus { get; set; } = string.Empty;
        public bool IsActive { get; set; }

    }
}
