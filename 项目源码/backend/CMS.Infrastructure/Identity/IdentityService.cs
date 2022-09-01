using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CMS.App.Common.Config;
using CMS.App.Common.Interface;
using CMS.App.ReqDto;
using CMS.App.ResDto;
using CMS.Common.Interface;
using CMS.Domain.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CMS.Infrastructure.Identity
{
    public class IdentityService : IIdentity
    {
        private readonly IConfiguration _config;
        private readonly IRepository<UserInfo> _userInfo;
        private readonly IRepository<IdentityUser> _identityUser;


        public IdentityService
        (
            IConfiguration config,
            IRepository<UserInfo> userInfo,
            IRepository<IdentityUser> identityUser
        )
        {
            _config = config;
            _userInfo = userInfo;
            _identityUser = identityUser;
        }

        public async Task<TokenDto> GenerateToken(UserInfo userInfo)
        {
            var jwtSetting = _config.GetSection("JwtSetting").Get<JwtSetting>();
            var claims = new[]
           {
                new Claim("UserId",userInfo!.Id.ToString()),
                new Claim("Username",userInfo!.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(jwtSetting.Issuer, jwtSetting.Audience, claims, expires: DateTime.UtcNow.AddMinutes(jwtSetting.AccessExpiration), signingCredentials: credentials);

            //生成token
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            var refreshToken = GenerateRefreshToken();

            var tmpIdentityUser = new IdentityUser
            {
                UserId = userInfo.Id,
                Username = userInfo.Username,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(jwtSetting.RefreshExpiration)
            };

            var IdentityUser = _identityUser.Table.Where(x => x.UserId == userInfo.Id).ToList();
            await _identityUser.DeleteAsync(IdentityUser, true);
            await _identityUser.UpdateAsync(tmpIdentityUser);

            return new TokenDto
            {
                AccessToken = token,
                RefreshToken = refreshToken
            };
        }

        public async Task<TokenDto> GenerateRefreshToken(TokenDto token)
        {
            var jwtSetting = _config.GetSection("JwtSetting").Get<JwtSetting>();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSetting.Issuer,
                ValidAudience = jwtSetting.Audience,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Secret))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token.AccessToken, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken)
            {
                throw new SecurityTokenException("token无效");
            }

            if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("错误的加密算法");
            }
            var username = principal.FindFirstValue("Username");

            var user = _identityUser.Table.FirstOrDefault(x => x.Username.Equals(username));
            if (user == null || user.RefreshToken != token.RefreshToken)
            {
                throw new Exception("传入的 Refresh token 无效");
            }

            if ( user.RefreshTokenExpiration <= DateTime.Now)
            {
                throw new Exception("Refresh token 已经过期了");
            }
            var appUser = _userInfo.Table.FirstOrDefault(x => x.Username == username);

            if (appUser != null)
            {
                return await GenerateToken(appUser);
            }
            else
            {
                return new TokenDto();
            }
        }


        public async Task<TokenDto> ValidateUserAsync(LoginUserDto userForAuth)
        {
            var appUser = _userInfo.Table.FirstOrDefault(x => x.Username.Equals(userForAuth.Username)
            && x.Password.Equals(userForAuth.Password));
            if (appUser != null)
            {
                var appToken =await this.GenerateToken(appUser);
                return appToken;
            }
            else
            {
                return new TokenDto();
            }
        }
        private string GenerateRefreshToken()
        {
            var rndNumber = new byte[32];

            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(rndNumber);

            return Convert.ToBase64String(rndNumber);
        }
    }
}