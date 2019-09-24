using System;

namespace EducationApp.PresentationLayer.Common
{
    public class Config
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; } = "EducationApp";
        public string JwtAudience { get; set; } = "WebApiEducationApp";
        public int JwtExpireMinutes { get; set; }
        public TimeSpan AccessTokenExpiration { get; set; } = TimeSpan.FromMinutes(10);
        public TimeSpan RefreshTokenExpiration { get; set; } = TimeSpan.FromDays(60);
    }
}
