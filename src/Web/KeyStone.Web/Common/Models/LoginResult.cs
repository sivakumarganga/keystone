namespace KeyStone.Web.Common.Models
{
    public class LoginResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public DateTime? ExpiresOn { get; set; }
    }
}
