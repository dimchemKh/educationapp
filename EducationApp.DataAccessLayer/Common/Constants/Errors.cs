using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Common.Constants
{
    public partial class Constants
    {
        public partial class Errors
        {
            public const string TokenExpire = "RefreshToken expired!";
            public const string InvalidEmail = "Email not found";
            public const string IsExistedUser = "User is created";
            public const string UserNull = "User is null";
            public const string InvalidIdOrToken = "Get invalid confirm id or token";
            public const string InvalidData = "Invalid input data";
            public const string InvalidConfirmPassword = "Invalid confirm password";
            public const string InvalidToken = "Invalid token";
        }
    }
}
