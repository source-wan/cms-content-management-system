using CMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Infrastructure.Persistence.Configuration;
public class IdentityUserConfig : BaseEntityConfig<IdentityUser>
{
    public override void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        base.Configure(builder);

        builder.ToTable("identity_user");

        builder.Property(x => x.UserId).HasColumnName("user_id").HasMaxLength(100).HasColumnOrder(1);
        builder.Property(x => x.Username).HasColumnName("username").HasMaxLength(200).HasColumnOrder(2);
        builder.Property(x => x.RefreshToken).HasColumnName("refresh_token").HasMaxLength(500).HasColumnOrder(3);
        builder.Property(x => x.RefreshTokenExpiration).HasColumnName("refresh_token_expiration").HasMaxLength(100).HasColumnOrder(4);
    }
}