using DataAccess.DBContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.ConfigurationManage
{
  public class AddSystemConfig
  {
    public static Dictionary<string, string> Initialize(IApplicationBuilder applicationBuilder)//LetgoSysContext is EF context
    {
      var scopeFactory = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
      using (var scope = scopeFactory.CreateScope())
      {
        var context = scope.ServiceProvider.GetRequiredService<SampleNetCoreAPIContext>();

        context.Database.EnsureCreated();//if db is not exist ,it will create database .but ,do nothing .

        var configs = context.HotelManagementCoreConfig.ToList();
        var dict = new Dictionary<string, string>();
        foreach (var item in configs)
        {
          dict[item.Key] = item.Value;
        };

        return dict;
      }
    }
  }
}
