using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Model
{
  public class User : BaseEntity
  {
    public User()
    {
      UserTypeUser = new HashSet<UserTypeUser>();
    }
    [Required]
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }

    public virtual ICollection<UserTypeUser> UserTypeUser { get; set; }
  }
}
