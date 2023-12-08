using log4net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Reflection;
using System.Xml;

namespace HotelManagementCore
{
  public class Program
  {
    public static void Main(string[] args)
    {
      XmlDocument log4netConfig = new XmlDocument();
      log4netConfig.Load(File.OpenRead("log4net.config"));
      var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

      log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

      BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>()
            .UseUrls("http://localhost:5000/")//Sử dụng nếu muốn deploy nhiều App trên cùng 1 server, giúp vitual host có thể phân luồng theo call request
            .Build();
  }
}
