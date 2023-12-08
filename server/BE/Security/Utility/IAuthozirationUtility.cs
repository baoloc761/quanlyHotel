using Microsoft.AspNetCore.Authorization;
using Security.SecurityModel;
using System.IdentityModel.Tokens.Jwt;

namespace Security
{
  public interface IAuthozirationUtility
  {
    string RenderAccessToken(current_user_access access_user);
    JwtSecurityToken GetRequestAccessToken(AuthorizationHandlerContext context);
  }
}
