using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Common.Constants
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
