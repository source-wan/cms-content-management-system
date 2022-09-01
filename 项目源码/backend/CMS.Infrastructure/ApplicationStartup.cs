using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.App.Common.Interface;
using CMS.Domain.Entity;
using CMS.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CMS.Infrastructure
{
    public static class ApplicationStartup
    {
        public static async void MigrateDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<CMSDbContext>();

                // 自动同步迁移文件到数据库服务器，前提是必须先（手动）生成迁移文件
                context.Database.Migrate();

                var appUserRes = services.GetRequiredService<IRepository<UserInfo>>();
                // var appRoleRes = services.GetRequiredService<IRepository<AppRole>>();
                // var appUserRoleRes = services.GetRequiredService<IRepository<AppUserRole>>();
                // var appArticle=services.GetRequiredService<IRepository<ArticleInfo>>();
                // var CategoryInfo=services.GetRequiredService<IRepository<CategoryInfo>>();
                if (!appUserRes.Table.Any())
                {

                    var user = new UserInfo
                    {
                        Username = "admin",
                        Password = "113",
                        NickName="超级小子",
                        isAdmin=true
                    };
                    var users = new List<UserInfo>()
                    {
                        
                        new UserInfo
                        {
                            Username = "momfucker",
                            Password = "sucks",
                            NickName = "yourdad"
                        },
                        new UserInfo
                        {
                            Username = "admin1",
                            Password = "sucks",
                            NickName = "yourdad"
                        },
                        new UserInfo
                        {
                            Username = "admin2",
                            Password = "sucks",
                            NickName = "yourdad"
                        },
                        new UserInfo
                        {
                            Username = "admin3",
                            Password = "sucks",
                            NickName = "yourdad"
                        },
                        new UserInfo
                        {
                            Username = "admin4",
                            Password = "sucks",
                            NickName = "yourdad"
                        },
                        new UserInfo
                        {
                            Username = "admin5",
                            Password = "sucks",
                            NickName = "yourdad"
                        },
                        new UserInfo
                        {
                            Username = "admin6",
                            Password = "sucks",
                            NickName = "yourdad"
                        },
                        new UserInfo
                        {
                            Username = "admin7",
                            Password = "sucks",
                            NickName = "yourdad"
                        },
                        new UserInfo
                        {
                            Username = "admin8",
                            Password = "sucks",
                            NickName = "yourdad"
                        },

                    };
                    await appUserRes.AddAsync(user);
                    await appUserRes.AddAsync(users);
                }
                // 如果没有任何角色，则生成种子数据

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred migrating the DB: {ex.Message}");
            }
        }
    }
}