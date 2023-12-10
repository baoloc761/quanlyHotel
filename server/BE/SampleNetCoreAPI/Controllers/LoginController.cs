using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SampleNetCoreAPI.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.AspNetCore.Authorization;
using Security;
using BusinessAccess.Services.Interface;
using System.Threading.Tasks;
using Security.SecurityModel;
using System.Linq;
using Common;
using Security.AuthozirationAttributes;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using System.Diagnostics.Tracing;

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
      StringValues tokenString = "";
      Request.Headers.TryGetValue("Authorization", out tokenString);
      var token = new JwtSecurityToken(jwtEncodedString: tokenString.First().Replace("Bearer ", ""));
      var userId = token?.Payload["email"]?.ToString() ?? "";
      try
      {
        var menusList = await _userService.GetListMenu(new Guid(userId));
        return Ok(new
        {
          status = 200,
          message = "GetMenusListSuccess",
          data = menusList
        });
      }
      catch (Exception ex)
      {
        return Ok(new
        {
          status = 404,
          message = "GetMenusListFailed"
        });
      }
    }

    [HttpGet, Route("users-list")]
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
      catch (Exception ex)
      {
        return Ok(new
        {
          status = 404,
          message = "GetUsersListFailed"
        });
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
          var AccessToken = _securityUtility.RenderAccessToken(new current_user_access()
          {
            Email = basicUserInfo.Email,
            ExpireTime = DateTime.Now.AddDays(1),
            UserName = basicUserInfo.UserName,
            UserType = basicUserInfo.UserTypeUser.FirstOrDefault().UserType?.Type ?? (int)UserTypeEnum.Staff,
            UserTypeName = basicUserInfo.UserTypeUser.FirstOrDefault().UserType?.UserTypeName ?? string.Empty,
            UserId = basicUserInfo.Id,
            PermissionList = basicUserInfo.PermissionList
          });
          var result = Json(AccessToken);
          return result;
        }

        return Ok(new
        {
          status = 404,
          message = checkLogin.Item2,
          reason = checkLogin.Item3
        });
      }
      catch (Exception ex)
      {
        return BadRequest
        ("ErrorGenerateToken");
      }
    }
  }
}
