using CMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Infrastructure.Persistence.Configuration;
public class UserInfoConfig : BaseEntityConfig<UserInfo>
{
    public override void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        base.Configure(builder);

        builder.ToTable("user_info");

        builder.Property(x => x.Username).HasColumnName("username").HasMaxLength(100).HasColumnOrder(1);
        builder.Property(x => x.Password).HasColumnName("password").HasMaxLength(200).HasColumnOrder(2);
        builder.Property(x => x.Avatar).HasColumnName("avatar").HasMaxLength(500).HasColumnOrder(3);
        builder.Property(x => x.NickName).HasColumnName("nickname").HasMaxLength(100).HasColumnOrder(4);
        builder.Property(x=>x.isAdmin).HasColumnName("isadmin").HasColumnOrder(5);
    }
}