using CMS.App.Common.Interface;
using CMS.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CMS.App.Common.Config;
using CMS.Common.Interface;
using CMS.Infrastructure.Identity;
using CMS.Infrastructure.File;

namespace CMS.Infrastructure.Persistence
{
    /// <summary>
    /// 依赖注入相关功能
    /// </summary>
    public static class DI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // 注入数据库上下文
            services.AddDbContext<CMSDbContext>(options =>
                options.UseNpgsql(
                    // 获取链接字符串
                    config.GetConnectionString("PostgreSQL"),
                    // 应用数据库的相关设置在Migrations中
                    builder => builder.MigrationsAssembly(typeof(CMSDbContext).Assembly.FullName)
                )
            );

            // 注入 Repository 的实现
            services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));
            // 注入验证服务
            services.AddTransient(typeof(IIdentity), typeof(IdentityService));
            services.AddScoped(typeof(IAppFileUpload),typeof(AppFileUpload));
            // 注入验证器0
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false; // 设置是否验证请求方法
                    options.SaveToken = true; // 设置是否将 Token 保存在 API 上下文

                    var tokenParameter = config.GetSection("JwtSetting").Get<JwtSetting>();
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = tokenParameter.Issuer,
                        ValidAudience = tokenParameter.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenParameter.Secret))
                    };
                });
            return services;
        }
    }
}