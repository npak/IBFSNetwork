using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IBFSNetwork.Migrations
{
    public partial class UdateDocModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FraudsterId",
                table: "Documents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_FraudsterId",
                table: "Documents",
                column: "FraudsterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Fraudsters_FraudsterId",
                table: "Documents",
                column: "FraudsterId",
                principalTable: "Fraudsters",
                principalColumn: "FraudsterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Fraudsters_FraudsterId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_FraudsterId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "FraudsterId",
                table: "Documents");
        }
    }
}
