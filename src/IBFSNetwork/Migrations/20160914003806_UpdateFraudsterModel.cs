using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IBFSNetwork.Migrations
{
    public partial class UpdateFraudsterModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Alerts_AlertId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_AlertId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "AlertId",
                table: "Documents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AlertId",
                table: "Documents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_AlertId",
                table: "Documents",
                column: "AlertId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Alerts_AlertId",
                table: "Documents",
                column: "AlertId",
                principalTable: "Alerts",
                principalColumn: "AlertId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
