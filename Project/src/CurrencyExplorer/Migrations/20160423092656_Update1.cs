using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace CurrencyExplorer.Migrations
{
    public partial class Update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_CurrencyData_CurrencyCode_CurrencyCodeId", table: "CurrencyData");
            migrationBuilder.DropColumn(name: "ActualDateString", table: "CurrencyData");
            migrationBuilder.DropColumn(name: "Name", table: "CurrencyData");
            migrationBuilder.DropColumn(name: "ShortName", table: "CurrencyData");
            migrationBuilder.AlterColumn<int>(
                name: "CurrencyCodeId",
                table: "CurrencyData",
                nullable: false);
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CurrencyCode",
                nullable: true);
            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyData_CurrencyCode_CurrencyCodeId",
                table: "CurrencyData",
                column: "CurrencyCodeId",
                principalTable: "CurrencyCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_CurrencyData_CurrencyCode_CurrencyCodeId", table: "CurrencyData");
            migrationBuilder.DropColumn(name: "Name", table: "CurrencyCode");
            migrationBuilder.AlterColumn<int>(
                name: "CurrencyCodeId",
                table: "CurrencyData",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "ActualDateString",
                table: "CurrencyData",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CurrencyData",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "CurrencyData",
                nullable: true);
            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyData_CurrencyCode_CurrencyCodeId",
                table: "CurrencyData",
                column: "CurrencyCodeId",
                principalTable: "CurrencyCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
