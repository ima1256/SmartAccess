using System.ComponentModel.DataAnnotations;

namespace SmartAccess.Application.DTOs
{
    public class UserDto
    {
        [Required]
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string RFIDCard { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;

    }
}