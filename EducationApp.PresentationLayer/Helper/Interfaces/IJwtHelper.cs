using EducationApp.BusinessLayer.Models.Auth;
using EducationApp.PresentationLayer.Common;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace EducationApp.PresentationLayer.Helper.Interfaces
{
    public interface IJwtHelper
    {
        AuthModel CheckAccess(string token);
        AuthModel Generate(AuthModel authModel, IOptions<Config> configOptions);
    }
}
