namespace TrackMS.AuthService.Entities
{
    public class Session
    {
        public string SessionId { get; set; }
        public string UserId { get; set; }
        public string LogInIpAddress { get; set; }
        public string RefreshToken { get; set; }
        public bool IsBlocked { get; set; } = false;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ExpiredAt { get; set; }
    }
}
