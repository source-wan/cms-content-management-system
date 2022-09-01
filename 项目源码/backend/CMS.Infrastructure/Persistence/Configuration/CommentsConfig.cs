using CMS.Domain.Entity;
using CMS.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Infrastructrue.Persistence.Configuration
{
    public class CommentsConfig : BaseEntityConfig<Comments>
    {
        public override void Configure(EntityTypeBuilder<Comments> builder)
        {
            base.Configure(builder);

            builder.ToTable("comments");

            builder.Property(x => x.ArtocleId).HasColumnName("article_id").HasMaxLength(2000).HasColumnOrder(1);
            builder.Property(x => x.Content).HasColumnName("content").HasColumnType("text").HasColumnOrder(2);
            builder.Property(x => x.LikeCount).HasColumnName("like_count").HasMaxLength(2000).HasColumnOrder(3);
            builder.Property(x => x.UnlikeCount).HasColumnName("unlike_count").HasMaxLength(2000).HasColumnOrder(4);
        }
    }
}