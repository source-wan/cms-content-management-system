using CMS.App.Common.Interface;
using CMS.App.Util;
using CMS.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class CollectionController : ControllerBase
{
    private readonly IRepository<Collections> _collectionTable;
    private readonly IRepository<ArticleInfo> _articleTable;
    private readonly IRepository<UserInfo> _userInfoTable;
    private readonly IUserSession _user;
    private readonly IQueryHelper<History> _queryHelper;

    public CollectionController
    (
        IRepository<Collections> collectionTable,
        IRepository<ArticleInfo> articleTable,
        IRepository<UserInfo> userInfoTable,
        IUserSession user,
        IQueryHelper<History> queryHelper
    )
    {
        _collectionTable = collectionTable;
        _articleTable = articleTable;
        _userInfoTable = userInfoTable;
        _user = user;
        _queryHelper = queryHelper;
    }

    /// <summary>
    /// 获取当前用户的收藏夹列表，默认情况下一页五条
    /// </summary>
    /// <returns></returns>
    [HttpGet("/collection")]
    public dynamic GetCurrentUserCollection()
    {
        var data = _collectionTable
            .Table
            .Where
            (
                x => x.CreatedBy == _user.Id
                && x.IsActived == true
                && x.IsDeleted == false
            )
            .OrderByDescending(x => x.UpdatedAt)
            .ToList();

        int count = data.Count();

        var res =
            (
                from collect in data
                join article in _articleTable.Table on collect.ArticleId equals article.Id into collectedArticle
                from article in collectedArticle.DefaultIfEmpty()
                join author in _userInfoTable.Table on article.CreatedBy equals author.Id into articleAuthor
                from author in articleAuthor.DefaultIfEmpty()
                select new
                {
                    Id = collect.Id,
                    AuthorId = author.Id,
                    AuthorName = author.Username,
                    AuthorNickName = author.NickName,
                    AuthorAvatar = author.Avatar,
                    ArticleId = article.Id,
                    Title = article.Title,
                    Remark = article.Remarks,
                    CreatedAt = collect.CreatedAt
                }
            ).ToList();

        if (res != null)
        {
            var index = int.Parse(_queryHelper.GetRequestQueryParam("index") ?? "1");
            var size = int.Parse(_queryHelper.GetRequestQueryParam("size") ?? "10");
            res = res.Skip((index - 1) * size).Take(size).ToList();
        }

        return new
        {
            code = 1000,
            data = res,
            count = count,
            msg = "获取用户的收藏列表成功"
        }.SerializeObject();
    }

    /// <summary>
    /// 收藏文章
    /// </summary>
    /// <param name="id">被收藏的文章id</param>
    /// <returns></returns>
    [HttpPost("/colletcion/{id}")]
    public async Task<dynamic> ColletcionArticleHandle(Guid id)
    {

        // 判断需要被收藏的文章是否存在
        bool notExistArticle = _articleTable.GetById(id).Result == null;
        if (notExistArticle)
        {
            return new
            {
                code = 4000,
                data = id,
                msg = string.Format("id为{0}的文章不存在", id)
            }.SerializeObject();
        }

        // 判断该文章是否已经在用户的收藏夹中了
        bool isColletcion = _collectionTable.Table.FirstOrDefault(x => x.ArticleId == id && x.CreatedBy == _user.Id) != null;
        if (isColletcion)
        {
            return new
            {
                code = 4000,
                msg = "你已经收藏过这篇文章了"
            }.SerializeObject();
        }

        await _collectionTable.AddAsync(new Collections { ArticleId = id });
        return new
        {
            code = 1000,
            msg = "收藏成功"
        }.SerializeObject();
    }

    /// <summary>
    /// 取消收藏某篇文章
    /// </summary>
    /// <param name="id">需要被取消收藏的Id， 同意不是文章的id</param>
    /// <returns></returns>
    [HttpDelete("/collection/{id}")]
    public async Task<dynamic> DeletCurrentUserColletc(Guid id)
    {
        var history = _collectionTable.GetById(id).Result;
        if (history == null)
        {
            return new
            {
                code = 4000,
                data = history,
                msg = string.Format("取消收藏失败, 该用户没有收藏id为{0}的文章", id)
            }.SerializeObject();
        }

        await _collectionTable.DeleteAsync(history, true);
        return new
        {
            code = 1000,
            data = history,
            msg = "取消收藏成功"
        }.SerializeObject();
    }
}