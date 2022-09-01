using CMS.App.Common.Interface;
using CMS.App.ReqDto;
using CMS.App.ResDto;
using CMS.App.Util;
using CMS.Common.Interface;
using CMS.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IRepository<UserInfo> _userRepository;

    private readonly IIdentity _identityService;

    public UserController(IRepository<UserInfo> userRepository, IIdentity identityService)
    {
        _userRepository = userRepository;
        _identityService = identityService;
    }

    //注册
    [AllowAnonymous]
    [HttpPost("/reg")]
    public async Task<string> Reg(RegisterUserDto userDto)
    {
        // throw new Exception("你是不是傻逼");
        var appUser = new UserInfo
        {
            Username = userDto.Username,
            Password = userDto.Password,
            NickName=userDto.NickName,
            isAdmin=userDto.isAdmin
        };

        if (_userRepository.Table.FirstOrDefault(x => x.Username == appUser.Username) != null)
        {
            return new
            {
                code = 4000,
                data = appUser,
                msg = string.Format("用户名: {0} 已被注册", appUser.Username)
            }.SerializeObject();
        }

        if (appUser.Username.Length > 16 || appUser.Password.Length < 8)
            {
                return new
                {
                    code = 4000,
                    data = appUser,
                    msg =
                        string.Format
                        (
                            "无效的用户名或密码: 用户名:{0}的长度大于16 或(及)密码:{1}的长度小于8",
                            appUser.Username, appUser.Password
                        )

                }.SerializeObject();
            }

        await _userRepository.AddAsync(appUser);
        var res = new
        {
            code = 1000,
            data = appUser,
            msg = "注册成功"
        };
        return res.SerializeObject();
    }

    //登录
    [AllowAnonymous]
    [HttpPost("/login")]
    public async Task<string> Login(LoginUserDto loginDto)
    {

        var token = await _identityService.ValidateUserAsync(loginDto);
        
        if (token.AccessToken != null && token.RefreshToken != null)
        {
            var res = new
            {
                code = 1000,
                data = token,
                msg = "登录成功"
            };
            return res.SerializeObject();
        }
        else
        {
            var res = new
            {
                code = 4000,
                data = token,
                msg = "用户名或者密码错误,请确认重试"
            };
            return res.SerializeObject();
        }
    }

    //验证管理员
    [HttpGet("/admin/{id}")]
    public string ValidateUser(Guid id)
    {
        var user=_userRepository.GetById(id).Result;
        if(user!=null&&user.isAdmin==true)
        {
            var res=new
            {
                code=2000,
                data=user,
                msg="用户为管理员"
            };
            return res.SerializeObject();
        }else
        {
            var res=new
            {
                code=4000,
                data=user,
                msg="无此用户或者无权限"
            };
            return res.SerializeObject();
        }
    }

    [AllowAnonymous]
    [HttpPost("/refresh")]
    public async Task<string> RefreshToken(TokenDto tokenDto)
    {
        var token = await _identityService.GenerateRefreshToken(tokenDto);
        if (token != null)
        {
            var res = new
            {
                code = 1000,
                data = token,
                msg = "刷新成功"
            };
            return res.SerializeObject();
        }
        else
        {
            var res = new
            {
                code = 4000,
                data = token,
                msg = "请确认重试"
            };
            return res.SerializeObject();
        }

    }
}

