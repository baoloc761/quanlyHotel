using System.Collections.Generic;

namespace DataAccess.Model
{
  public class UserType : BaseEntity
  {
    public UserType()
    {
      UserTypeUser = new HashSet<UserTypeUser>();
    }

    public int Type { get; set; }
    public string UserTypeName { get; set; }
    public string Description { get; set; }

    public virtual ICollection<UserTypeUser> UserTypeUser { get; set; }
  }
}
