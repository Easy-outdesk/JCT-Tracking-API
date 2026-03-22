namespace Vessel_Tracking_Api.Models
{
    public class RateLimitSettings
    {
        public int PermitLimit { get; set; }
        public int Window { get; set; }
        public int QueueLimit { get; set; }
    }

    public class TokenRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class ApiUser
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
