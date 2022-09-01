using CMS.App.Common.Interface;
using CMS.App.ReqDto;
using CMS.App.Util;
using CMS.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserManageController : ControllerBase
{
    private readonly IRepository<UserInfo> _userRepository;

    private readonly IQueryHelper<UserInfo> _queryValidate;


    public UserManageController(IRepository<UserInfo> userRepository, IQueryHelper<UserInfo> queryValidate)
    {
        _userRepository = userRepository;
        _queryValidate = queryValidate;
    }

    //查找用户
    [AllowAnonymous]
    [HttpGet("/user")]
    public dynamic queryUser()
    {
        var pageIndex = int.Parse(_queryValidate.GetRequestQueryParam("index") ?? "1");
        var pageSize = int.Parse(_queryValidate.GetRequestQueryParam("size") ?? "5");

        var data = Request.Query.Count == 0
            ? _userRepository.Table.ToList()
            : _userRepository.Table.Where(_queryValidate.ValidateData).ToList();

        data = data.Where(x => x.IsActived == true && x.IsDeleted == false).ToList();
        var count = data.Count();
        data = _userRepository.GetByPageIndex(data.AsQueryable(), pageIndex, pageSize).ToList();
        return new
        {
            code = 1000,
            data = new
            {
                data = data,
                count = count
            },
            msg = "查找成功"
        }.SerializeObject();
    }

    // 根据用户id查找用户
    [AllowAnonymous]
    [HttpGet("/user/{id}")]
    public async Task<string> GetUserById(Guid id)
    {
        var user = await _userRepository.GetById(id);
        if (user == null)
        {
            return new
            {
                code = 4000,
                data = user,
                msg = "用户不存在"
            }.SerializeObject();
        }

        return new
        {
            code = 1000,
            data = user,
            msg = "查询成功"
        }.SerializeObject();
    }

    //修改用户
    [AllowAnonymous]
    [HttpPut("/user/{id}")]
    public async Task<string> UpdateUser(Guid id, PutUserDto model)
    {
        var entity = _userRepository.GetById(id).Result;
        if (entity != null)
        {
            entity.Username = model.Username;
            entity.Password = model.Password;
            entity.Avatar = model.Avatar;
            entity.NickName = model.NickName;
            entity.isAdmin = model.isAdmin;

            await _userRepository.UpdateAsync(entity);
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
    //删除用户
    [AllowAnonymous]
    [HttpDelete("/user/{id}")]
    public async Task<dynamic> DeleteUser(Guid id, bool shoudHardDel = false)
    {
        var entity = _userRepository.GetById(id).Result;
        if (entity == null)
        {
            var res = new
            {
                code = 4000,
                data = entity,
                msg = "无此id用户"
            };
            return res.SerializeObject();
        }
        else
        {
            await _userRepository.DeleteAsync(entity, shoudHardDel);
            var res = new
            {
                code = 1000,
                data = entity,
                msg = "删除用户成功"
            };
            return res.SerializeObject();
        }
    }

}