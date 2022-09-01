using CMS.App.Common.Interface;
using CMS.App.Util;
using CMS.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class HistoryController : ControllerBase
{
    private readonly IRepository<History> _historyTable;
    private readonly IRepository<ArticleInfo> _articleTable;
    private readonly IRepository<UserInfo> _userInfoTable;
    private readonly IUserSession _user;
    private readonly IQueryHelper<History> _queryHelper;

    public HistoryController
    (
        IRepository<History> historyTable, 
        IRepository<ArticleInfo> articleTable, 
        IRepository<UserInfo> userInfoTable, 
        IUserSession user, 
        IQueryHelper<History> queryHelper)
    {
        _historyTable = historyTable;
        _articleTable = articleTable;
        _userInfoTable = userInfoTable;
        _user = user;
        _queryHelper = queryHelper;
    }
    /// <summary>
    /// 获取历史记录列表， 可以通过查询字符串选择查看第几页， 默认一页10条
    /// </summary>
    /// <returns></returns>
    [HttpGet("/history")]
    public dynamic GetCurrentUserHistory()
    {
        var data = _historyTable
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
                from history in data
                join article in _articleTable.Table on history.ArticleId equals article.Id into historyArticle
                from article in historyArticle.DefaultIfEmpty()
                join author in _userInfoTable.Table on article.CreatedBy equals author.Id into articleAuthor
                from author in articleAuthor.DefaultIfEmpty()
                select new
                {
                    Id = history.Id,
                    AuthorId = author.Id,
                    AuthorName = author.Username,
                    AuthorNickName = author.NickName,
                    AuthorAvatar = author.Avatar,
                    ArticleId = article.Id,
                    Title = article.Title,
                    Remark = article.Remarks,
                    CreatedAt = history.CreatedAt
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
            msg = "获取历史记录列表成功"
        }.SerializeObject();
    }

    /// <summary>
    /// 移除指定的历史记录
    /// </summary>
    /// <param name="id">需要被删除的历史记录的Id</param>
    /// <returns></returns>
    [HttpDelete("/history/{id}")]
    public async Task<dynamic> DeleteCurrentUserHistoryAsync(Guid id)
    {
        var history = _historyTable.GetById(id).Result;
        if (history == null)
        {
            return new
            {
                code = 4000,
                data = history,
                msg = string.Format("删除失败, id为{0}的数据不存在", id)
            }.SerializeObject();
        }

        await _historyTable.DeleteAsync(history);
        return new
        {
            code = 1000,
            data = history,
            msg = "删除成功"
        }.SerializeObject();
    }
}