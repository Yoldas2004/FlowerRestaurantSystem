using Microsoft.AspNetCore.Components.Web;

namespace Restoran.Api.DTOs
{
    public class UpdateTableActiveRequestDto
    {
        public int TableId { get; set; }

        public bool IsActive { get; set; }  
    }
}
