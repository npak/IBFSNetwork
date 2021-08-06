using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IBFSNetwork.Migrations
{
    public partial class addFeedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Feeds",
                maxLength: 450,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feeds_UserId",
                table: "Feeds",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feeds_AspNetUsers_UserId",
                table: "Feeds",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feeds_AspNetUsers_UserId",
                table: "Feeds");

            migrationBuilder.DropIndex(
                name: "IX_Feeds_UserId",
                table: "Feeds");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Feeds");
        }
    }
}
