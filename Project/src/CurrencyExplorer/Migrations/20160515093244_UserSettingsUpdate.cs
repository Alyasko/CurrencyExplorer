using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace CurrencyExplorer.Migrations
{
    public partial class UserSettingsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserLanguageEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Language = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLanguageEntry", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "UserSettingsEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChartBeginTime = table.Column<DateTime>(nullable: false),
                    ChartEndTime = table.Column<DateTime>(nullable: false),
                    CookieUid = table.Column<long>(nullable: false),
                    LanguageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettingsEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSettingsEntry_UserLanguageEntry_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "UserLanguageEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.AddColumn<int>(
                name: "UserSettingsEntryId",
                table: "CurrencyCodeEntry",
                nullable: true);
            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyCodeEntry_UserSettingsEntry_UserSettingsEntryId",
                table: "CurrencyCodeEntry",
                column: "UserSettingsEntryId",
                principalTable: "UserSettingsEntry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_CurrencyCodeEntry_UserSettingsEntry_UserSettingsEntryId", table: "CurrencyCodeEntry");
            migrationBuilder.DropColumn(name: "UserSettingsEntryId", table: "CurrencyCodeEntry");
            migrationBuilder.DropTable("UserSettingsEntry");
            migrationBuilder.DropTable("UserLanguageEntry");
        }
    }
}
