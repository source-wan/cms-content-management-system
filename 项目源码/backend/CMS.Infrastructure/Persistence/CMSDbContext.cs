using System.Reflection;
using Microsoft.EntityFrameworkCore;
using CMS.Domain.Entity;
using FileInfo = CMS.Domain.Entity.FileInfo;

namespace CMS.Infrastructure.Persistence
{
    public class CMSDbContext : DbContext
    {
        public CMSDbContext(DbContextOptions options) : base(options: options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        public DbSet<UserInfo> UserInfo => Set<UserInfo>();
        public DbSet<IdentityUser> IdentityUsers => Set<IdentityUser>();
        public DbSet<FileInfo> FileInfo => Set<FileInfo>();
        public DbSet<History> History => Set<History>();
        public DbSet<UnlikesInfo> UnlikesInfo => Set<UnlikesInfo>();
        public DbSet<LikesInfo> LikesInfo => Set<LikesInfo>();
        public DbSet<Collections> Collections => Set<Collections>();
        public DbSet<Comments> Comments => Set<Comments>();
        public DbSet<CategoryInfo> CategoryInfos => Set<CategoryInfo>();
        public DbSet<ArticleInfo> ArticleInfo => Set<ArticleInfo>();
    }
}