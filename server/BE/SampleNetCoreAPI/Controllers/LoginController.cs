using Microsoft.AspNetCore.Mvc;
using SampleNetCoreAPI.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using Security;
using BusinessAccess.Services.Interface;
using System.Threading.Tasks;
using Security.SecurityModel;
using System.Linq;
using SampleNetCoreAPI.Helper;
using Security.AuthozirationAttributes;
using Common;
using System.Collections.Generic;

namespace SampleNetCoreAPI.Controllers
{
  [Produces("application/json")]
  [Route("api/[controller]/")]
  public class LoginController : Controller
  {
    private readonly IUserService _userService;
    private readonly IAuthozirationUtility _securityUtility;

    public LoginController(IUserService userService, IAuthozirationUtility securityUtility)
    {
      _userService = userService;
      _securityUtility = securityUtility;
    }

    [HttpGet, Route("menus-list")]
    public async Task<IActionResult> GetMenuList()
    {
      try
      {
        var menusList = await _userService.GetListMenu(AuthenticationUtils.getUserId(Request));
        return Ok(new
        {
          status = 200,
          message = "GetMenusListSuccess",
          data = menusList
        });
      }
      catch {
        return Ok(new
        {
          status = 404,
          message = "GetMenusListFailed"
        });
      }
    }

    [HttpGet, Route("detail")]
    public async Task<IActionResult> GetUserDetail()
    {
      try
      {
        var users = await _userService.Detail(AuthenticationUtils.getUserId(Request));
        return Ok(new
        {
          status = 200,
          message = "GetUserDetailSuccess",
          data = users.FirstOrDefault()
        });
      }
      catch
      {
        return BadRequest("GetUserDetailFailed");
      }
    }
    [HttpGet, Route("user")]
    public async Task<IActionResult> GetUserById([FromQuery] Guid id)
    {
      try
      {
        var users = await _userService.Detail(id);
        return Ok(new
        {
          status = 200,
          message = "GetUserDetailSuccess",
          data = users.FirstOrDefault()
        });
      }
      catch
      {
        return BadRequest("GetUserDetailFailed");
      }
    }

    [HttpGet, Route("roles-list")]
    [CustomAuthorization(UserTypeEnum.Administrator, UserTypeEnum.Manager)]
    public async Task<IActionResult> GetRolesList()
    {
      try
      {
        var roles = await _userService.GetRolesList(AuthenticationUtils.getUserId(Request));
        List<RoleDTO> rolesList = new List<RoleDTO>();
        foreach (var role in roles)
        {
          rolesList.Add(new RoleDTO
          {
            Id= role.Id,
            Type= role.Type,
            Name = role.UserTypeName,
            Description = role.Description
          });
        }
        return Ok(new
        {
          status = 200,
          message = "GetRolesListSuccess",
          data = rolesList
        });
      }
      catch
      {
        return BadRequest("GetRolesListFailed");
      }
    }

    [HttpGet, Route("users-list")]
    [CustomAuthorization(UserTypeEnum.Administrator, UserTypeEnum.Manager)]
    public async Task<IActionResult> GetUsersList([FromQuery] string keyword = "")
    {
      try
      {
        var users = await _userService.GetAllUsers(keyword);
        return Ok(new
        {
          status = 200,
          message = "GetUsersListSuccess",
          data = users
        });
      }
      catch
      {
        return BadRequest("GetUsersListFailed");
      }
    }

    [HttpPost, Route("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDTO loginDTO)
    {
      try
      {
        if (string.IsNullOrEmpty(loginDTO.UserName) ||
        string.IsNullOrEmpty(loginDTO.Password))
          return BadRequest("UserNamePasswordNotSpecified");

        var checkLogin = await _userService.CheckLogin(loginDTO.UserName, loginDTO.Password);
        if (checkLogin.Item1)
        {
          var basicUserInfo = checkLogin.Item4;
          var currentUserAccess = new current_user_access()
          {
            Email = basicUserInfo.Email,
            ExpireTime = DateTime.Now.AddDays(1),
            UserName = basicUserInfo.UserName,
            UserType = basicUserInfo.Type,
            UserTypeName = basicUserInfo.UserTypeName ?? string.Empty,
            UserId = basicUserInfo.Id,
            PermissionList = basicUserInfo.PermissionList
          };
          var accessToken = _securityUtility.RenderAccessToken(currentUserAccess);
          return Ok(new
          {
            status = 200,
            message = checkLogin.Item2,
            token = Json(accessToken).Value,
            userInfo = currentUserAccess
          });
        }

        return Ok(new
        {
          status = 404,
          message = checkLogin.Item2,
          reason = checkLogin.Item3
        });
      }
      catch
      {
        return BadRequest
        ("ErrorGenerateToken");
      }
    }
  }
}
