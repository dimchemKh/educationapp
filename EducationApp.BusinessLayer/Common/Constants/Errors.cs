using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Common.Constants
{
    public partial class Constants
    {
        public partial class Errors
        {
            public const string EmailNotFound = "Email not found";
            public const string EmailInvalid = "Invalid email";
            public const string EmailIsConfirmed = "User didn't confirm email";
            public const string EmailConfirmed = "User confirmed email yet";

            public const string PasswordEmpty = "Password is empty";
            public const string PasswordInvalid = "Invalid password";
            public const string PasswordsSame = "New password can't be equals with current password";

            public const string TokenExpire = "RefreshToken expired";
            public const string TokenInvalid = "Invalid token";

            public const string UserIdInvalid = "Invalid userId";
            public const string UserExisted = "User is created";
            public const string UserNotFound = "User not found";
            public const string UserRemoved = "We're sorry, maybe this user deleted";
            public const string UserWrongRegister = "Something wrong with register user";
            public const string UserFailIdentity = "We couldn't identify user with this email";
            public const string UserBloced = "This user blocked";

            public const string InvalidConfirmData = "Get invalid userId or confirmToken";
            public const string InvalidData = "Invalid input data";

            public const string PrintingEditionExisted = "Printing Edition is existed";

            public const string TransactionInvalid = "Invalid transaction or order";

            public const string OccuredProcessing = "An error has occurred while processing your request";

            public const string FailedUpdate = "Failed update";
        }
    }
}
