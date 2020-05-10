namespace MatchBox.Configuration
{
    public class SecurityConfiguration
    {
        public string JwtIssuerSigningKey { get; set; } = "2BFD74B8X07A1X44A9X8452X5B0FB629E0B69EF1CF7DX89DCX48E3X838DX82B0670483DB1C6727DEX0A46X42F5X9BE8X3248A89772FD";
        public string[] CorsOrigins { get; set; }
        public string Issuer { get; set; } = "MatchBox";
        public string Audience  { get; set; }
    }    
}