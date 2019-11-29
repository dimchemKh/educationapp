namespace EducationApp.BusinessLogic.Models.Configs
{
    public class PasswordOptions
    {
        public int RequiredLength { get; set; }
        public int RequiredUniqueChars { get; set; }
        public int DefaultLockoutTimeSpan { get; set; }
        public int MaxFailedAccessAttempts { get; set; }
        public bool RequireDigit { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
        public bool AllowedForNewUsers { get; set; }
        public bool RequireUniqueEmail { get; set; }
    }
}
