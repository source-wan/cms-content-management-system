using CMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Infrastructure.Persistence.Configuration
{
    public class CollectionsConfig : BaseEntityConfig<Collections>
    {
        public override void Configure(EntityTypeBuilder<Collections> builder)
        {
            base.Configure(builder);

            builder.ToTable("collections");

            builder.Property(x => x.ArticleId).HasColumnName("article_id").HasMaxLength(2000).HasColumnOrder(1);
        }
    }

}