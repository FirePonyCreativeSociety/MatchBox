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

        public SecurityConfiguration Security { get; set; } = new SecurityConfiguration();

        public UserConfiguration User { get; set; } = new UserConfiguration();            

        public PasswordConfiguration Password { get; set; } = new PasswordConfiguration();

        public EmailConfiguration Email { get; set; } = new EmailConfiguration();

        
    }
}