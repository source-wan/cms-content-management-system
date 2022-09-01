using CMS.Domain.Entity;
using CMS.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Infrastructrue.Persistence.Configuration
{
    public class CategoryInfoConfig : BaseEntityConfig<CategoryInfo>
    {
        public override void Configure(EntityTypeBuilder<CategoryInfo> builder)
        {
            base.Configure(builder);

            builder.ToTable("category_info");

            builder.Property(x => x.CategoryName).HasColumnName("category_name").HasMaxLength(2000).HasColumnOrder(1);
        }
    }
}