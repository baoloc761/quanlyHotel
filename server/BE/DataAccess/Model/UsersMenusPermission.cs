using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Model
{
  public class UsersMenusPermission : BaseEntity
  {
    public Guid UserId { get; set; }
    public Guid MenuId { get; set; }
    public string PermissionList { get; set; }
  }

}
