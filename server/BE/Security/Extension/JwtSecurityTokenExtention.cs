using System;
using System.IdentityModel.Tokens.Jwt;

namespace Security.Extension
{
  public static class JwtSecurityTokenExtention
  {
    public static string GetClaimValue(this JwtSecurityToken jwtSecurityToken, string claim_name)
    {
      if (jwtSecurityToken == null)
      {
        throw new ArgumentNullException(nameof(jwtSecurityToken));
      }
      try
      {
        return jwtSecurityToken?.Payload[claim_name]?.ToString() ?? "";
      } catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
