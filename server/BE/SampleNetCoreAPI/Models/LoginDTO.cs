using DataAccess.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace SampleNetCoreAPI.Models
{
  public class LoginDTO
  {
    public string UserName { get; set; }
    public string Password { get; set; }
  }

  public class RoleDTO
  {
    public Guid Id { get; set;}
    public int Type { get; set;}
    public string Name { get; set;}
    public string Description { get; set; }
  }

  public class RegistUserDTO
  {
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
  }
}
