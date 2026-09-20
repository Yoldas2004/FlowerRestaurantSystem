using Restoran.Data.Enums;

namespace Restoran.Api.DTOs
{
    public class UpdateTableStatusRequestDto 
    {public int TableId { get; set; }
        public TableStatusType TableStatus { get; set; }
    }
}
