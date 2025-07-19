using System.ComponentModel.DataAnnotations;

namespace SmartAccess.Application.DTOs
{
    public class UserDto
    {

        public string Id { get; set; } = string.Empty;
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "RFIDCard is required")]
        public string RFIDCard { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;

    }
}