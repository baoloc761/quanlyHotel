using AutoMapper;
using BusinessAccess.Repository;
using BusinessAccess.Services.Interface;
using Common;
using DataAccess.Model;
using log4net;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessAccess.Services.Implement
{
  public class UserService : IUserService
  {
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<UserType> _userTypesRepository;
    private readonly IRepository<UsersMenusPermission> _usersMenusPermissionRepository;
    private readonly IRepository<Menu> _menuRepository;
    private readonly IMapper _mapper;
    private readonly ILog _logger;

    public UserService(IRepository<User> userRepository, IRepository<UserType> userTypesRepository, IRepository<UsersMenusPermission> usersMenusPermissionRepository, IRepository<Menu> menuRepository, IMapper mapper)
    {
      _userRepository = userRepository;
      _userTypesRepository = userTypesRepository;
      _menuRepository = menuRepository;
      _usersMenusPermissionRepository = usersMenusPermissionRepository;
      _logger = LogManager.GetLogger(typeof(UserService));
      _mapper = mapper;
    }

    public async Task<List<UserType>> GetRolesList(Guid userId)
    {
      var currentUser = (await Detail(userId)).FirstOrDefault();
      if (currentUser == null)
      {
        throw new NullReferenceException("NotExistedUser");
      }

      switch (currentUser.Type)
      {
        case (int)UserTypeEnum.Staff: return new List<UserType>();
        case (int)UserTypeEnum.Manager: return await _userTypesRepository.Filter(x => x.Type == (int)UserTypeEnum.Staff).OrderBy(x => x.Type).ToListAsync();
        case (int)UserTypeEnum.Administrator: return await _userTypesRepository.GetAll().OrderBy(x => x.Type).ToListAsync();
        default: return new List<UserType>();
      }
    }

    public async Task<List<UserInfo>> GetAllUsers(string keyword, bool checkActive = false)
    {
      Expression<Func<User, bool>> filter = (x) => string.IsNullOrEmpty(keyword)
        || x.Id.ToString().ToLower().IndexOf(keyword.ToLower()) != -1
        || x.Email.ToString().ToLower().IndexOf(keyword.ToLower()) != -1
        || x.FirstName.ToString().ToLower().IndexOf(keyword.ToLower()) != -1
        || x.LastName.ToString().ToLower().IndexOf(keyword.ToLower()) != -1;
      var result = _userRepository.Filter(filter);
      if (checkActive)
      {
        result = result.Where(x => x.Active);
      }
      var users = await result.Include(x => x.UserTypeUser).ThenInclude(x => x.UserType).ToListAsync();
      var usersInfoList = _mapper.Map<List<User>, List<UserInfo>>(users);
      foreach (var user in usersInfoList)
      {
        user.PermissionList = await updatePermissionList(user);
        var userType = users.FirstOrDefault(x => x.Id == user.Id).UserTypeUser.FirstOrDefault().UserType;
        user.Type = userType.Type;
        user.UserTypeId = userType.Id;
        user.UserTypeName = userType.UserTypeName;
      }
      return usersInfoList;
    }
    public async Task<User> GetUserById(Guid id)
    {
      return await _userRepository.GetAsync(id);
    }
    public async Task<User> Update(User user)
    {
      await _userRepository.UpdateAsync(user);
      Expression<Func<User, bool>> filter = (x) => x.Id == user.Id;
      return await _userRepository.Filter(filter).FirstOrDefaultAsync();
    }
    public async Task<User> Add(User user)
    {
      await _userRepository.InsertAsync(user);
      Expression<Func<User, bool>> filter = (x) => x.UserName == user.UserName;
      return await _userRepository.Filter(filter).FirstOrDefaultAsync();
    }
    public async Task Delete(User user)
    {
      user.Active = false;
      _userRepository.Update(user);
    }

    public async Task<List<Menu>> GetListMenu(Guid userId)
    {
      var userMenuPermission = await _usersMenusPermissionRepository.Filter(x => x.UserId == userId && x.PermissionList.Contains("\"CanView\": true")).Select(x => x.MenuId).ToListAsync();
      var allMenus = await _menuRepository.Filter(x => userMenuPermission.Contains(x.Id)).ToListAsync();
      return allMenus;
    }

    public async Task<List<UserInfo>> Detail(Guid userId)
    {
      return await GetAllUsers(keyword: userId.ToString());
    }

    // first param: success or not, second param: message, third param: reason (if any)
    public async Task<(bool, string, string, UserInfo)> CheckLogin(string username, string password)
    {
      try
      {
        var encryptPassword = Encrypt.getHash(password);
        Expression<Func<User, bool>> filter = (x) => x.UserName == username;
        var findUsersOnUsername = await _userRepository.Filter(filter).AnyAsync();
        if (!findUsersOnUsername)
        {
          return (false, "LoginFailed", "UserNotExisted", null);
        }
        var user = await _userRepository.Filter(filter).Include(x => x.UserTypeUser).ThenInclude(y => y.UserType).FirstOrDefaultAsync(x => x.Password == encryptPassword);
        var mapperUser = user != null ? _mapper.Map<User, UserInfo>(user) : null;
        if (mapperUser != null)
        {
          mapperUser.PermissionList = await updatePermissionList(mapperUser);

          var userType = user.UserTypeUser.FirstOrDefault().UserType;
          mapperUser.Type = userType.Type;
          mapperUser.UserTypeId = userType.Id;
          mapperUser.UserTypeName = userType.UserTypeName;
        }
        return (user != null, user != null ? "LoginSuccess" : "LoginFailed", user != null ? "" : "WrongPassword", mapperUser);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    private async Task<List<Dictionary<string, object>>> updatePermissionList(UserInfo user)
    {
      if (user != null)
      {
        var permissionList = await _usersMenusPermissionRepository
        .Filter(x => x.UserId == user.Id)
        .Select(x => new {
          Permission = JsonConvert.DeserializeObject<Dictionary<string, object>>(Regex.Replace(x.PermissionList, @"\\", "")),
          MenuId = x.MenuId
        })
        .ToListAsync();
        var userPermissionList = new List<Dictionary<string, object>>();
        foreach (var pem in permissionList)
        {
          var permision = pem.Permission;
          permision.Add("MenuId", pem.MenuId);
          userPermissionList.Add(permision);
        }
        return userPermissionList;
      }
      return new List<Dictionary<string, object>>();
    }
  }
}
