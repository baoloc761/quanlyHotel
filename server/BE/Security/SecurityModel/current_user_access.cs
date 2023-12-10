using System;
using System.Collections.Generic;

namespace Security.SecurityModel
{
  public class current_user_access
  {
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public int UserType { get; set; }
    public string UserTypeName { get; set; }
    public List<Dictionary<string, object>> PermissionList { get; set; }
    public DateTime ExpireTime { get; set; }
    public current_user_access()
    {
      PermissionList= new List<Dictionary<string, object>>();
    }
  }
}
