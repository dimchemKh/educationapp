namespace EducationApp.BusinessLogic.Common.Constants
{
    public partial class Constants
    {
        public class PasswordGenerationParams
        {
            public const string LowercaseChars = "abcdefghijkmnopqrstuvwxyz";
            public const string UppercaseChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            public const string DigitChars = "0123456789";

            public static readonly string[] ListCharsForPassword = new string[]
            {
                LowercaseChars,
                UppercaseChars,
                DigitChars
            };
        }
    }
}
