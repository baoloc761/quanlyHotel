using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Linq;

namespace SampleNetCoreAPI.Helper
{
  public static class AuthenticationUtils
  {
    public static Guid getUserId(Microsoft.AspNetCore.Http.HttpRequest request)
    {
      StringValues tokenString = "";
      request.Headers.TryGetValue("Authorization", out tokenString);
      var token = new JwtSecurityToken(jwtEncodedString: tokenString.First().Replace("Bearer ", ""));
      var userId = token?.Payload["email"]?.ToString() ?? "";
      return new Guid(userId);
    }
  }
}
