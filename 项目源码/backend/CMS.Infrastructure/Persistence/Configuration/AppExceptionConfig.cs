using CMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Infrastructure.Persistence.Configuration
{
public class AppExceptionLog : BaseEntityConfig<AppException>
    {
        public override void Configure(EntityTypeBuilder<AppException> builder)
        {
            base.Configure(builder);

            //设置表名，查了老半天才查到原来设置表名方法是这个，残念。。。熬了个大夜
            builder.ToTable("app_exception_log");

            builder.Property(x => x.ShortMessage).HasColumnName("short_message").HasColumnType("text").HasColumnOrder(1);
            builder.Property(x => x.FullMessage).HasColumnName("full_message").HasColumnType("text").HasColumnOrder(2);
        }
    }

}