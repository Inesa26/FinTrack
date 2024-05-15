namespace FinTrack.Infrastructure.Options
{
    public class JwtSettings
    {
        public string? SecretKey { get; set; }
        public string? Issuer { get; set; }
        public string[]? Audiences { get; set; }
    }
}
