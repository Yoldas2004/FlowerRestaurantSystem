using Restoran.Data.Enums;

namespace Restoran.Api.DTOs
{
    public class UserResponseDto
    {
     public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public RoleType Role { get; set; }
         
    }
}
