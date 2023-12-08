using AutoMapper;
using BusinessAccess.Repository;
using BusinessAccess.Services.Interface;
using Common;
using DataAccess.Model;
using log4net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessAccess.Services.Implement
{
  public class UserService : IUserService
  {
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Menu> _menuRepository;
    private readonly IMapper _mapper;
    private readonly ILog _logger;

    public UserService(IRepository<User> userRepository, IRepository<Menu> menuRepository, IMapper mapper)
    {
      _userRepository = userRepository;
      _menuRepository = menuRepository;
      _logger = LogManager.GetLogger(typeof(UserService));
      _mapper = mapper;
    }

    public async Task<List<User>> GetAllUsers(string keyword, bool checkActive = false)
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
      return await result.ToListAsync();
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

    public async Task<List<Menu>> GetListMenu()
    {
      return await _menuRepository.GetAll().ToListAsync();
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
        var user = await _userRepository.Filter(filter).FirstOrDefaultAsync(x => x.Password == encryptPassword);
        var mapperUser = user != null ? _mapper.Map<User, UserInfo>(user) : null;
        return (user != null, user != null ? "LoginSuccess" : "LoginFailed", user != null ? "" : "WrongPassword", mapperUser);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
