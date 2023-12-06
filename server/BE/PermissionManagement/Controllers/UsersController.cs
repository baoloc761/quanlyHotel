using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PermissionManagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PermissionManagement.Controllers
{
  [Authorize]
  public class UsersController : Controller
  {
    private readonly UserManager<IdentityUser> _userManager;
    public UsersController(UserManager<IdentityUser> userManager)
    {
      _userManager = userManager;
    }
    public async Task<IActionResult> GetUsersList(string keyword = null)
    {
      var currentUser = await _userManager.GetUserAsync(HttpContext.User);
      var result = await _userManager.Users
        .Where(a => string.IsNullOrEmpty(keyword) || a.UserName.ToLower().IndexOf(keyword.ToLower()) >= 0 ||
           a.Email.ToLower().IndexOf(keyword.ToLower()) >= 0)
        .ToListAsync();

      return Ok(new HotelResponseMessage(code: "200", message: "Get Users List Successfull", result));
    }
  }
}
