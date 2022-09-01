using System.Security.Claims;
using CMS.App.Common.Interface;

namespace CMS.Api.Service
{
    public class UserSession : IUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid? Id
        {
            get
            {
                // 从请求的上下文中获取当前 Session 的用户Id
                var userIdStr = _httpContextAccessor.HttpContext?.User.FindFirstValue("UserId");
                // 如果本次请求中 存在 userId, 则返回他，否则返回 null
                return userIdStr == null ? null : new Guid(userIdStr);
            }
        }

        public string? Name => _httpContextAccessor.HttpContext?.User.FindFirstValue("UserName");
    }
}