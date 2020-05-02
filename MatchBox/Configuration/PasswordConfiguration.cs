namespace MatchBox.Configuration
{
    public class PasswordConfiguration
    {
        public int MinLength { get; set; } = 8;
        public bool RequireLowercase { get; set; } = false;
        public bool RequireUppercase { get; set; } = false;
        public bool RequireNonAlphanumeric { get; set; } = false;
        public bool RequireDigit { get; set; } = false;        
    }    
}