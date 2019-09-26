﻿using EducationApp.PresentationLayer.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Helper.Interfaces
{
    public interface IJwtHelper
    {
        string GenerateToken(List<Claim> claims, IOptions<Config> configOptions, TimeSpan tokenExpiration);
        List<Claim> GetAccessClaims(string userId, string role, string userName);
        List<Claim> GetRefreshClaims(string userId);
    }
}