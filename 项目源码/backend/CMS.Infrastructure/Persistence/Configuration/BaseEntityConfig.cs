using CMS.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Infrastructure.Persistence.Configuration;

public class BaseEntityConfig<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").HasColumnOrder(0);
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnOrder(992);
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnOrder(993);
        builder.Property(x => x.CreatedBy).HasColumnName("created_by").HasColumnOrder(994);
        builder.Property(x => x.UpdatedBy).HasColumnName("updated_by").HasColumnOrder(995);
        builder.Property(x => x.IsActived).HasColumnName("is_actived").HasColumnOrder(996);
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted").HasColumnOrder(997);
        builder.Property(t => t.Remarks).HasColumnName("remarks").HasMaxLength(800).HasColumnOrder(999);
    }
}

