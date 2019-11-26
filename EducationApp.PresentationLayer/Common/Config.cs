﻿using System;

namespace EducationApp.PresentationLayer.Common
{
    public class Config
    {
        public string AccessName { get; set; }
        public string RefreshName { get; set; }
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
        public TimeSpan AccessTokenExpiration { get; set; } = TimeSpan.FromMinutes(10);
        public TimeSpan RefreshTokenLongExpiration { get; set; } = TimeSpan.FromDays(60);
        public TimeSpan RefreshTokenShortExpiration { get; set; } = TimeSpan.FromHours(12);
    }
}
