using CMS.App.ReqDto;
using CMS.App.ResDto;
using CMS.Domain.Entity;

namespace CMS.Common.Interface
{
    public interface IIdentity
    {
        Task<TokenDto> ValidateUserAsync(LoginUserDto userForAuth);

        Task<TokenDto> GenerateToken(UserInfo user);

        Task<TokenDto> GenerateRefreshToken(TokenDto token);
    }
}