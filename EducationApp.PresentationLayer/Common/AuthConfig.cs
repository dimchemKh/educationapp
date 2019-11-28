using System;

namespace EducationApp.Presentation.Common
{
    public class AuthConfig
    {
        public string AccessName { get; set; }
        public string RefreshName { get; set; }
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
        public TimeSpan AccessTokenExpiration { get; set; } = TimeSpan.FromMinutes(10);
        public TimeSpan RefreshTokenExpiration { get; set; } = TimeSpan.FromDays(60);
    }
}
