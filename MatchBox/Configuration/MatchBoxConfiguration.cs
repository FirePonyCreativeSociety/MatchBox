using MatchBox.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Configuration
{
    public partial class MatchBoxConfiguration
    {
        public const string AppSettingsSectionName = "MatchBox";

        public JwtConfiguration Jwt { get; set; } = new JwtConfiguration();

        public UserConfiguration User { get; set; } = new UserConfiguration();            

        public PasswordConfiguration Password { get; set; } = new PasswordConfiguration();

        public EmailSettings Email { get; set; } = new EmailSettings();
    }
}