using CMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Infrastructure.Persistence.Configuration
{
    public class LikesInfoConfig : BaseEntityConfig<LikesInfo>
    {
        public override void Configure(EntityTypeBuilder<LikesInfo> builder)
        {
            base.Configure(builder);

            builder.ToTable("likes_info");

            builder.Property(x => x.ArticleId).HasColumnName("article_id").HasMaxLength(2000).HasColumnOrder(1);
        }
    }

}