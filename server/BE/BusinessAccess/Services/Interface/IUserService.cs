using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BusinessAccess.Services.Interface
{
  public interface IUserService
  {
    Task<List<User>> GetAllUsers(string keyword, bool checkActive = false);
    Task<User> GetUserById(Guid id);
    Task<User> Update(User user);
    Task<User> Add(User user);
    Task Delete(User user);
    // first param: success or not, second param: message, third param: reason (if any)
    Task<(bool, string, string, UserInfo)> CheckLogin(string username, string password);
    Task<List<Menu>> GetListMenu();
  }

  public class UserInfo : BaseEntity
  {
    [Required]
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
  }
}
