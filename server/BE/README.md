Update Starup.cs Constructor to 

```
 public Startup(IHostingEnvironment env)
  {
      var builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json");

      Configuration = builder.Build();

      //var connectionStringConfig = builder.Build();

      /////ADd Config From Database
      //var config = new ConfigurationBuilder()
      //    .SetBasePath(env.ContentRootPath)
      //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
      //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
      //    .AddEnvironmentVariables().AddEntityFrameworkConfig(options =>
      //        options.UseSqlServer(connectionStringConfig.GetConnectionString("MSSQLServerConnection"))
      //     );

      //Configuration = config.Build();
  }
```    

Open Pakage Manager Console and type:
```
update-database
 ```
 
 
 Update Startup.cs to
 
 ```
public Startup(IHostingEnvironment env)
{
     var connectionStringConfig = builder.Build();

      ///ADd Config From Database
      var config = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables().AddEntityFrameworkConfig(options =>
              options.UseSqlServer(connectionStringConfig.GetConnectionString("MSSQLServerConnection"))
           );

      Configuration = config.Build();
}
```
{
  "status": 200,
  "message": "LoginSuccess",
  "userInfo": {
    "userName": "admin",
    "email": "baoloc761@gmail.com",
    "firstName": "Lộc",
    "lastName": "Hoàng Bảo",
    "id": "85ffa034-ac95-ee11-afdb-088fc3524857",
    "active": true,
    "updatedTime": "2023-12-08T09:28:55.452",
    "createdTime": "2023-12-08T09:28:55.452"
  },
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE3MDIxMTk1MTMsImlzcyI6ImFkbWluIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1OTY4OCJ9.FpWQFpbdzVTiKNTQs4EBSi0AJp1mPCul4-4Gbm2eD8o"
}

UserType = new List<string>() { "Administrator" }