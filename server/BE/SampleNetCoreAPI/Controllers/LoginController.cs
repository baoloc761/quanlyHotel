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

namespace SampleNetCoreAPI.Controllers
{
  [Produces("application/json")]
  [Route("api/[controller]/")]
  public class LoginController : Controller
  {
    private readonly IUserService _userService;

    public LoginController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpGet, Route("menus-list")]
    [AllowAnonymous]
    public async Task<IActionResult> GetMenuList()
    {
      try
      {
        var menusList = await _userService.GetListMenu();
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
          var secretKey = new SymmetricSecurityKey
          (Encoding.UTF8.GetBytes("thisisasecretkey@123"));
          var signinCredentials = new SigningCredentials
          (secretKey, SecurityAlgorithms.HmacSha256);
          var jwtSecurityToken = new JwtSecurityToken(
              issuer: loginDTO.UserName,
              audience: "http://localhost:59688",
              claims: new List<Claim>(),
              expires: DateTime.Now.AddDays(1),
              signingCredentials: signinCredentials
          );
          var basicUserInfo = checkLogin.Item4;
          return Ok(new
          {
            status = 200,
            message = checkLogin.Item2,
            userInfo = basicUserInfo,
            token = new JwtSecurityTokenHandler().
          WriteToken(jwtSecurityToken)
          });
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
