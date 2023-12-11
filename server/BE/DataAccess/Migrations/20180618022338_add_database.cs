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
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "hotel_management_net_core_config");
    }
  }
}
