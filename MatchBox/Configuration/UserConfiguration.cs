namespace MatchBox.Configuration
{
    public class UserConfiguration
    {
        public int MaxFailedAccessAttempts { get; set; } = 3;
        public int LockoutDurationInMinutes { get; set; } = 5;
        public string AllowedUserNameCharacters { get; set; } = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    }    
}