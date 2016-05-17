using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace CurrencyExplorer.Migrations
{
    public partial class UserSettingsUpdate4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_CurrencyCodeEntry_UserSettingsEntry_UserSettingsEntryId", table: "CurrencyCodeEntry");
            migrationBuilder.DropColumn(name: "UserSettingsEntryId", table: "CurrencyCodeEntry");
            migrationBuilder.CreateTable(
                name: "CorrespondanceEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CurrencyCodeId = table.Column<int>(nullable: true),
                    UserSettingsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrespondanceEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorrespondanceEntry_CurrencyCodeEntry_CurrencyCodeId",
                        column: x => x.CurrencyCodeId,
                        principalTable: "CurrencyCodeEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorrespondanceEntry_UserSettingsEntry_UserSettingsId",
                        column: x => x.UserSettingsId,
                        principalTable: "UserSettingsEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("CorrespondanceEntry");
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
    }
}
