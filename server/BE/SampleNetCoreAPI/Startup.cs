using AutoMapper;
using BusinessAccess.DataContract;
using BusinessAccess.Repository;
using BusinessAccess.Services.Implement;
using BusinessAccess.Services.Interface;
using Common;
using DataAccess.ConfigurationManage;
using DataAccess.DBContext;
using DataAccess.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NotificationLayer;
using SampleNetCoreAPI.Models;
using Security;
using Security.CustomAuthorization;
using Security.Extension;
using System;
using System.IO;
using System.Linq;

namespace HotelManagementCore
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json");

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

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      #region Add connection string to EF

      var sqlConnectionString = Configuration.GetConnectionString("MSSQLServerConnection");
      services.AddDbContext<SampleNetCoreAPIContext>(options => options.UseSqlServer(sqlConnectionString));

      #endregion

      #region ADd Configuration to dependency injection

      services.AddSingleton<IConfiguration>(Configuration);

      #endregion

      #region Add Authorization by using JWT
      services.AddAuthentication(o =>
      {
        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(options =>
      {
        options.Audience = Configuration["TokenAuthentication:siteUrl"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Constant.SecretSercurityKey)),

          ValidateIssuer = true,
          ValidIssuer = Configuration["TokenAuthentication:siteUrl"],

          ValidateAudience = true,
          ValidAudience = Configuration["TokenAuthentication:siteUrl"],

          ValidateLifetime = true,
        };
      });
      #endregion

      #region Create Authorization Role

      var userRoleTypes = Enum.GetValues(typeof(UserTypeEnum)).Cast<UserTypeEnum>().ToList();

      for (int i = 1; i <= userRoleTypes.Count(); i++)
      {
        foreach (var policyNames in userRoleTypes.Combinate(i))
        {
          ///Administrator,Customer
          var policyConcat = string.Join(",", policyNames);
          var result = policyNames.GroupBy(c => c).Where(c => c.Count() > 1).Select(c => new { charName = c.Key, charCount = c.Count() });
          if (result.Count() <= 0)
          {
            services.AddAuthorization(options =>
            {
              options.AddPolicy(policyConcat, policy => policy.Requirements.Add(new CustomAuthoRequire(policyConcat)));
            });
          }
        }
      }
      services.AddSingleton<IAuthorizationHandler, CustomAuthorizationHandle>();
      #endregion

      #region Add Service to dependency injection
      services.AddTransient<IEmailProvider, EmailProvider>();
      services.AddTransient<IUserService, UserService>();
      services.AddTransient<IBlogService, BlogService>();
      services.AddTransient<IAuthozirationUtility, AuthozirationUtility>();
      #endregion

      #region Add Repository

      services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

      #endregion

      #region Add AutoMapper
      ConfigAutoMapper(services);
      #endregion

      #region add cors
      var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
      services.AddCors(options =>
      {
        options.AddPolicy(MyAllowSpecificOrigins,
                              policy =>
                              {
                                policy.WithOrigins("http://localhost:5688", "https://localhost:5689")
                                .AllowAnyOrigin()
                                .AllowCredentials()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                              });
      });
      #endregion

      services.AddMvc();
      services.AddMvcCore().AddApiExplorer();

      #region Enable swagger - api document
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotel Management API", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          In = ParameterLocation.Header,
          Description = "Please enter a valid token",
          Name = "Authorization",
          Type = SecuritySchemeType.Http,
          BearerFormat = "JWT",
          Scheme = "Bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
      });
      services.AddSwaggerGenNewtonsoftSupport();
      #endregion
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      app.UseDeveloperExceptionPage();
      app.UseStaticFiles();
      app.UseAuthentication();
      app.UseMvc(routes =>
      {
        // SwaggerGen won't find controllers that are routed via this technique.
        routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
      });
      var publicUrl = Configuration.GetSection("PublicSettings:publicUrl").Get<string>();
      app.UseCors(
         options => options.WithOrigins(publicUrl)
         .AllowAnyHeader()
         .AllowAnyMethod()
         .SetIsOriginAllowed(origin => true)
         .AllowCredentials()
     );
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel Management API V1");
        c.EnableValidator(null);
      });
    }

    public void ConfigAutoMapper(IServiceCollection services)
    {
      var config = new AutoMapper.MapperConfiguration(cfg =>
      {
        cfg.CreateMap<Blog, BlogContract>();
        cfg.CreateMap<User, UserInfo>();
        cfg.CreateMap<UserInfo, User>();

      });

      IMapper mapper = config.CreateMapper();
      services.AddSingleton<IMapper>(mapper);
    }
  }
}
