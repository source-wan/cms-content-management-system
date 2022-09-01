using CMS.App.Common.Interface;
using CMS.App.Util;
using CMS.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class UnlikeController : ControllerBase
{
    private readonly IRepository<LikesInfo> _likeTable;
    private readonly IRepository<UnlikesInfo> _unlikeTable;
    private readonly IRepository<ArticleInfo> _articleInfoTable;
    private readonly IUserSession _user;
    private readonly IQueryHelper<History> _queryHelper;

    public UnlikeController
    (
        IRepository<LikesInfo> likeTable,
        IRepository<UnlikesInfo> unlikeTable,
        IRepository<ArticleInfo> articleInfoTable,
        IUserSession user,
        IQueryHelper<History> queryHelper
    )
    {
        _likeTable = likeTable;
        _unlikeTable = unlikeTable;
        _articleInfoTable = articleInfoTable;
        _user = user;
        _queryHelper = queryHelper;
    }

    /// <summary>
    /// 获取当前用户踩过的文章的列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("/unlike")]
    public dynamic GetCurrentUserLikeList()
    {
        var data = _unlikeTable
            .Table
            .Where
            (
                x => x.CreatedBy == _user.Id
                && x.IsActived == true
                && x.IsDeleted == false
            )
            .OrderByDescending(x => x.UpdatedAt)
            .ToList();

        if (data != null)
        {
            var index = int.Parse(_queryHelper.GetRequestQueryParam("index") ?? "1");
            var size = int.Parse(_queryHelper.GetRequestQueryParam("size") ?? "10");
            data = _unlikeTable
                .GetByPageIndex(data.AsQueryable(), pageIndex: index, pageSize: size)
                .ToList();
        }

        return new
        {
            code = 1000,
            data = data,
            msg = "获取用户喜欢的列表成功"
        }.SerializeObject();
    }

    /// <summary>
    /// 给文章点踩
    /// </summary>
    /// <param name="id">被踩的文章id</param>                
    /// <returns></returns>
    [HttpPost("/unlike/{id}")]
    public async Task<dynamic> UnlikeArticleHandle(Guid id)
    {
        // 判断要被点踩的文章是否存在
        bool notExistArticle = _articleInfoTable.GetById(id).Result == null;
        if (notExistArticle)
        {
            return new
            {
                code = 4000,
                data = id,
                msg = string.Format("id为{0}的文章不存在或已经被删除", id)
            }.SerializeObject();
        }

        // 判断用户是否已经给该文章点了踩，因为点赞点踩的取消都不会被软删除，只会被硬删除， 所以不需要相关的判断
        bool isLike = _likeTable.Table.FirstOrDefault(x => x.ArticleId == id && x.CreatedBy == _user.Id) != null;
        if (isLike)
        {
            return new
            {
                code = 4000,
                msg = "无法给点过赞的文章点踩"
            }.SerializeObject();
        }

        // 判断是否重复点踩
        bool isUnlike = _unlikeTable.Table.FirstOrDefault(x => x.ArticleId == id && x.CreatedBy == _user.Id) != null;
        if (isUnlike)
        {
            return new
            {
                code = 4000,
                msg = "你已经给这个文章点过踩了"
            }.SerializeObject();
        }

        await _unlikeTable.AddAsync(new UnlikesInfo { ArticleId = id });
        return new
        {
            code = 1000,
            msg = "点踩成功"
        }.SerializeObject();
    }

    /// <summary>
    /// 移除用户踩的文章记录
    /// </summary>
    /// <param name="id">需要被移除的文章记录的id，不是文章的id而是记录的id</param>
    /// <returns></returns>
    [HttpDelete("/unlike/{id}")]
    public async Task<dynamic> DeleteCurrentUserHistoryAsync(Guid id)
    {
        var unlike = _unlikeTable.GetById(id).Result;
        if (unlike == null)
        {
            return new
            {
                code = 4000,
                data = unlike,
                msg = string.Format("删除失败, id为{0}的数据不存在", id)
            }.SerializeObject();
        }

        await _unlikeTable.DeleteAsync(unlike, true);
        return new
        {
            code = 1000,
            data = unlike,
            msg = "删除成功"
        }.SerializeObject();
    }
}