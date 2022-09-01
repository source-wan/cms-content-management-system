using CMS.App.Common.Interface;
using CMS.App.ReqDto;
using CMS.App.ResDto;
using CMS.App.Util;
using CMS.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class ArticleController : ControllerBase
{
    private readonly IQueryHelper<ArticleInfo> _queryHelper;
    private readonly IUserSession _user;
    private readonly IRepository<ArticleInfo> _articleTable;
    private readonly IRepository<UserInfo> _userInfoTable;
    private readonly IRepository<CategoryInfo> _categoryInfoTable;
    private readonly IRepository<History> _historyTable;
    private readonly IRepository<LikesInfo> _likeTable;
    private readonly IRepository<UnlikesInfo> _unlikeTable;
    private readonly IRepository<Collections> _collectionsTable;

    public ArticleController
    (
        IRepository<ArticleInfo> articleTable,
        IRepository<UserInfo> userInfoTable,
        IRepository<CategoryInfo> categoryInfoTable,
        IRepository<History> historyTable,
        IRepository<LikesInfo> likeTable,
        IRepository<UnlikesInfo> unlikeTable,
        IRepository<Collections> collectionsTable,
        IQueryHelper<ArticleInfo> queryHelper,
        IUserSession user
    )
    {
        _articleTable = articleTable;
        _userInfoTable = userInfoTable;
        _categoryInfoTable = categoryInfoTable;
        _historyTable = historyTable;
        _likeTable = likeTable;
        _unlikeTable = unlikeTable;
        _collectionsTable = collectionsTable;
        _queryHelper = queryHelper;
        _user = user;
    }


    /// <summary>
    /// 添加一篇文章
    /// </summary>
    /// <param name="articleDto">文章对象</param>
    /// <returns></returns>
    [HttpPost("/article")]
    public async Task<string> Add(ArticleDto articleDto)
    {
        // throw new Exception("你是不是傻逼");
        var news = new ArticleInfo
        {
            Title = articleDto.Title,
            Content = articleDto.Content,
            Remarks = articleDto.Remarks,
            CategoryId = articleDto.CategoryId
        };
        await _articleTable.AddAsync(news);
        var res = new
        {
            code = 1000,
            data = news,
            msg = "新建文章成功"
        };
        return res.SerializeObject();
    }
    /// <summary>
    /// 删除指定的文章
    /// </summary>
    /// <param name="id">需要被删除的文章</param>
    /// <returns></returns>
    [HttpDelete("/article/{id}")]
    public async Task<dynamic> DeleteArticle(Guid id)
    {
        var entity = _articleTable.GetById(id).Result;
        bool shouldHardDel = _queryHelper.GetRequestQueryParam("hard") == "true" ? true : false;
        if (entity == null)
        {
            var res = new
            {
                code = 4000,
                data = entity,
                msg = "无此id文章"
            };
            return res.SerializeObject();
        }
        else
        {
            await _articleTable.DeleteAsync(entity, shouldHardDel);
            var res = new
            {
                code = 1000,
                data = entity,
                msg = "删除文章成功"
            };
            return res.SerializeObject();
        }
    }

    /// <summary>
    /// 更新一篇文章
    /// </summary>
    /// <param name="id">需要更新的文章Id</param>
    /// <param name="model">需要更新为的文章对象</param>
    /// <returns></returns>

    [HttpPut("/article/{id}")]
    public async Task<string> UpdateUser(Guid id, ArticleDto model)
    {
        var entity = _articleTable.GetById(id).Result;
        if (entity != null)
        {
            entity.Title = model.Title;
            entity.Content = model.Content;
            entity.Remarks = model.Remarks;
            entity.CategoryId = model.CategoryId;

            await _articleTable.UpdateAsync(entity);
            var res = new
            {
                code = 1000,
                data = entity,
                msg = "修改成功"
            };
            return res.SerializeObject();
        }
        else
        {
            var res = new
            {
                code = 4000,
                data = entity,
                msg = "参数为空，修改失败"
            };
            return res.SerializeObject();
        }
    }

    /// <summary>
    /// 获取文章列表， 默认一页5条
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("/article")]
    public async Task<dynamic> queryArticle()
    {
        // 文章数据库为空， 随机插入一些字符，方便测试
        if (_articleTable.Table.FirstOrDefault() == null)
        {
            var rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                await _articleTable.AddAsync
                (
                    new ArticleInfo
                    {
                        Title = string.GetHashCode(string.Format("Title{0}", i + rand.Next(114514))).ToString(),
                        Content = string.GetHashCode(string.Format("Content{0}", i + rand.Next(114514))).ToString(),
                        Remarks = string.GetHashCode(string.Format("Remark{0}", i + rand.Next(114514))).ToString(),
                        VisibleCount = rand.Next((i + 1) * 114514)
                    }
                );
            }
        }

        var pageIndex = int.Parse(_queryHelper.GetRequestQueryParam("index") ?? "1");
        var pageSize = int.Parse(_queryHelper.GetRequestQueryParam("size") ?? "5");
        int count;
        var data =
            GetArticleInfo
            (
                article =>
                    article.IsDeleted == false
                    && article.IsActived == true
                    && _queryHelper.ValidateData(article),
                out count,
                pageIndex, pageSize
            );

        return new
        {
            code = 1000,
            data = data,
            count = count,
            msg = "查找成功"
        }.SerializeObject();

        // var pageIndex = int.Parse(_queryHelper.GetRequestQueryParam("index") ?? "1");
        // var pageSize = int.Parse(_queryHelper.GetRequestQueryParam("size") ?? "5");

        // var data =
        //     GetArticleInfo
        //     (
        //         article => 
        //             article.IsDeleted == false 
        //             && article.IsActived == true
        //             && _queryHelper.ValidateData(article),
        //         pageIndex, pageSize
        //     );
        // var count = data.Count();



        // return new
        // {
        //     code = 1000,
        //     data = data,
        //     count = count
        // }.SerializeObject();

    }

    [AllowAnonymous]
    [HttpGet("/hot")]
    public string GetHotArrticle()
    {
        var data =
        (
            from article in _articleTable.Table
            orderby article.VisibleCount descending
            select new
            {
                Id = article.Id,
                Title = article.Title
            }
        ).Take(5);

        return new
        {
            code = 1000,
            data = data,
            msg = "查找成功"
        }.SerializeObject();
    }

    /// <summary>
    /// 获取文章的具体内容
    /// </summary>
    /// <param name="id">需要获取的文章id</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("/article/{id}")]
    public async Task<dynamic> GetArticleInfo(Guid id)
    {
        int count;
        var data = GetArticleInfo(article => article.Id == id && article.IsActived == true && article.IsDeleted == false, out count).FirstOrDefault();

        if (data == null)
        {
            return new
            {
                code = 4000,
                msg = string.Format("id为{0}的文章不存在", id)
            }.SerializeObject();
        }
        // 决定是向历史记录中添加记录，或是更新该记录，使其和用户访问的时间保持一致
        // 在大部分情况下，我们不需要知道用户访问该文章的精确时间，只需要确保其访问的顺序不出问题
        var history = _historyTable.Table.FirstOrDefault(x => x.CreatedBy == _user.Id && x.ArticleId == id);
        // 当用户第一次访问该文章时，向 History 添加记录，同时让文章的点击量+1 否则只更新 UpdateAt
        if (history == null)
        {
            await _historyTable.AddAsync(new History { ArticleId = id });
            var article = _articleTable.GetById(data.Id).Result;
            if (article != null)
            {
                article.VisibleCount++;
                await _articleTable.UpdateAsync(article);
            }
        }
        else
        {
            history.IsDeleted = false;
            history.IsActived = true;
            await _historyTable.UpdateAsync(history);
        }

        return new
        {
            code = 1000,
            data = data
        }.SerializeObject();
    }

    /// <summary>
    /// *这不是一个 Action*
    /// 该函数会获取文章（列表）的相关信息并连着文章本身的信息返回
    /// </summary>
    /// <param name="validateFunc">校验器</param>
    /// <param name="pageIndex">页面索引</param>
    /// <param name="pageSize">页面大小</param>
    /// <returns>文章详细信息的集合</returns>
    [NonAction]
    private IList<ArticleInfoDto> GetArticleInfo(Func<ArticleInfo, bool> validateFunc, out int count, int pageIndex = 1, int pageSize = 1)
    {
        var data = _articleTable.Table.Where(validateFunc);
        count = data.Count();
        // 使用连接查询获取到文章的相关数据并返回
        return
            (
                from article in _articleTable.GetByPageIndex(data.AsQueryable(), pageIndex, pageSize)
                join author in _userInfoTable.Table on article.CreatedBy equals author.Id into articleAuthor
                from author in articleAuthor.DefaultIfEmpty()
                join category in _categoryInfoTable.Table on article.CategoryId equals category.Id into articleCategory
                from category in articleCategory.DefaultIfEmpty(new CategoryInfo { CategoryName = "Default" })
                join like in _likeTable.Table on article.Id equals like.ArticleId into articleLike
                let likeCount = articleLike.Count()
                join unlike in _unlikeTable.Table on article.Id equals unlike.ArticleId into articleUnlike
                let unlikeCount = articleUnlike.Count()
                join colletcted in _collectionsTable.Table on article.Id equals colletcted.ArticleId into articleColletcion
                let collectionCount = articleColletcion.Count()
                select new ArticleInfoDto
                {
                    Id = article.Id,
                    AuthorId = article.CreatedBy,
                    AuthorName = author.Username,
                    AuthorNickName = author.NickName,
                    AuthorAvatar = author.Avatar,
                    CategoryId = category.Id,
                    CategoryName = category.CategoryName,
                    Title = article.Title,
                    Content = article.Content,
                    Visible = article.VisibleCount,
                    LikeCount = likeCount,
                    UnlikeCount = unlikeCount,
                    CollectionCount = collectionCount,
                    Remark = article.Remarks,
                    PublishAt = article.CreatedAt,
                    UpdatedAt = article.UpdatedAt,
                    UpdatedBy = article.UpdatedBy,
                }
            ).ToList();

    }
}
