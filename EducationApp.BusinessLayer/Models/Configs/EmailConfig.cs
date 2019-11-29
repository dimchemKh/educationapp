namespace EducationApp.BusinessLogic.Models.Configs
{
    public class EmailConfig
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string NetCredentialName { get; set; }
        public string NetCredentialPass { get; set; }
        public string TestEmail { get; set; }
        public string ConfirmEmailUrl { get; set; }
        public string SubjectRecovery { get; set; }
        public string SubjectConfirmEmail { get; set; }
    }
}
