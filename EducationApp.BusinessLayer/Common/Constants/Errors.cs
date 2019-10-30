using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Common.Constants
{
    public partial class Constants
    {
        public partial class Errors
        {
            public const string TokenExpire = "RefreshToken expired";
            public const string EmailNotFound = "Email not found";
            public const string InvalidEmail = "Invalid email";
            public const string InvalidUserId = "Invalid userId";
            public const string IsExistedUser = "User is created";
            public const string UserNotFound = "User not found";
            public const string InvalidConfirmData = "Get invalid userId or confirmToken";
            public const string InvalidData = "Invalid input data";
            public const string EmptyPassword = "Password is empty";
            public const string InvalidPassword = "Invalid password";
            public const string InvalidToken = "Invalid token";
            public const string IsExistedPrintingEdition = "Printing Edition is existed";
            public const string InvalidFiltteringData = "Invalid filtering data";
            public const string SamePasswords = "New password can't be equals with current password";
            public const string InvalidTransaction = "Invalid transaction or order";
            public const string RemovedUser = "We're sorry, maybe this user with this email was removed";
            public const string CanNotRegisterUser = "Something wrong with register user";
            public const string OccuredProcessing = "An error has occurred while processing your request";
            public const string IsConfirmedEmail = "User didn't confirm email";
            public const string SuccessConfirmedEmail = "User confirmed email yet";
            public const string FalseIdentityUser = "We couldn't identify user with this email";
            public const string BlockedUser = "This user blocked";
            public const string FailedUpdate = "Failed update";
        }
    }
}
