namespace CMS.App.Common.Config
{
    public class JwtSetting
    {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string Secret { get; set; } = null!;
        public int RefreshExpiration { get; set; }
        public int AccessExpiration { get; set; }
    }
}