using CMS.Domain.Base;

namespace CMS.Domain.Entity
{
    public class IdentityUser : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime RefreshTokenExpiration { get; set; }
    }
}