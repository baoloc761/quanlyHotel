using System;
using System.Collections.Generic;

namespace Security.SecurityModel
{
  public class current_user_access
  {
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public List<string> UserType { get; set; }
    public DateTime ExpireTime { get; set; }
  }
}
