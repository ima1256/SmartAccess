
namespace SmartAccess.Domain.Entities
{
    public class Door
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsLocked { get; set; } = true;
    }

}
