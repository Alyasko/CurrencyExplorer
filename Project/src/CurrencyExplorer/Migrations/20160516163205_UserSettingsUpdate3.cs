using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace CurrencyExplorer.Migrations
{
    public partial class UserSettingsUpdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "UserLanguageEntry",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Language",
                table: "UserLanguageEntry",
                nullable: false);
        }
    }
}
