namespace EducationApp.DataAccessLayer.Common.Constants
{
    public partial class Constants
    {
        public partial class Errors
        {
            public const string TokenExpire = "RefreshToken expired!";
            public const string InvalidEmail = "Email not found";
            public const string IsExistedUser = "User is created";
            public const string UserNotFound = "User not found";
            public const string InvalidIdOrToken = "Get invalid confirmId or token";
            public const string InvalidData = "Invalid input data";
            public const string InvalidPassword = "Invalid password";
            public const string InvalidPasswordRequire = "Invalid password";
            public const string InvalidToken = "Invalid token";
            public const string InvalidDataFromClient = "Invalid Data From Client";
            public const string IsExistedPrintingEdition = "Printing Edition is existed";
            public const string InvalidFiltteringData = "Invalid filtering data";
            public const string SamePasswords = "New password can't be equals with current password";

            public const long NotFindUserId = 0;
        }
    }
}
