using CMS.App.Common.Interface;
using CMS.App.Util;
using CMS.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using CMS.App.ReqDto;
using Microsoft.AspNetCore.Authorization;
using CMS.App.ResDto;

namespace CMS.Api.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IRepository<ArticleInfo> _articleTable;
    private readonly IRepository<UserInfo> _userInfoTable;
    private readonly IRepository<CategoryInfo> _categoryInfoTable;
    private readonly IRepository<History> _historyTable;
    private readonly IRepository<LikesInfo> _likeTable;
    private readonly IRepository<UnlikesInfo> _unlikeTable;
    private readonly IRepository<Collections> _collectionsTable;
    private readonly IUserSession _user;
    private readonly IQueryHelper<CategoryInfo> _queryHelper;

    public CategoryController
    (
        IRepository<ArticleInfo> articleTable,
        IRepository<CategoryInfo> categoryInfoTable,
        IRepository<UserInfo> userInfoTable,
        IRepository<History> historyTable,
        IRepository<LikesInfo> likeTable,
        IRepository<UnlikesInfo> unlikeTable,
        IRepository<Collections> collectionsTable,
        IUserSession user,
        IQueryHelper<CategoryInfo> queryHelper
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

    [AllowAnonymous]
    [HttpGet("/ArticleCategory")]
    public dynamic GetCategoryArticleInfoList()
    {   
         var pageIndex = int.Parse(_queryHelper.GetRequestQueryParam("index") ?? "1");
        var pageSize = int.Parse(_queryHelper.GetRequestQueryParam("size") ?? "5");
        int count;
        var data =
            GetArticleInfo
            (
                category =>
                    category.IsDeleted == false
                    && category.IsActived == true
                    && _queryHelper.ValidateData(category),
                out count,
                pageIndex, pageSize
            );

        return new
        {
            code = 1000,
            data = data,
            count = count,
            msg = "????????????"
        }.SerializeObject();
    }
    /// <summary>
    /// ???????????????????????????
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("/category")]
    public dynamic GetCategoryInfoList()
    {   
        var data = _categoryInfoTable.Table.Where(category => category.IsActived == true && category.IsDeleted == false).ToList();
            
        return new
        {
            code = 1000,
            data = data,
            msg = "????????????????????????"
        }.SerializeObject();
    }

    /// <summary>
    /// ????????????????????? ?????????????????????
    /// </summary>
    /// <param name="categoryInfo">?????????????????????????????????????????????</param>
    /// <returns>??????????????????</returns>
    [HttpPost("/category")]
    public async Task<string> CreateCategoryAsync(CategoryInfoDto categoryInfo)
    {

        bool isExists = _categoryInfoTable.Table.Where(category => category.CategoryName == categoryInfo.CategoryName).FirstOrDefault() != null;

        if (isExists == true)
        {
            return new
            {
                code = 3000,
                msg = "????????????????????????",
                data = categoryInfo
            }.SerializeObject();
        }

        var category = await _categoryInfoTable.AddAsync
        (
            new CategoryInfo
            {
                CategoryName = categoryInfo.CategoryName
            }
        );

        return new
        {
            code = 1000,
            msg = "????????????????????????",
            data = category
        }.SerializeObject();
    }

    /// <summary>
    /// ???????????????????????????
    /// </summary>
    /// <param name="categoryId">???????????????????????????id</param>
    /// <returns></returns>
    [HttpDelete("/category/{id}")]
    public async Task<string>  DeleteCategoryInfo(Guid id)
    {
        var category =_categoryInfoTable.GetById(id).Result;

        if (category == null )
        {
            return new
            {
                code = 4000,
                msg = string.Format("Id ??? {0} ??????????????????",id),
                data = category
            }.SerializeObject();
        }

        await _categoryInfoTable.DeleteAsync(category,true);
        return new 
        {
            code = 1000,
            msg = "????????????????????????",
            data = category
        }.SerializeObject();
    }

    [HttpPut("/category/{id}")]
    public async Task<string> UpdateCategoryInfo(Guid id, CategoryInfoDto categoryInfo)
    {
        var category = await _categoryInfoTable.GetById(id);
        
        if (category == null)
        {
            return new
            {
                code = 4000,
                msg = string.Format("Id ??? {0} ??????????????????",id),
                data = category
            }.SerializeObject();
        }

        category = await _categoryInfoTable.UpdateAsync
        (
            new CategoryInfo
            {
                CategoryName = categoryInfo.CategoryName
            }
        );

        return new
        {
            code = 1000,
            msg = "????????????????????????",
            data = category
        }.SerializeObject();
    }
    [NonAction]
    private IList<ArticleInfoDto> GetArticleInfo(Func<CategoryInfo, bool> validateFunc, out int count, int pageIndex = 1, int pageSize = 1)
    {
        var data = _categoryInfoTable.Table.Where(validateFunc);
        count = data.Count();
        // ?????????????????????????????????????????????????????????
        return
            (
                from category in _categoryInfoTable.GetByPageIndex(data.AsQueryable(), pageIndex, pageSize)
                join article in _articleTable.Table on category.Id equals article.CategoryId into articleCategory
                from article in articleCategory.DefaultIfEmpty(_articleTable.Table.First())
                join author in _userInfoTable.Table on article.CreatedBy equals author.Id into articleAuthor
                from author in articleAuthor.DefaultIfEmpty()
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

