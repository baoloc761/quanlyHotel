using Common;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DataAccess.Migrations
{
  public partial class add_database : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "blogs",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, defaultValueSql: "newsequentialid()"),
            Active = table.Column<bool>(nullable: false),
            CreatedTime = table.Column<DateTime>(nullable: true),
            Name = table.Column<string>(nullable: true),
            UpdatedTime = table.Column<DateTime>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_blogs", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "hotel_management_net_core_config",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, defaultValueSql: "newsequentialid()"),
            Active = table.Column<bool>(nullable: false),
            CreatedTime = table.Column<DateTime>(nullable: true),
            Key = table.Column<string>(nullable: true),
            UpdatedTime = table.Column<DateTime>(nullable: true),
            Value = table.Column<string>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_SampleNetCoreConfig", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "posts",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, defaultValueSql: "newsequentialid()"),
            Active = table.Column<bool>(nullable: false),
            BlogId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
            Content = table.Column<string>(nullable: true),
            CreatedTime = table.Column<DateTime>(nullable: true),
            Title = table.Column<string>(maxLength: 200, nullable: true),
            UpdatedTime = table.Column<DateTime>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_posts", x => x.Id);
            table.ForeignKey(
                      name: "FK_Post_Blog_BlogId",
                      column: x => x.BlogId,
                      principalTable: "blogs",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "FK_Post_Blog_BlogId_idx",
          table: "posts",
          column: "BlogId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
        name: "menus");

      migrationBuilder.DropTable(
          name: "posts");

      migrationBuilder.DropTable(
          name: "hotel_management_net_core_config");

      migrationBuilder.DropTable(
          name: "blogs");
    }
  }
}
