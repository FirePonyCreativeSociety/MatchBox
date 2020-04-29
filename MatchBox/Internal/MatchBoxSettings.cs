using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Internal
{
    public class MatchBoxSettings
    {                
        public class JwtSettings
        {
            public string IssuerSigningKey { get; set; } = "2BFD74B8X07A1X44A9X8452X5B0FB629E0B69EF1CF7DX89DCX48E3X838DX82B0670483DB1C6727DEX0A46X42F5X9BE8X3248A89772FD";
        }

        public JwtSettings Jwt { get; set; } = new JwtSettings();

        public class UserSettings
        {
            public int MaxFailedAccessAttempts { get; set; } = 3;
            public int LockoutDurationInMinutes { get; set; } = 5;
            public string AllowedUserNameCharacters { get; set; } = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        }

        public UserSettings User { get; set; } = new UserSettings();            

        public class PasswordSettings
        {
            public int MinLength { get; set; } = 8;
            public bool RequireLowercase { get; set; } = false;
            public bool RequireUppercase { get; set; } = false;
            public bool RequireNonAlphanumeric { get; set; } = false;
            public bool RequireDigit { get; set; } = false;
        }

        public PasswordSettings Password { get; set; } = new PasswordSettings();
    }
}