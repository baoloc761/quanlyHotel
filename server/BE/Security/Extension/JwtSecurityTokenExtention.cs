using System.IdentityModel.Tokens.Jwt;

namespace Security.Extension
{
  public static class JwtSecurityTokenExtention
  {
    public static string GetClaimValue(this JwtSecurityToken jwtSecurityToken, string claim_name)
    {
      return jwtSecurityToken.Payload[claim_name].ToString();
    }
  }
}
