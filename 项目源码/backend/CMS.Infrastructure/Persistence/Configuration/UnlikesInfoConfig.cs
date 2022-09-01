using CMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Infrastructure.Persistence.Configuration
{
    public class UnlikesInfoConfig : BaseEntityConfig<UnlikesInfo>
    {
        public override void Configure(EntityTypeBuilder<UnlikesInfo> builder)
        {
            base.Configure(builder);

            builder.ToTable("unlike_info");

            builder.Property(x => x.ArticleId).HasColumnName("article_id").HasMaxLength(2000).HasColumnOrder(1);
        }
    }

}