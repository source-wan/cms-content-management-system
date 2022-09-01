using CMS.App.Common.Interface;
using CMS.App.ResDto;
using CMS.Domain.Entity;

namespace CMS.Api.Views
{

    public class ArticleInfoView : IDbViews<ArticleInfo, ArticleInfoDto>
    {
        private readonly IQueryHelper<ArticleInfo> _queryHelper;
        private readonly IUserSession _user;
        private readonly IRepository<ArticleInfo> _articleTable;
        private readonly IRepository<UserInfo> _userInfoTable;
        private readonly IRepository<CategoryInfo> _categoryInfoTable;
        private readonly IRepository<LikesInfo> _likeTable;
        private readonly IRepository<UnlikesInfo> _unlikeTable;
        private readonly IRepository<Collections> _collectionsTable;

        public ArticleInfoView
        (
            IQueryHelper<ArticleInfo> queryHelper,
            IUserSession user,
            IRepository<ArticleInfo> articleTable,
            IRepository<UserInfo> userInfoTable,
            IRepository<CategoryInfo> categoryInfoTable,
            IRepository<LikesInfo> likeTable,
            IRepository<UnlikesInfo> unlikeTable,
            IRepository<Collections> collectionsTable
        )
        {
            _queryHelper = queryHelper;
            _user = user;
            _articleTable = articleTable;
            _userInfoTable = userInfoTable;
            _categoryInfoTable = categoryInfoTable;
            _likeTable = likeTable;
            _unlikeTable = unlikeTable;
            _collectionsTable = collectionsTable;
        }

        IList<ArticleInfoDto> IDbViews<ArticleInfo, ArticleInfoDto>.Get(Func<ArticleInfo, bool> validateFunc, out int count, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        ArticleInfoDto IDbViews<ArticleInfo, ArticleInfoDto>.Get(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}