using BusinessAccess.Services.Interface;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security;
using Security.AuthozirationAttributes;
using Security.SecurityModel;
using System;
using System.Collections.Generic;

namespace HotelManagementCore.Controllers
{
  [Produces("application/json")]
  [Route("api/[controller]/")]
  public class BlogController : Controller
  {
    private readonly IBlogService _blogService;
    private readonly IAuthozirationUtility _securityUtility;
    public BlogController(IBlogService blogService, IAuthozirationUtility securityUtility)
    {
      _blogService = blogService;
      _securityUtility = securityUtility;
    }

    [HttpPost]
    [CustomAuthorization(UserTypeEnum.Staff)]
    [Route("GetAllBlogs")]
    public IActionResult GetAllBlogs()
    {
      var result = _blogService.GetAllBlogs();
      return Json(ApiSecurity.Encode(result));
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("GetAccessToken")]
    public IActionResult GetAccessToken()
    {
      //Nên gọi function này ở Login Service để render access token theo login user
      var AccessToken = _securityUtility.RenderAccessToken(new current_user_access()
      {
        Email = "baoloc761@gmail.com",
        ExpireTime = DateTime.Now.AddYears(1),
        UserName = "baoloc761@gmail.com",
        UserType = new List<string>() { "Staff" }
      });
      var result = Json(AccessToken);
      return result;
    }
  }
}
