using CMS.Domain.Entity;
using CMS.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Infrastructrue.Persistence.Configuration
{
    public class ArticleConfig : BaseEntityConfig<ArticleInfo>
    {
        public override void Configure(EntityTypeBuilder<ArticleInfo> builder)
        {
            base.Configure(builder);

            builder.ToTable("article_info");

            builder.Property(x => x.Title).HasColumnName("title").HasMaxLength(2000).HasColumnOrder(1);
            builder.Property(x => x.Content).HasColumnName("content").HasColumnType("text").HasColumnOrder(2);
            builder.Property(x => x.CategoryId).HasColumnName("category_id").HasMaxLength(2000).HasColumnOrder(3);
            builder.Property(x => x.CommentCount).HasColumnName("comment_count").HasMaxLength(2000).HasColumnOrder(4);
            builder.Property(x => x.VisibleCount).HasColumnName("visible_count").HasMaxLength(2000).HasColumnOrder(5);
        }
    }
}