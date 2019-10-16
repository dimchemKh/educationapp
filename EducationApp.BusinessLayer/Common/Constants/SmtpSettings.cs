using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Common.Constants
{
    public partial class Constants
    {
        public class SmtpSettings
        {
            public const string SmtpHost = "smtp.mailtrap.io";
            public const int SmtpPort = 2525;
            public const string NetCredentialName = "beb858dd98302d";
            public const string NetCredentialPass = "f2b28f22609ec7";

            public const string TestEmail = "from@example.com";
        }
    }
}
