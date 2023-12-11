using Common;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DataAccess.Migrations
{
  public partial class add_authorization_table : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: "menus",
        columns: table => new
        {
          Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, defaultValueSql: "newsequentialid()"),
          Active = table.Column<bool>(nullable: false, defaultValue: true),
          Icon = table.Column<string>(nullable: true),
          Path = table.Column<string>(nullable: true),
          Description = table.Column<string>(nullable: true),
          Title = table.Column<string>(nullable: true),
          CreatedTime = table.Column<DateTime>(nullable: true, defaultValue: DateTime.UtcNow),
          UpdatedTime = table.Column<DateTime>(nullable: true, defaultValue: DateTime.UtcNow)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_menus", x => x.Id);
        });

      migrationBuilder.CreateTable(
          name: "user_types",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, defaultValueSql: "newsequentialid()"),
            Active = table.Column<bool>(nullable: false, defaultValue: true),
            CreatedTime = table.Column<DateTime>(nullable: true, defaultValue: DateTime.UtcNow),
            Description = table.Column<string>(nullable: true),
            UpdatedTime = table.Column<DateTime>(nullable: true, defaultValue: DateTime.UtcNow),
            Type = table.Column<int>(nullable: false, defaultValue: (int)UserTypeEnum.Staff),
            UserTypeName = table.Column<string>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_user_types", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "users",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, defaultValueSql: "newsequentialid()"),
            Active = table.Column<bool>(nullable: false, defaultValue: true),
            UserName = table.Column<string>(nullable: false),
            Email = table.Column<string>(nullable: true),
            FirstName = table.Column<string>(nullable: true),
            LastName = table.Column<string>(nullable: true),
            Password = table.Column<string>(nullable: true),
            CreatedTime = table.Column<DateTime>(nullable: true, defaultValue: DateTime.UtcNow),
            UpdatedTime = table.Column<DateTime>(nullable: true, defaultValue: DateTime.UtcNow)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_users", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "user_type_users",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, defaultValueSql: "newsequentialid()"),
            Active = table.Column<bool>(nullable: false, defaultValue: true),
            CreatedTime = table.Column<DateTime>(nullable: true, defaultValue: DateTime.UtcNow),
            UpdatedTime = table.Column<DateTime>(nullable: true, defaultValue: DateTime.UtcNow),
            UserId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
            UserTypeId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_user_type_users", x => x.Id);
            table.ForeignKey(
                      name: "FK_user_type_users_users_UserId",
                      column: x => x.UserId,
                      principalTable: "users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_user_type_users_user_types_UserTypeId",
                      column: x => x.UserTypeId,
                      principalTable: "user_types",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "users_menus_permissions",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, defaultValueSql: "newsequentialid()"),
            Active = table.Column<bool>(nullable: false, defaultValue: true),
            CreatedTime = table.Column<DateTime>(nullable: true, defaultValue: DateTime.UtcNow),
            UpdatedTime = table.Column<DateTime>(nullable: true, defaultValue: DateTime.UtcNow),
            UserId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
            MenuId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
            PermissionList = table.Column<string>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_users_menus_permissions", x => x.Id);
            table.ForeignKey(
                      name: "FK_users_menus_permissions_users_UserId",
                      column: x => x.UserId,
                      principalTable: "users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_users_menus_permissions_menus_MenuId",
                      column: x => x.MenuId,
                      principalTable: "menus",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_user_type_users_UserId",
          table: "user_type_users",
          column: "UserId");


      migrationBuilder.CreateIndex(
          name: "IX_user_type_users_UserTypeId",
          table: "user_type_users",
          column: "UserTypeId");

      migrationBuilder.CreateIndex(
          name: "IX_users_menus_permissions_UserId",
          table: "users_menus_permissions",
          column: "UserId");

      migrationBuilder.CreateIndex(
          name: "IX_users_menus_permissions_MenuId",
          table: "users_menus_permissions",
          column: "MenuId");

      var userId = Guid.NewGuid();
      var userId2 = Guid.NewGuid();
      var userId3 = Guid.NewGuid();
      var userTypeIdStaft = Guid.NewGuid();
      var userTypeIdManager = Guid.NewGuid();
      var userTypeIdAdmin = Guid.NewGuid();

      var menu1 = Guid.NewGuid();
      var menu2 = Guid.NewGuid();
      var menu3 = Guid.NewGuid();

      migrationBuilder.InsertData(
      table: "menus",
      columns: new string[] { "Id", "Icon", "Path", "Description", "Title" },
      values: new object[,] {
                   { menu1, "", "/home", "Pages.Home.description", "Pages.Home.title" },
                   { menu2, "", "/account/users", "Pages.UserManagement.description", "Pages.UserManagement.title" },
                   { menu3, "", "/account/users/Authorization", "Pages.UserAuthorization.description", "Pages.UserAuthorization.title" }
        }
      );

      migrationBuilder.InsertData(
        table: "users",
        columns: new string[] { "Id", "Active", "UserName", "Email", "FirstName", "LastName", "Password" },
        values: new object[,] {
                   { userId, true, "admin", "baoloc761@gmail.com", "Lộc", "Hoàng Bảo", Encrypt.getHash("123456") },
                   { userId2, true, "manager1", "manager1@gmail.com", "test", "manager 1", Encrypt.getHash("123456") },
                   { userId3, true, "staff1", "staff1@gmail.com", "test", "staff 1", Encrypt.getHash("123456") }
        }
      );

      migrationBuilder.InsertData(
        table: "users_menus_permissions",
        columns: new string[] { "UserId", "MenuId", "PermissionList" },
        values: new object[,] {
                   { userId, menu1, "{ \"CanView\": true, \"CanEdit\": true, \"CanDelete\": true, \"CanReport\": true}" },
                   { userId, menu2 , "{ \"CanView\": true, \"CanEdit\": true, \"CanDelete\": true, \"CanReport\": true}"},
                   { userId, menu3 , "{ \"CanView\": true, \"CanEdit\": true, \"CanDelete\": true, \"CanReport\": true}"},
                   { userId2, menu1, "{ \"CanView\": true, \"CanEdit\": true, \"CanDelete\": true, \"CanReport\": true}" },
                   { userId2, menu2 , "{ \"CanView\": true, \"CanEdit\": true, \"CanDelete\": true, \"CanReport\": true}"},
                   { userId2, menu3 , "{ \"CanView\": true, \"CanEdit\": true, \"CanDelete\": true, \"CanReport\": true}"},
                   { userId3, menu1, "{ \"CanView\": true, \"CanEdit\": true, \"CanDelete\": false, \"CanReport\": false}" },
                   { userId3, menu2 , "{ \"CanView\": false, \"CanEdit\": false, \"CanDelete\": false, \"CanReport\": false}"},
                   { userId3, menu3 , "{ \"CanView\": true, \"CanEdit\": true, \"CanDelete\": false, \"CanReport\": false}"}
        }
      );

      migrationBuilder.InsertData(
        table: "user_types",
        columns: new string[] { "Id", "Description", "Type", "UserTypeName" },
        values: new object[,] {
                   { userTypeIdStaft, "Staff", (int)UserTypeEnum.Staff, "Staff" },
                   { userTypeIdAdmin, "Administrator", (int)UserTypeEnum.Administrator, "Administrator" },
                   { userTypeIdManager, "Manager", (int)UserTypeEnum.Manager, "Manager" }
        }
      );

      migrationBuilder.InsertData(
        table: "user_type_users",
        columns: new string[] { "UserId", "UserTypeId" },
        values: new object[,] {
                   { userId, userTypeIdAdmin },
                   { userId2, userTypeIdManager },
                   { userId3, userTypeIdStaft }
        }
      );
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
        name: "menus");

      migrationBuilder.DropTable(
          name: "user_type_users");

      migrationBuilder.DropTable(
          name: "users");

      migrationBuilder.DropTable(
          name: "user_types");

      migrationBuilder.DropTable(
          name: "users_menus_permissions");
    }
  }
}
