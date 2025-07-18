
namespace SmartAccess.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string RFIDCard { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public string Role { get; set; } = string.Empty;

    }

}
