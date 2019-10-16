using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Helpers.Interfaces
{
    public interface IPasswordHelper
    {
        string GenerateRandomPassword();
    }
}
