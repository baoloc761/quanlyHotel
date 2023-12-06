using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PermissionManagement.Models;
using System.Threading.Tasks;

namespace PermissionManagement.Controllers
{
  [Authorize(Roles = "SuperAdmin")]
  public class RolesController : Controller
  {
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolesController(RoleManager<IdentityRole> roleManager)
    {
      _roleManager = roleManager;
    }

    public async Task<IActionResult> GetRolesList()
    {
      var roles = await _roleManager.Roles.ToListAsync();
      return Ok(new HotelResponseMessage(code: "200", message: "Get Roles List Successfull", roles));
    }
    [HttpPost]
    public async Task<IActionResult> AddRole(string roleName)
    {
      if (roleName != null)
      {
        await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
      }
      return Ok(new HotelResponseMessage(code: "200", message: "Add Role Successfull", new { roleName }));
    }
  }
}
