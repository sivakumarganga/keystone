using EntityFrameworkCore.UnitOfWork.Interfaces;
using KeyStone.Concerns.Domain;
using KeyStone.Identity.Dtos;
using KeyStone.Identity.Wrappers.Managers;
using KeyStone.Shared.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KeyStone.Data.Models.Identity;
using KeyStone.Data.RepoContracts;
using KeyStone.Identity.Contracts;

namespace KeyStone.Identity.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly IdentitySettings _siteSetting;
        private readonly AppUserManager _userManager;
        private readonly IUserClaimsPrincipalFactory<User> _claimsPrincipal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRefreshTokenRepository _refreshTokenRepository;
        public JwtService(IOptions<IdentitySettings> siteSetting,
                          AppUserManager userManager,
                          IUserClaimsPrincipalFactory<User> claimsPrincipal,
                          IUnitOfWork unitOfWork,
                          IUserRefreshTokenRepository refreshTokenRepository)
        {
            _claimsPrincipal = claimsPrincipal;
            _siteSetting = siteSetting.Value;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<AccessTokenResponse> GenerateAsync(User user)
        {
            var secretKey = Encoding.UTF8.GetBytes(_siteSetting.SecretKey); // longer that 16 character
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionkey = Encoding.UTF8.GetBytes(_siteSetting.Encryptkey); //must be 16 character
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);


            var claims = await GetClaimsAsync(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _siteSetting.Issuer,
                Audience = _siteSetting.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(0),
                Expires = DateTime.Now.AddMinutes(_siteSetting.ExpirationMinutes),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

            var repository = _unitOfWork.CustomRepository<IUserRefreshTokenRepository>();
            var refreshToken = await repository.CreateToken(user.Id);

            await _unitOfWork.SaveChangesAsync();

            //TODO: Pass refresh token
            return new AccessTokenResponse(securityToken, refreshToken.ToString());
        }

        public async Task<AccessTokenResponse> GenerateByPhoneNumberAsync(string phoneNumber)
        {
            var user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            var result = await this.GenerateAsync(user);
            return result;
        }

        public Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_siteSetting.SecretKey)),
                ValidateLifetime = false,
                TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_siteSetting.Encryptkey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return Task.FromResult(principal);
        }

        public async Task<AccessTokenResponse> RefreshToken(Guid refreshTokenId)
        {
            var refreshToken = await _refreshTokenRepository.GetTokenWithInvalidation(refreshTokenId);
            if (refreshToken is not null)
            {
                refreshToken.IsValid = false;
                _refreshTokenRepository.Update(refreshToken);
                var user = await _refreshTokenRepository.GetUserByRefreshToken(refreshTokenId);

                if (user is null)
                    return null;

                var result = await this.GenerateAsync(user);
            }

            return default;
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(User user)
        {
            var claims = new List<Claim>();
            var result = await _claimsPrincipal.CreateAsync(user);
            claims.AddRange(result.Claims);
            return claims;
        }
    }
}
