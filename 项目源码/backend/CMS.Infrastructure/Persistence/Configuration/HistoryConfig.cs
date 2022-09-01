using CMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Infrastructure.Persistence.Configuration
{
    public class HistoryConfig : BaseEntityConfig<History>
    {
        public override void Configure(EntityTypeBuilder<History> builder)
        {
            base.Configure(builder);

            builder.ToTable("history");

            builder.Property(x => x.ArticleId).HasColumnName("article_id").HasMaxLength(2000).HasColumnOrder(1);
        }
    }

}