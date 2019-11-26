namespace EducationApp.BusinessLayer.Common.Constants
{
    public partial class Constants
    {
        public class PasswordOptions
        {
            public const int RequiredLength = 8;
            public const int RequiredUniqueChars = 4;
            public const bool RequireDigit = true;
            public const bool RequireUppercase = true;
            public const bool RequireLowercase = false;
            public const bool RequireNonAlphanumeric = false;
        }
    }
}
