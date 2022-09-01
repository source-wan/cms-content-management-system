using CMS.App.Common.Interface;
using CMS.App.Util;
using CMS.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class LikeController : ControllerBase
{
    private readonly IRepository<LikesInfo> _likeTable;
    private readonly IRepository<UnlikesInfo> _unlikeTable;
    private readonly IRepository<ArticleInfo> _articleInfoTable;
    private readonly IRepository<UserInfo> _userInfoTable;
    private readonly IUserSession _user;
    private readonly IQueryHelper<History> _queryHelper;

    public LikeController
    (
        IRepository<LikesInfo> likeTable,
        IRepository<UnlikesInfo> unlikeTable,
        IRepository<ArticleInfo> articleInfoTable,
        IRepository<UserInfo> userInfoTable,
        IUserSession user,
        IQueryHelper<History> queryHelper
    )
    {
        _likeTable = likeTable;
        _unlikeTable = unlikeTable;
        _articleInfoTable = articleInfoTable;
        _userInfoTable = userInfoTable;
        _user = user;
        _queryHelper = queryHelper;
    }

    /// <summary>
    /// 获取当前用户喜欢的文章的列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("/like")]
    public dynamic GetCurrentUserLikeList()
    {
        var data = _likeTable
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
                from like in data
                join article in _articleInfoTable.Table on like.ArticleId equals article.Id into likeArticle
                from article in likeArticle.DefaultIfEmpty()
                join author in _userInfoTable.Table on article.CreatedBy equals author.Id into articleAuthor
                from author in articleAuthor.DefaultIfEmpty()
                select new
                {
                    Id = like.Id,
                    AuthorId = author.Id,
                    AuthorName = author.Username,
                    AuthorNickName = author.NickName,
                    AuthorAvatar = author.Avatar,
                    ArticleId = article.Id,
                    Title = article.Title,
                    Remark = article.Remarks,
                    CreatedAt = like.CreatedAt
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
            msg = "获取用户喜欢的列表成功"
        }.SerializeObject();
    }

    /// <summary>
    /// 给文章点赞
    /// </summary>
    /// <param name="id">被点赞的文章id</param>                
    /// <returns></returns>
    [HttpPost("/like/{id}")]
    public async Task<dynamic> LikeArticleHandle(Guid id)
    {
        // 判断要被点赞的文章是否存在
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
        bool isUnlike = _unlikeTable.Table.FirstOrDefault(x => x.ArticleId == id && x.CreatedBy == _user.Id) != null;
        if (isUnlike)
        {
            return new
            {
                code = 4000,
                msg = "无法给被踩的文章点赞"
            }.SerializeObject();
        }

        // 判断是否重复点赞
        bool isLike = _likeTable.Table.FirstOrDefault(x => x.ArticleId == id && x.CreatedBy == _user.Id) != null;
        if (isLike)
        {
            return new
            {
                code = 4000,
                msg = "你已经给这个文章点过赞了"
            }.SerializeObject();
        }

        await _likeTable.AddAsync(new LikesInfo { ArticleId = id });
        return new
        {
            code = 1000,
            msg = "点赞成功"
        }.SerializeObject();
    }

    /// <summary>
    /// 移除用户喜欢的文章记录
    /// </summary>
    /// <param name="id">需要被移除的文章记录的id，不是文章的id而是记录的id</param>
    /// <returns></returns>
    [HttpDelete("/like/{id}")]
    public async Task<dynamic> DeleteCurrentUserHistoryAsync(Guid id)
    {
        var like = _likeTable.GetById(id).Result;
        if (like == null)
        {
            return new
            {
                code = 4000,
                data = like,
                msg = string.Format("删除失败, id为{0}的数据不存在", id)
            }.SerializeObject();
        }

        await _likeTable.DeleteAsync(like, true);
        return new
        {
            code = 1000,
            data = like,
            msg = "删除成功"
        }.SerializeObject();
    }
}