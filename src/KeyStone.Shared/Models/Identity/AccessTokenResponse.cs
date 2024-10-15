using System.IdentityModel.Tokens.Jwt;

namespace KeyStone.Shared.Models.Identity
{
    public class AccessTokenResponse(JwtSecurityToken securityToken, string refreshToken = "")
    {
        public string AccessToken { get; set; } = new JwtSecurityTokenHandler().WriteToken(securityToken);
        public string RefreshToken { get; set; } = refreshToken;
        public string TokenType { get; set; } = "Bearer";
        public int ExpiresIn { get; set; } = (int)(securityToken.ValidTo - DateTime.UtcNow).TotalSeconds;
    }
}
