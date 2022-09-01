using CMS.Domain.Entity;
using CMS.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Infrastructrue.Persistence.Configuration
{
    public class AppAuditLogConfig : BaseEntityConfig<AppAuditLog>
    {
        public override void Configure(EntityTypeBuilder<AppAuditLog> builder)
        {
            base.Configure(builder);

            //设置表名，查了老半天才查到原来设置表名方法是这个，残念。。。熬了个大夜
            builder.ToTable("app_audit_log");

            builder.Property(x => x.Parameters).HasColumnName("parameters").HasMaxLength(2000).HasColumnOrder(1);
            builder.Property(x => x.BrowserInfo).HasColumnName("browser_info").HasMaxLength(2000).HasColumnOrder(2);
            builder.Property(x => x.ClientName).HasColumnName("client_name").HasMaxLength(2000).HasColumnOrder(3);
            builder.Property(x => x.ClientIpAddress).HasColumnName("client_ip_address").HasMaxLength(2000).HasColumnOrder(4);
            builder.Property(x => x.ExecutionDuration).HasColumnName("execution_duration").HasMaxLength(2000).HasColumnOrder(5);
            builder.Property(x => x.ExecutionTime).HasColumnName("execution_time").HasColumnOrder(6);
            builder.Property(x => x.ReturnValue).HasColumnName("return_value").HasColumnType("text").HasColumnOrder(7);
            builder.Property(x => x.Exception).HasColumnName("exception").HasColumnType("text").HasColumnOrder(8);
            builder.Property(x => x.MethodName).HasColumnName("method_name").HasMaxLength(2000).HasColumnOrder(9);
            builder.Property(x => x.ServiceName).HasColumnName("service_name").HasMaxLength(2000).HasColumnOrder(10);
            builder.Property(x => x.UserInfo).HasColumnName("user_info").HasMaxLength(2000).HasColumnOrder(11);
            builder.Property(x => x.CustomData).HasColumnName("custom_data").HasMaxLength(2000).HasColumnOrder(12);
        }
    }
}