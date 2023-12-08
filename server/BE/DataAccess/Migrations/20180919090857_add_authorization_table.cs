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
          name: "user_types",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, defaultValueSql: "newsequentialid()"),
            Active = table.Column<bool>(nullable: false),
            CreatedTime = table.Column<DateTime>(nullable: true),
            Description = table.Column<string>(nullable: true),
            UpdatedTime = table.Column<DateTime>(nullable: true),
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
            Active = table.Column<bool>(nullable: false),
            UserName = table.Column<string>(nullable: false),
            Email = table.Column<string>(nullable: true),
            FirstName = table.Column<string>(nullable: true),
            LastName = table.Column<string>(nullable: true),
            Password = table.Column<string>(nullable: true),
            CreatedTime = table.Column<DateTime>(nullable: true),
            UpdatedTime = table.Column<DateTime>(nullable: true)
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
            Active = table.Column<bool>(nullable: false),
            CreatedTime = table.Column<DateTime>(nullable: true),
            UpdatedTime = table.Column<DateTime>(nullable: true),
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

      migrationBuilder.CreateIndex(
          name: "IX_user_type_users_UserId",
          table: "user_type_users",
          column: "UserId");


      migrationBuilder.CreateIndex(
          name: "IX_user_type_users_UserTypeId",
          table: "user_type_users",
          column: "UserTypeId");

      migrationBuilder.InsertData(
        table: "users",
        columns: new string[] { "Active", "UserName", "Email", "FirstName", "LastName", "Password", "CreatedTime", "UpdatedTime" },
        values: new object[,] {
                   { true, "admin", "baoloc761@gmail.com", "Lộc", "Hoàng Bảo", Encrypt.getHash("123456"), DateTime.UtcNow, DateTime.UtcNow }
        }
      );
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "user_type_users");

      migrationBuilder.DropTable(
          name: "users");

      migrationBuilder.DropTable(
          name: "user_types");
    }
  }
}
