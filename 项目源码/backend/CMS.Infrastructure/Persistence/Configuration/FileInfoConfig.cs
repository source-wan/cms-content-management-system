using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FileInfo = CMS.Domain.Entity.FileInfo;

namespace CMS.Infrastructure.Persistence.Configuration;
public class FileInfoConfig : BaseEntityConfig<FileInfo>
{
    public override void Configure(EntityTypeBuilder<FileInfo> builder)
    {
        base.Configure(builder);

        builder.ToTable("file_info");

        builder.Property(x => x.Id).HasColumnName("file_id").HasMaxLength(100).HasColumnOrder(1);
        builder.Property(x => x.OriginName).HasColumnName("origin_name").HasMaxLength(200).HasColumnOrder(2);
        builder.Property(x => x.CurrentName).HasColumnName("current_name").HasMaxLength(500).HasColumnOrder(3);
        builder.Property(x => x.ContentType).HasColumnName("content_type").HasMaxLength(100).HasColumnOrder(4);
        builder.Property(x => x.RelativePath).HasColumnName("relative_path").HasMaxLength(100).HasColumnOrder(5);
        builder.Property(x => x.FileSize).HasColumnName("file_size").HasMaxLength(100).HasColumnOrder(6);
    }
}
