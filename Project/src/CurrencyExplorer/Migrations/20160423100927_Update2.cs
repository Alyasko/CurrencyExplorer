using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace CurrencyExplorer.Migrations
{
    public partial class Update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("CurrencyData");
            migrationBuilder.DropTable("CurrencyCode");
            migrationBuilder.CreateTable(
                name: "CurrencyCodeEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Alias = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyCodeEntry", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "CurrencyDataEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActualDate = table.Column<DateTime>(nullable: false),
                    DbCurrencyCodeEntryId = table.Column<int>(nullable: true),
                    Value = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyDataEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencyDataEntry_CurrencyCodeEntry_DbCurrencyCodeEntryId",
                        column: x => x.DbCurrencyCodeEntryId,
                        principalTable: "CurrencyCodeEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("CurrencyDataEntry");
            migrationBuilder.DropTable("CurrencyCodeEntry");
            migrationBuilder.CreateTable(
                name: "CurrencyCode",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Alias = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyCode", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "CurrencyData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActualDate = table.Column<DateTime>(nullable: false),
                    CurrencyCodeId = table.Column<int>(nullable: false),
                    Value = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencyData_CurrencyCode_CurrencyCodeId",
                        column: x => x.CurrencyCodeId,
                        principalTable: "CurrencyCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }
    }
}
