namespace SmartAccess.Domain.Entities
{
    public class AccessLog
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DoorId { get; set; }
        public DateTime Timestamp { get; set; }
        public bool AccessGranted { get; set; }
    }
}