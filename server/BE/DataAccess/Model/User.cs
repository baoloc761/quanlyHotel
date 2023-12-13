using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Model
{
  public class User : BaseEntity
  {
    [Required]
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public Guid? AvatarFileId { get; set; }
    public string AvatarImage { get; set; }
    public Guid? IdBackFileId { get; set; }
    public string IdBackImage { get; set; }
    public Guid? IdFrontFileId { get; set; }
    public string IdFrontImage { get; set; }
    public virtual ICollection<UserTypeUser> UserTypeUser { get; set; }
    public virtual ICollection<UserAttachment> Documents { get; set; }
    public User()
    {
      UserTypeUser = new HashSet<UserTypeUser>();
      Documents = new HashSet<UserAttachment>();
    }
  }
}
