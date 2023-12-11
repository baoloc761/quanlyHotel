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

staff:
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI0NjY5NTEwOS0zZWNiLTQxYmUtOGU2OC02MzEwNjg2YWU4ZDEiLCJlbWFpbCI6IjRhYWEzZmE4LTEyOTItNGNhYi1hNDkzLTk1ZGRiMzdiNDI4YSIsInR5cCI6IjIiLCJnaXZlbl9uYW1lIjoic3RhZmYxIiwiZXhwIjoxNzAyMzcwOTM1LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjU5Njg4IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1OTY4OCJ9.af2fF3HJnIL4QGnrCmkw200n0Oe1aoNml3dCIHQ9n3c

admin:
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI0ZTA4ZTBmYS1mN2UwLTQ0MDEtYmY5YS0wM2U4ODMxMTQ0YmEiLCJlbWFpbCI6ImQ1N2IwZjJlLTdkNGYtNDkxZC1iN2M5LTIyMWM2ZDkyYzIwOCIsInR5cCI6IjEiLCJnaXZlbl9uYW1lIjoiYWRtaW4iLCJleHAiOjE3MDIzNzEyOTcsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTk2ODgiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjU5Njg4In0.oZmKXWz0QHfwl2XnWCPxOYN8fC5byQUGTSWkBqZab2Y

manager:
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI0ZjMxYjk3Ny04NTc2LTQxYmEtYjU2MS1iOTk2MGYzZTgxZWMiLCJlbWFpbCI6ImNmMjQ1OTBmLWMyYTQtNDg2MS1iZGQ3LWJhY2VhNWU0NWMxYyIsInR5cCI6IjMiLCJnaXZlbl9uYW1lIjoibWFuYWdlcjEiLCJleHAiOjE3MDIzNzI5NDIsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTk2ODgiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjU5Njg4In0.0mW2BJ1uoppJ_cjYkdrcrUsZ4UnmkUzLpVxkf_YiQ0I